using System;
using System.Diagnostics;

namespace UtilityCS
{
    public class Random
    {
        private ulong state;

        public Random(ulong seed)
        {
            state = seed;
        }

        public Random()
        {
            ulong t = (ulong)DateTime.Now.Ticks;
            ulong g = (ulong)Guid.NewGuid().GetHashCode();
            ulong s = (ulong)Stopwatch.GetTimestamp();
            state = t ^ g ^ s;
        }

        private ulong NextState()
        {
            state ^= state >> 12;
            state ^= state << 25;
            state ^= state >> 27;
            return state * 2685821657736338717UL;
        }

        public int Next()
        {
            return (int)(NextState() & 0x7FFFFFFF);
        }

        public int Next(int max)
        {
            if (max <= 0) throw new ArgumentOutOfRangeException(nameof(max));
            return Next() % max;
        }

        public int Next(int min, int max)
        {
            if (min >= max) throw new ArgumentOutOfRangeException();
            return min + (Next() % (max - min));
        }

        public double NextDouble() // Returns a double between 0.0 - 1.0
        {
            return (double)NextState() / ulong.MaxValue;
        }
    }
}
