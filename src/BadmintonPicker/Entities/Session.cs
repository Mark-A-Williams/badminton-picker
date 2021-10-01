using System;

namespace BadmintonPicker.Entities
{
    internal class Session
    {
        public int Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public IEnumerable<PlayerSession> PlayerSessions { get; set; } = default!;
    }
}
