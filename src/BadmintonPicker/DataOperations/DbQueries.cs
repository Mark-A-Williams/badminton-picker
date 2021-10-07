using BadmintonPicker.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
                .FirstOrDefaultAsync(o => o.FirstName == components[0] && o.LastName == components[1]);
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
    }
}
