using System.Diagnostics;

namespace UtilityCS
{
    public class Random
    {
        private ulong state;

        public Random(ulong seed)
        {
            SetSeed(seed);
        }

        public Random()
        {
            RemoveSeed();
        }

        private ulong NextState()
        {
            state ^= state >> 12;
            state ^= state << 25;
            state ^= state >> 27;
            return state * 2685821657736338717UL;
        }

        public void SetSeed(ulong seed)
        {
            state = seed;
        }
        public void RemoveSeed()
        {
            ulong t = (ulong)DateTime.Now.Ticks;
            ulong g = (ulong)Guid.NewGuid().GetHashCode();
            ulong s = (ulong)Stopwatch.GetTimestamp();
            state = t ^ g ^ s;
        }

        public ulong GetSeed() => state;

        private int NextInt()
        {
            return (int)(NextState() & 0x7FFFFFFF);
        }
        private double NextDouble()
        {
            return (double)NextState() / ulong.MaxValue;
        }

        public int Next(int max)
        {
            if (max <= 0) throw new ArgumentOutOfRangeException(nameof(max));
            return NextInt() % max;
        }

        public int Next(int min, int max)
        {
            if (min >= max) throw new ArgumentOutOfRangeException($"Next({min},{max}) is wrong, min can't be more or equal to max.");
            return min + (NextInt() % (max - min));
        }

        public double Next(double min, double max)
        {
            return min + (NextDouble() * (max - min));
        }

        public bool FlipCoin() => Next(0, 2) == 0;

        public bool Chance(float threshold, float max = 1.0f)
        {
            return Next(0f, max) < threshold;
        }
    }
}
