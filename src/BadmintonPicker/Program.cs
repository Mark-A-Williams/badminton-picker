using BadmintonPicker.Entities;
using BadmintonPicker.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BadmintonPicker
{
    public static class Program
    {
        public static async Task Main()
        {            
            var serviceProvider = new ServiceCollection()
                .AddDbContext<AppDbContext>()
                .AddSingleton<DbQueries>()
                .BuildServiceProvider();

            var foo = serviceProvider.GetRequiredService<AppDbContext>().Sessions.FirstOrDefault();
            await Run(serviceProvider);
        }

        public static async Task Run(ServiceProvider serviceProvider)
        {
            var dbQueries = serviceProvider.GetRequiredService<DbQueries>();

            Console.WriteLine("Welcome to badminton picker! Please select one of the following options:");
            
            while (true)
            {
                WriteInstructions();
                await HandleUserInput(dbQueries);
            }
        }

        private static async Task HandleUserInput(DbQueries dbQueries)
        {
            var userInput = Console.ReadLine();
            if (int.TryParse(userInput, out var number))
            {
                switch (number)
                {
                    case 1:
                        await ShowPlayersList(dbQueries);
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
            Console.WriteLine("2 - View the five most recent sessions");
            Console.WriteLine("3 - Create a new session");
            Console.WriteLine("4 - Get the suggested players for the next upcoming session");
            Console.WriteLine("5 - Substitute a player for the next upcoming");
            Console.ResetColor();
        }

        private static async Task ShowPlayersList(DbQueries dbQueries)
        {
            var alphabeticalPlayers = (await dbQueries.GetAllPlayers())
                .OrderBy(o => o.Initials);

            foreach (var player in alphabeticalPlayers)
            {
                Console.WriteLine($"[{player.Initials}] {player.FirstName} {player.LastName}");
            }
        }

        private static async Task ShowRecentSessions()
        {
            throw new NotImplementedException();
        }

        private static async Task CreateNewSession()
        {
            throw new NotImplementedException();
        }

        private static async Task SelectPlayersForUpcomingSession()
        {
            throw new NotImplementedException();
        }

        private static async Task SubstitutePlayerForUpcomingSession()
        {
            throw new NotImplementedException();
        }
    }
}
