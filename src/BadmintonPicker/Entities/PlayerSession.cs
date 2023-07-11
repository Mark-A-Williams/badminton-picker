using System.ComponentModel.DataAnnotations;

namespace BadmintonPicker.Entities
{
    internal class PlayerSession
    {
        [Required]
        public required string PlayerId { get; set; }

        [Required]
        public Player? Player { get; set; }

        [Required]
        public int SessionId { get; set; }

        [Required]
        public Session? Session { get; set; }

        public Status Status { get; set; }
    }
}
