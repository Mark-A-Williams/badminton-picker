﻿using System;
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
            var sessionsToShow = 5;
            var recentSessions = await _dbQueries.GetRecentSessions(sessionsToShow);

            foreach (var session in recentSessions)
            {
                Console.WriteLine($"Session date: {session.Date:yyyy-MM-dd}");
                foreach (var playerSession in session.PlayerSessions.OrderBy(o => o.Status))
                {
                    switch (playerSession.Status)
                    {
                        case Status.Selected:
                        case Status.SubbedIn:
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            break;
                        case Status.NotSelected:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case Status.DroppedOut:
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            break;
                        default:
                            break;
                    }

                    Console.WriteLine($"{playerSession.Player.FirstName} {playerSession.Player.LastName} - {playerSession.Status}");
                    Console.ResetColor();
                }
                Console.WriteLine("===============");
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
                session = new Session { Date = GetNextWednesday() };
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
            var playerLimit = 8; // TODO Generalise
            var weeksToLookBack = 3;

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
                playerWeightings.Add(player.Initials, 0);
            }

            var recentSessions = await _dbQueries.GetRecentSessions(weeksToLookBack);
            foreach (var session in recentSessions)
            {
                foreach (var playerInitials in playerWeightings.Keys)
                {
                    var playerStatus = session.PlayerSessions
                        .SingleOrDefault(o => o.Player.Initials == playerInitials)?
                        .Status;

                    if (playerStatus == null)
                    {
                        playerWeightings[playerInitials] += 0.5;
                    }
                    else if (playerStatus == Status.NotSelected)
                    {
                        playerWeightings[playerInitials] += 1;
                    }
                    else if (playerStatus == Status.DroppedOut)
                    {
                        playerWeightings[playerInitials] -= 0.5;
                    }

                    var rng = new Random();
                    playerWeightings[playerInitials] += rng.NextDouble() * 0.1;
                }
            }

            // TODO actually commit this to DB
            foreach (var player in playerWeightings.OrderByDescending(o => o.Value))
            {
                Console.WriteLine($"{player.Key}: {player.Value}");
            }
        }

        private static async Task SubstitutePlayerForUpcomingSession()
        {
            throw new NotImplementedException();
        }

        private static DateTimeOffset GetNextWednesday()
        {
            for (int i = 0; i < 7; i++)
            {
                var day = DateTimeOffset.Now.Date.AddDays(i);
                if (day.DayOfWeek == DayOfWeek.Wednesday)
                {
                    return day;
                }
            }

            throw new Exception("What is this, a leap week?");
        }
    }
}
