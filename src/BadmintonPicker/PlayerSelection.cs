using BadmintonPicker.Entities;

namespace BadmintonPicker
{
    internal class PlayerSelection
    {
        public required string Initials { get; init; }
        public Status Status { get; init; }
        public double Weighting { get; init; }
    }
}
