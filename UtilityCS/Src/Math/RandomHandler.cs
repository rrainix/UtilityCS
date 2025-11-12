using BenScr.Security.Cryptography;

namespace BenScr.Random
{
    public static class RandomHandler
    {
        private static RandomCS random = new RandomCS();
        private static RandomSecure randomSecure = new RandomSecure();

        public static void SetSeed(ulong seed) => random.SetSeed(seed);
        public static void RemoveSeed(ulong seed) => random.RemoveSeed();
        public static int NextInt(int min, int max) => random.NextInt(min, max);
        public static double NextDouble(double min, double max) => random.NextDouble(min, max);
        public static bool NextBool() => random.NextBool();
        public static double NextDouble() => random.NextDouble(0.0, 1.0);

        public static class Secure
        {
            public static int Range(int min, int max) => randomSecure.Next(min, max);
            public static double Range(double min, double max) => randomSecure.Next(min, max);
            public static bool CoinFlip() => randomSecure.CoinFlip();
            public static bool Chance(float threshold, float max = 1.0f) => randomSecure.Chance(threshold, max);
            public static double NextDouble() => randomSecure.Next(0.0, 1.0);
            public static string Code() => randomSecure.GenerateCode();
            public static void GenerateBytes(byte[] bytes) => randomSecure.GenerateBytes(bytes);
        }
    }
}
