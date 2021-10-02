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

        public async Task<IEnumerable<Session>> GetRecentSessions(int numberToGet)
        {
            return await _appDbContext.Sessions
                .Include(s => s.PlayerSessions)
                .ThenInclude(ps => ps.Player)
                .Where(s => s.Date < DateTimeOffset.Now.Date)
                .OrderByDescending(s => s.Date)
                .Take(numberToGet)
                .ToArrayAsync();
        }

        public async Task<bool> GetIfSessionExistsWithSameDate(Session session)
        {
            return await _appDbContext.Sessions
                .Where(s => s.Date.Date == session.Date.Date)
                .AnyAsync();
        }
    }
}
