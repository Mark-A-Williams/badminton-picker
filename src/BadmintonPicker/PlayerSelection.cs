using BadmintonPicker.Entities;

namespace BadmintonPicker
{
    internal class PlayerSelection
    {
        public string Initials { get; set; } = default!;
        public Status Status { get; set; }
        public double Weighting { get; set; }
    }
}
