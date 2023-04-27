using BadmintonPicker.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BadmintonPicker.DataOperations
{
    internal class DbQueries
    {
        private readonly AppDbContext _appDbContext;

        public DbQueries(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Player>> GetAllPlayers()
        {
            return await _appDbContext.Players.ToArrayAsync();
        }

        public async Task<Player?> GetPlayerByFullName(string fullName)
        {
            var components = fullName.Split(' ');
            return await _appDbContext.Players
                .Include(p => p.PlayerSessions)
                .FirstOrDefaultAsync(o => o.FirstName == components[0] && o.LastName == components[1]);
        }

        public async Task<IList<Session>> GetRecentSessions(
            int numberToGet = 3,
            int numberOfWeeksToLookBack = 100)
        {
            var oldestToTake = DateOnly.FromDateTime(DateTimeOffset.Now.Date).AddDays(-7 * numberOfWeeksToLookBack);

            return await _appDbContext.Sessions
                .OrderByDescending(s => s.Date)
                .Include(s => s.PlayerSessions)
                .ThenInclude(ps => ps.Player)
                .Where(s => s.Date > oldestToTake)
                .Take(numberToGet)
                .OrderBy(s => s.Date)
                .ToListAsync();
        }

        public async Task<bool> GetIfSessionExistsWithSameDate(Session session)
        {
            return await _appDbContext.Sessions
                .Where(s => s.Date == session.Date)
                .AnyAsync();
        }

        public async Task<Session> GetUpcomingSession()
        {
            // TODO account for possibility of multiple future sessions already existing
            // and also of none existing?
            return await _appDbContext.Sessions
                .Where(s => s.Date >= DateOnly.FromDateTime(DateTimeOffset.Now.Date))
                .FirstAsync();
        }
    }
}
