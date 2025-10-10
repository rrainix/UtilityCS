using System.Diagnostics.CodeAnalysis;

namespace UtilityCS
{
    public struct Vector2
    {
        public float X, Y;

        public static readonly Vector2 Zero = new Vector2(0, 0);
        public static readonly Vector2 One = new Vector2(1, 1);
        public static readonly Vector2 Up = new Vector2(0, 1);
        public static readonly Vector2 Down = new Vector2(0, -1);
        public static readonly Vector2 Right = new Vector2(1, 0);
        public static readonly Vector2 Left = new Vector2(-1, 0);

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 operator +(Vector2 a, Vector3 b) => new Vector2(a.X + b.X, a.Y + b.Y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.X - b.X, a.Y - b.Y);
        public static Vector2 operator *(Vector2 a, float scalar) => new Vector2(a.X * scalar, a.Y * scalar);
        public static Vector2 operator /(Vector2 a, float scalar) => new Vector2(a.X / scalar, a.Y / scalar);

        public static bool operator ==(Vector2 a, Vector2 b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(Vector2 a, Vector2 b) => !(a == b);

        public static implicit operator Vector3(Vector2 other) => new Vector3(other.X, other.Y);


        public static Vector2 FromString(string s)
        {
            string[] splits = s.Replace("(", "").Replace(")", "").Split(',');

            if (splits.Length < 2 || splits.Length > 3)
            {
                throw new ArgumentOutOfRangeException();
            }

            return new Vector2(float.Parse(splits[0]), float.Parse(splits[1]));
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public struct Vector3
    {
        public float X, Y, Z;

        public static readonly Vector3 Zero = new Vector3(0, 0);
        public static readonly Vector3 One = new Vector3(1, 1, 1);
        public static readonly Vector3 Up = new Vector3(0, 1);
        public static readonly Vector3 Down = new Vector3(0, -1);
        public static readonly Vector3 Right = new Vector3(1, 0);
        public static readonly Vector3 Left = new Vector3(-1, 0);

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Vector3(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vector3 operator -(Vector3 a, Vector3 b) => new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Vector3 operator *(Vector3 a, float scalar) => new Vector3(a.X * scalar, a.Y * scalar, a.Z * scalar);
        public static Vector3 operator /(Vector3 a, float scalar) => new Vector3(a.X / scalar, a.Y / scalar, a.Z / scalar);

        public static bool operator ==(Vector3 a, Vector3 b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        public static bool operator !=(Vector3 a, Vector3 b) => !(a == b);


        public static implicit operator Vector2(Vector3 other) => new Vector2(other.X, other.Y);

        public static Vector3 FromString(string s)
        {
            string[] splits = s.Replace("(", "").Replace(")", "").Split(',');

            if (splits.Length < 3 || splits.Length > 3)
            {
                throw new ArgumentOutOfRangeException();
            }

            return new Vector3(float.Parse(splits[0]), float.Parse(splits[1]), float.Parse(splits[2]));
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
