namespace BadmintonPicker
{
    internal static class Constants
    {
        public const int PlayerLimit = 12;
        public const DayOfWeek BadmintonDay = DayOfWeek.Wednesday;
        public const int WeeksToLookBackForWeightings = 3;
        public const int WeightingMultiplierForLastWeek = 2;

        public const double WeightingForNewPlayer = 10;
        public const double WeightingForNotSelected = 1;
        public const double WeightingForDidNotSignUp = 0.25;
        public const double WeightingForDroppedOut = -0.5;
        public const double WeightingRandomComponent = 0.05;
    }
}
