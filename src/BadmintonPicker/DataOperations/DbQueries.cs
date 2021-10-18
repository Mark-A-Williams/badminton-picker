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

        public async Task<IList<Session>> GetRecentSessions(int numberToGet)
        {
            return await _appDbContext.Sessions
                .Include(s => s.PlayerSessions)
                .ThenInclude(ps => ps.Player)
                .Where(s => s.Date < DateTimeOffset.Now.Date)
                .OrderBy(s => s.Date)
                .Take(numberToGet)
                .ToListAsync();
        }

        public async Task<bool> GetIfSessionExistsWithSameDate(Session session)
        {
            return await _appDbContext.Sessions
                .Where(s => s.Date.Date == session.Date.Date)
                .AnyAsync();
        }

        public async Task<Session> GetUpcomingSession()
        {
            // TODO account for possibility of multiple future sessions already existing
            // and also of none existing?
            return await _appDbContext.Sessions
                .Where(s => s.Date >= DateTimeOffset.Now.Date)
                .FirstOrDefaultAsync();
        }
    }
}
