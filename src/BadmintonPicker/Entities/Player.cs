using System.Collections.Generic;

namespace BadmintonPicker.Entities
{
    internal class Player
    {
        public int Id { get; set; }
        public string Initials { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<PlayerSession> PlayerSessions { get; set; } = default!;

        public Player(string initials, string firstName, string lastName)
        {
            Initials = initials;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}