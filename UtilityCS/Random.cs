using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityCS
{
    public class Random
    {
        private long _seed;

        // Konstanten für LCG (Beispielwerte)
        private const long a = 1664525;
        private const long c = 1013904223;
        private const long m = 1L << 32; // 2^32

        public Random(long seed)
        {
            _seed = seed;
        }

        // Gibt eine Zahl zwischen 0 und int.MaxValue zurück
        public int Next()
        {
            _seed = (a * _seed + c) % m;
            return (int)(_seed & 0x7FFFFFFF);
        }

        // Gibt eine Zahl zwischen 0 und max zurück
        public int Next(int max)
        {
            return Next() % max;
        }

        // Gibt eine Zahl zwischen min und max zurück
        public int Next(int min, int max)
        {
            return min + (Next() % (max - min));
        }

        // Gibt einen Double-Wert zwischen 0.0 und 1.0 zurück
        public double NextDouble()
        {
            return (double)Next() / int.MaxValue;
        }
    }
}
