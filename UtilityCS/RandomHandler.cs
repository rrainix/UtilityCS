using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityCS
{
    public static class RandomHandler
    {
        private static RandomCS random = new RandomCS();

        public static void SetSeed(ulong seed) => random.SetSeed(seed);
        public static void RemoveSeed(ulong seed) => random.RemoveSeed();

        public static int Range(int min, int max) => random.Next(min, max);
        public static double Range(double min, double max) => random.Next(min, max);
        public static bool FlipCoin() => random.FlipCoin();
        public static bool Chance(float threshold, float max = 1.0f) => random.Chance(threshold, max);
        public static double NextDouble() => random.Next(0.0, 1.0);
    }
}
