using BenScr.Security.Cryptography;

namespace BenScr.Random
{
    public static class RandomHandler
    {
        private static RandomCS random = new RandomCS();
        private static RandomSecure randomSecure = new RandomSecure();

        public static void SetSeed(ulong seed) => random.SetSeed(seed);
        public static void RemoveSeed() => random.RemoveSeed();

        public static bool NextBool() => random.NextBool();
        public static int NextInt(int min, int max) => random.NextInt(min, max);
        public static double NextDouble(double min, double max) => random.NextDouble(min, max);

        public static class Secure
        {
            public static bool NextBool() => randomSecure.NextBool();
            public static int NextInt(int min, int max) => randomSecure.NextInt(min, max);
            public static double NextDouble(double min, double max) => randomSecure.NextDouble(min, max);
            public static string NextString() => randomSecure.NextString();
            public static void GenerateBytes(byte[] bytes) => randomSecure.GenerateBytes(bytes);
        }
    }
}
