using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityCS
{
    public static class RandomHandler
    {
        private static Random random = new Random();

        public static int Range(int min, int max) => random.Next(min, max);
        public static double Range(double min, double max) => random.Next(min, max);
        public static bool FlipCoin() => random.FlipCoin();
        public static bool Chance(float threshold, float max = 1.0f) => random.Chance(threshold, max);
    }
}
