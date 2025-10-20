
namespace UtilityCS
{
    public static class RandomHandler
    {
        private static RandomCS random = new RandomCS();
        private static RandomSecure randomSecure = new RandomSecure();

        public static void SetSeed(ulong seed) => random.SetSeed(seed);
        public static void RemoveSeed(ulong seed) => random.RemoveSeed();
        public static int Range(int min, int max) => random.Next(min, max);
        public static double Range(double min, double max) => random.Next(min, max);
        public static bool CoinFlip() => random.CoinFlip();
        public static bool Chance(float threshold, float max = 1.0f) => random.Chance(threshold, max);
        public static double NextDouble() => random.Next(0.0, 1.0);

        public static class Secure
        {
            public static int Range(int min, int max) => randomSecure.Next(min, max);
            public static double Range(double min, double max) => randomSecure.Next(min, max);
            public static bool CoinFlip() => randomSecure.CoinFlip();
            public static bool Chance(float threshold, float max = 1.0f) => randomSecure.Chance(threshold, max);
            public static double NextDouble() => randomSecure.Next(0.0, 1.0);
        }
    }
}
