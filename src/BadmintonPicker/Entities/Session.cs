using System;
using System.Collections.Generic;

namespace BadmintonPicker.Entities
{
    internal class Session
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public ICollection<PlayerSession> PlayerSessions { get; set; } = new List<PlayerSession>();
    }
}
