using BadmintonPicker.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BadmintonPicker
{
    public static class Program
    {
        public static void Main()
        {            
            var serviceProvider = new ServiceCollection()
                .AddDbContext<AppDbContext>()
                .BuildServiceProvider();

            var foo = serviceProvider.GetRequiredService<AppDbContext>().Sessions.FirstOrDefault();
            Console.Write(foo.Date);
        }
    }
}
