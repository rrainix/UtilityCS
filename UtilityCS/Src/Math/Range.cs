
namespace BenScr.Collections
{
    public struct Range
    {
        public float Min { get; set; }
        public float Max { get; set; }

        public Range(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public static bool operator ==(Range a, Range b)
        {
            return a.Min == b.Min && a.Max == b.Max;
        }
        public static bool operator !=(Range a, Range b) => !(a == b);

        public static Range operator +(Range a, Range b)
        {
            return new Range(a.Min + b.Min, a.Max + b.Max);
        }
        public static Range operator -(Range a, Range b)
        {
            return new Range(a.Min - b.Min, a.Max - b.Max);
        }

        public bool Contains(float value)
        {
            return value >= Min && value <= Max;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Min, Max);
        }

        public override bool Equals(object obj)
        {
            if (obj is Range other)
            {
                return this == other;
            }
            return false;
        }
    }
}
