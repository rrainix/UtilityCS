using System.Diagnostics.CodeAnalysis;

namespace BenScr.Collections
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

        public static Vector2 operator *(float scalar, Vector2 a) => new Vector2(a.X * scalar, a.Y * scalar);
        public static Vector2 operator *(Vector2 a, float scalar) => new Vector2(a.X * scalar, a.Y * scalar);
        public static Vector2 operator *(Vector2 a, Vector2 b) => new Vector2(a.X * b.X, a.Y * b.Y);

        public static Vector2 operator /(Vector2 a, float scalar) => new Vector2(a.X / scalar, a.Y / scalar);
        public static Vector2 operator /(Vector2 a, Vector2 b) => new Vector2(a.X / b.X, a.Y / b.Y);


        public static bool operator ==(Vector2 a, Vector2 b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(Vector2 a, Vector2 b) => !(a == b);

        public static implicit operator Vector3(Vector2 other) => new Vector3(other.X, other.Y);


        public float Length() => MathF.Sqrt(X * X + Y * Y);
        public float LengthSquared() => X * X + Y * Y;

        public Vector2 Normalized()
        {
            float len = Length();
            return len == 0 ? Zero : this / len;
        }

        public static float Dot(Vector2 a, Vector2 b) => a.X * b.X + a.Y * b.Y;

        public static float Distance(Vector2 a, Vector2 b) => (a - b).Length();

        public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            t = System.Math.Clamp(t, 0f, 1f);
            return a + (b - a) * t;
        }

        public static Vector2 Reflect(Vector2 vector, Vector2 normal)
            => vector - 2 * Dot(vector, normal) * normal;
        

        public static float Angle(Vector2 a, Vector2 b)
        {
            float dot = Dot(a, b);
            float len = a.Length() * b.Length();
            return MathF.Acos(dot / len);
        }

        public static Vector2 FromString(string s)
        {
            string[] splits = s.Replace("(", "").Replace(")", "").Split(',');
            if (splits.Length < 2 || splits.Length > 3)
                throw new ArgumentOutOfRangeException();

            return new Vector2(float.Parse(splits[0]), float.Parse(splits[1]));
        }

        public override string ToString() => $"({X}, {Y})";
        public override bool Equals(object? obj) => obj is Vector2 v && this == v;
        public override int GetHashCode() => HashCode.Combine(X, Y);
    }

    public struct Vector3
    {
        public float X, Y, Z;

        public static readonly Vector3 Zero = new Vector3(0, 0, 0);
        public static readonly Vector3 One = new Vector3(1, 1, 1);
        public static readonly Vector3 Up = new Vector3(0, 1, 0);
        public static readonly Vector3 Down = new Vector3(0, -1, 0);
        public static readonly Vector3 Right = new Vector3(1, 0, 0);
        public static readonly Vector3 Left = new Vector3(-1, 0, 0);
        public static readonly Vector3 Forward = new Vector3(0, 0, 1);
        public static readonly Vector3 Backward = new Vector3(0, 0, -1);

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
            Z = 0;
        }

        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vector3 operator -(Vector3 a, Vector3 b) => new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static Vector3 operator *(Vector3 a, float scalar) => new Vector3(a.X * scalar, a.Y * scalar, a.Z * scalar);
        public static Vector3 operator *(float scalar, Vector3 a) => new Vector3(a.X * scalar, a.Y * scalar, a.Z * scalar);


        public static Vector3 operator /(Vector3 a, float scalar) => new Vector3(a.X / scalar, a.Y / scalar, a.Z / scalar);

        public static bool operator ==(Vector3 a, Vector3 b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        public static bool operator !=(Vector3 a, Vector3 b) => !(a == b);

        public static implicit operator Vector2(Vector3 other) => new Vector2(other.X, other.Y);

        public float Length() => MathF.Sqrt(X * X + Y * Y + Z * Z);
        public float LengthSquared() => X * X + Y * Y + Z * Z;

        public Vector3 Normalized()
        {
            float len = Length();
            return len == 0 ? Zero : this / len;
        }

        public static float Dot(Vector3 a, Vector3 b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;

        public static Vector3 Cross(Vector3 a, Vector3 b) =>
            new Vector3(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X
            );

        public static float Distance(Vector3 a, Vector3 b) => (a - b).Length();

        public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
        {
            t = System.Math.Clamp(t, 0f, 1f);
            return a + (b - a) * t;
        }

        public static Vector3 Reflect(Vector3 vector, Vector3 normal)
        {
            return vector - 2 * Dot(vector, normal) * normal;
        }

        public static float Angle(Vector3 a, Vector3 b)
        {
            float dot = Dot(a, b);
            float len = a.Length() * b.Length();
            return MathF.Acos(dot / len);
        }

        public static Vector3 FromString(string s)
        {
            string[] splits = s.Replace("(", "").Replace(")", "").Split(',');
            if (splits.Length != 3)
                throw new ArgumentOutOfRangeException();

            return new Vector3(float.Parse(splits[0]), float.Parse(splits[1]), float.Parse(splits[2]));
        }

        public override string ToString() => $"({X}, {Y}, {Z})";
        public override bool Equals(object? obj) => obj is Vector3 v && this == v;
        public override int GetHashCode() => HashCode.Combine(X, Y, Z);
    }
}
