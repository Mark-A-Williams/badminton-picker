using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace BadmintonPicker.Entities
{
    internal class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Server=(local);Database=BadmintonPicker;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PlayerSession>().HasKey(ps => new
            {
                ps.PlayerId,
                ps.SessionId
            });

            builder.Entity<Player>().HasIndex(p => new { p.FirstName, p.LastName }).IsUnique();
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            builder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>()
                .HaveColumnType("date");
        }

        public DbSet<Player> Players => Set<Player>();
        public DbSet<Session> Sessions => Set<Session>();
        public DbSet<PlayerSession> PlayerSessions => Set<PlayerSession>();

        private class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
        {
            public DateOnlyConverter() : base(
                    d => d.ToDateTime(TimeOnly.MinValue),
                    d => DateOnly.FromDateTime(d))
            { }
        }
    }
}
