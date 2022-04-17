using System;

namespace BadmintonPicker
{
    internal static class Constants
    {
        public const int PlayerLimit = 16;
        public const DayOfWeek BadmintonDay = DayOfWeek.Wednesday;
        public const int WeeksToLookBackForWeightings = 3;
        public const int WeightingMultiplierForLastWeek = 2;

        public const double WeightingForNewPlayer = 10;
        public const double WeightingForNotSelected = 1;
        public const double WeightingForSubbedInShortNotice = 1;
        public const double WeightingForDidNotSignUp = 0.25;
        public const double WeightingForDroppedOutShortNotice = -0.5;
        public const double WeightingRandomComponent = 0.05;
    }
}
