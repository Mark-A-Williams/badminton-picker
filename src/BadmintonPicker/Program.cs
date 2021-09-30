using BadmintonPicker.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

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
