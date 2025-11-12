using System.Diagnostics;

namespace BenScr.Random
{
    public sealed class RandomCS
    {
        private ulong state;

        public RandomCS(ulong seed)
        {
            SetSeed(seed);
        }

        public RandomCS()
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


        public byte NextByte()
        {
            return (byte)(NextState() & 0xFF);
        }

        public bool NextBool() => NextInt(0, 2) == 0;


        public int NextInt()
        {
            return (int)(NextState() & 0x7FFFFFFF);
        }
        public int NextInt(int max)
        {
            if (max <= 0) throw new ArgumentOutOfRangeException(nameof(max));
            return NextInt() % max;
        }
        public int NextInt(int min, int max)
        {
            if (min >= max) throw new ArgumentOutOfRangeException($"Next({min},{max}) is wrong, min can't be more or equal to max.");
            return min + (NextInt() % (max - min));
        }

        public double NextDouble()
        {
            return (double)NextState() / ulong.MaxValue;
        }
        public double NextDouble(double max)
        {
            if (max <= 0) throw new ArgumentOutOfRangeException(nameof(max));
            return NextDouble() % max;
        }
        public double NextDouble(double min, double max)
        {
            return min + (NextDouble() * (max - min));
        }

        public float NextFloat()
        {
            return (float)NextDouble();
        }
        public float NextFloat(float max)
        {
            if (max <= 0) throw new ArgumentOutOfRangeException(nameof(max));
            return NextFloat() % max;
        }
        public float NextFloat(float min, float max)
        {
            return min + (NextFloat() * (max - min));
        }
    }
}
