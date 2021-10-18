using BadmintonPicker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonPicker.DataOperations
{
    internal class DbCommands
    {
        private readonly AppDbContext _appDbContext;

        public DbCommands(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddSession(Session session)
        {
            _appDbContext.Sessions.Add(session);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task CommitPlayerSelection(Session session, ICollection<PlayerSelection> playerSelections)
        {
            foreach (var selection in playerSelections)
            {
                _appDbContext.PlayerSessions.Add(new PlayerSession
                {
                    PlayerId = selection.Initials,
                    SessionId = session.Id,
                    Status = selection.Status
                });
            }

            await _appDbContext.SaveChangesAsync();
        }
    }
}
