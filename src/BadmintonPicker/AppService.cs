using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BadmintonPicker.DataOperations;
using BadmintonPicker.Entities;

namespace BadmintonPicker
{
    internal class AppService
    {
        private readonly DbQueries _dbQueries;
        private readonly DbCommands _dbCommands;

        public AppService(
            DbQueries dbQueries,
            DbCommands dbCommands)
        {
            _dbQueries = dbQueries;
            _dbCommands = dbCommands;
        }

        public async Task Run()
        {
            Console.WriteLine("Welcome to badminton picker! Please select one of the following options:");

            while (true)
            {
                WriteInstructions();
                await HandleUserInput();
            }
        }

        private async Task HandleUserInput()
        {
            var userInput = Console.ReadLine();
            if (int.TryParse(userInput, out var number))
            {
                switch (number)
                {
                    case 1:
                        await ShowPlayersList();
                        break;
                    case 2:
                        await ShowRecentSessions();
                        break;
                    case 3:
                        await CreateNewSession();
                        break;
                    case 4:
                        await SelectPlayersForUpcomingSession();
                        break;
                    default:
                        break;
                }
            }
        }

        private static void WriteInstructions()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("1 - View players list");
            Console.WriteLine("2 - View the five most recent sessions"); // TODO use generic number
            Console.WriteLine("3 - Create a new session");
            Console.WriteLine("4 - Get the suggested players for the next upcoming session");
            Console.WriteLine("5 - Substitute a player for the next upcoming");
            Console.ResetColor();
        }

        private async Task ShowPlayersList()
        {
            var alphabeticalPlayers = (await _dbQueries.GetAllPlayers())
                .OrderBy(o => o.Initials);

            foreach (var player in alphabeticalPlayers)
            {
                Console.WriteLine($"[{player.Initials}] {player.FirstName} {player.LastName}");
            }
        }

        private async Task ShowRecentSessions()
        {
            // WIP but functional
            var recentSessions = await _dbQueries.GetRecentSessions(numberToGet: 5);

            foreach (var session in recentSessions)
            {
                Console.WriteLine($"Session date: {session.Date:yyyy-MM-dd}");
                foreach (var playerSession in session.PlayerSessions
                    .OrderByDescending(o => DidPlayerPlay(o.Status))
                    .ThenBy(o => o.Status)
                    .ThenBy(o => o.PlayerId))
                {
                    Console.ForegroundColor = playerSession.Status switch
                    {
                        Status.SubbedInNormal => ConsoleColor.DarkBlue,
                        Status.SubbedInShortNotice => ConsoleColor.Blue,
                        Status.NotSelected => ConsoleColor.Yellow,
                        Status.DroppedOutShortNotice => ConsoleColor.DarkRed,
                        Status.DroppedOutNormal => ConsoleColor.Gray,
                        _ => ConsoleColor.DarkGreen,
                    };

                    Console.WriteLine($"{playerSession.Player.FirstName} {playerSession.Player.LastName} - {playerSession.Status}");
                    Console.ResetColor();
                }

                WriteLineBreak();
            }
        }

        private async Task CreateNewSession()
        {
            Console.WriteLine("Press enter to create a session on the upcoming Wednesday, or enter a date");
            
            var input = Console.ReadLine();
            Session session;

            if (!string.IsNullOrEmpty(input))
            {
                if (DateTimeOffset.TryParse(input, out var result))
                {
                    session = new Session { Date = result };
                }
                else
                {
                    return;
                }
            }
            else
            {
                session = new Session { Date = GetNextSessionDate() };
            }

            if (await _dbQueries.GetIfSessionExistsWithSameDate(session))
            {
                Console.WriteLine("A session already exists with the same date as this. Are you sure you want to add another?");

                var anotherInputIGuess = Console.ReadLine();
                var shouldProceed = char.TryParse(anotherInputIGuess, out var character) && character == 'y';

                if (!shouldProceed)
                {
                    return;
                }
            }
            await _dbCommands.AddSession(session);
            Console.WriteLine($"New session created for {session.Date:yyyy-MM-dd}");
        }

        private async Task SelectPlayersForUpcomingSession()
        {
            Console.WriteLine(
                "Enter a comma separated list of the players who have signed up (copy paste from Polly in Teams)");

            var commaSeparatedPlayers = Console.ReadLine();
            var playerNames = commaSeparatedPlayers?.Split(", ");

            if (playerNames == null || !playerNames.Any())
            {
                Console.WriteLine("No players entered");
                return;
            }

            var playerWeightings = new Dictionary<string, double>();

            foreach (var playerName in playerNames)
            {
                // Yeah I know it's inefficient
                var player = await _dbQueries.GetPlayerByFullName(playerName);
                if (player == null)
                {
                    Console.WriteLine($"No player exists with the name '{playerName}'");
                    return;
                }

                var initialWeighting = player.PlayerSessions.Any(o => o.Status != Status.NotSelected)
                    ? 0
                    : Constants.WeightingForNewPlayer; // Huge weighting for new players

                playerWeightings.Add(player.Initials, initialWeighting);
            }

            // Ordered by date, most recent last
            var recentSessions = await _dbQueries.GetRecentSessions(
                numberOfWeeksToLookBack: Constants.WeeksToLookBackForWeightings);

            for (int i = 0; i < recentSessions.Count; i++)
            {
                var session = recentSessions[i];
                foreach (var playerInitials in playerWeightings.Keys)
                {
                    var playerStatus = session.PlayerSessions
                        .SingleOrDefault(o => o.Player.Initials == playerInitials)?
                        .Status;

                    var weightingForSession = 0D;

                    if (playerStatus == null)
                    {
                        // The player didn't sign up - low additional priority
                        weightingForSession = Constants.WeightingForDidNotSignUp;
                    }
                    else if (playerStatus == Status.NotSelected)
                    {
                        // High priority to those who weren't selected
                        weightingForSession = Constants.WeightingForNotSelected;
                    }
                    else if (playerStatus == Status.SubbedInShortNotice)
                    {
                        //High priority to those who subbed in at short notice e.g. on the day
                        weightingForSession = Constants.WeightingForSubbedInShortNotice;
                    }
                    else if (playerStatus == Status.DroppedOutShortNotice)
                    {
                        // Penalty for dropping out with short notice
                        weightingForSession = Constants.WeightingForDroppedOutShortNotice;
                    }

                    if (i == recentSessions.Count - 1)
                    {
                        // Weight most recent week more
                        weightingForSession *= Constants.WeightingMultiplierForLastWeek;
                    }

                    var rng = new Random();
                    playerWeightings[playerInitials] += weightingForSession + (rng.NextDouble() * Constants.WeightingRandomComponent);
                }
            }

            var playerSelections = playerWeightings
                .OrderByDescending(o => o.Value)
                .Select((o, position) => new PlayerSelection
                {
                    Initials = o.Key,
                    Status = position < Constants.PlayerLimit ? Status.Selected : Status.NotSelected,
                    Weighting = o.Value
                })
                .ToList();

            Console.WriteLine("Selected players:");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            foreach (var selection in playerSelections.Where(o => o.Status == Status.Selected))
            {
                Console.WriteLine($"{selection.Initials}: {selection.Weighting}");
            }

            Console.ResetColor();
            Console.WriteLine("Players not selected:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (var selection in playerSelections.Where(o => o.Status == Status.NotSelected))
            {
                Console.WriteLine($"{selection.Initials}: {selection.Weighting}");
            }

            Console.ResetColor();
            Console.WriteLine("Commit selection to database?");

            var input = Console.ReadLine();
            var shouldProceed = char.TryParse(input, out var character) && character == 'y';

            if (!shouldProceed)
            {
                return;
            }

            var upcomingSession = await _dbQueries.GetUpcomingSession();
            await _dbCommands.CommitPlayerSelection(upcomingSession, playerSelections);

            Console.WriteLine("Player selections successfully committed.");
            WriteLineBreak();
        }

        private static async Task SubstitutePlayerForUpcomingSession()
        {
            throw new NotImplementedException();
        }

        private static DateTimeOffset GetNextSessionDate()
        {
            for (int i = 0; i < 7; i++)
            {
                var day = DateTimeOffset.Now.Date.AddDays(i);
                if (day.DayOfWeek == Constants.BadmintonDay)
                {
                    return day;
                }
            }

            throw new Exception("What is this, a leap week?");
        }

        private static void WriteLineBreak()
        {
            Console.WriteLine("===============");
        }

        private static bool DidPlayerPlay(Status status)
        {
            return status switch
            {
                Status.Selected or Status.SubbedInNormal or Status.SubbedInShortNotice => true,
                Status.NotSelected or Status.DroppedOutNormal or Status.DroppedOutShortNotice => false,
                _ => throw new NotSupportedException()
            };
        }
    }
}
