using System;
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
                foreach (var playerSession in session.PlayerSessions)
                {
                    Console.WriteLine($"{playerSession.Player.FirstName} {playerSession.Player.LastName} - {playerSession.Status}");
                }
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

            // TODO handle if there is already one with this date
            await _dbCommands.AddSession(session);
            Console.WriteLine($"New session created for {session.Date:yyyy-MM-dd}");
        }

        private static async Task SelectPlayersForUpcomingSession()
        {
            throw new NotImplementedException();
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
