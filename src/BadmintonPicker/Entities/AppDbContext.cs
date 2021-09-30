using Microsoft.EntityFrameworkCore;

namespace BadmintonPicker.Entities
{
    internal class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Server=(local);Database=BadmintonPicker;Trusted_Connection=True");
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

        public DbSet<Player> Players { get; set; } = default!;
        public DbSet<Session> Sessions { get; set; } = default!;
        public DbSet<PlayerSession> PlayerSessions { get; set; } = default!;
    }
}
