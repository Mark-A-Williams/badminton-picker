using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BadmintonPicker.Entities
{
    internal class Player
    {
        [Key]
        public string Initials { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<PlayerSession> PlayerSessions { get; set; } = new List<PlayerSession>();
        public bool Retired { get; set; }

        public Player(string initials, string firstName, string lastName)
        {
            Initials = initials;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}