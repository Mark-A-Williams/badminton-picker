namespace BadmintonPicker.Entities
{
    internal class PlayerSession
    {
        public string PlayerId { get; set; } = default!;
        public Player Player { get; set; } = default!;
        public int SessionId { get; set; }
        public Session Session { get; set; } = default!;
        public Status Status { get; set; }
    }
}
