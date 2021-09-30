using BadmintonPicker.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BadmintonPicker.Queries
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
    }
}
