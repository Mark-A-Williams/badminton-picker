using System;
using System.Threading.Tasks;
using BadmintonPicker.DataOperations;
using BadmintonPicker.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace BadmintonPicker
{
    public class Program
    {
        public static async Task Main()
        {            
            var serviceProvider = new ServiceCollection()
                .AddDbContext<AppDbContext>()
                .AddSingleton<AppService>()
                .AddSingleton<DbQueries>()
                .AddSingleton<DbCommands>()
                .BuildServiceProvider();

            var appService = serviceProvider.GetService<AppService>();
            
            _ = appService ?? throw new ArgumentException(nameof(appService));

            await appService.Run();
        }
    }
}
