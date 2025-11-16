using System.Security.Cryptography;

namespace BenScr.Security.Cryptography
{
    public sealed class RandomSecure
    {
        private const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvw0123456789";
        private static readonly RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();

        public bool NextBool() => NextInt(0, 2) == 0;
        public byte NextByte()
        {
            return (byte)(NextInt() & 0xFF);
        }

        public int NextInt()
        {
            byte[] fourBytes = new byte[4];
            randomNumberGenerator.GetBytes(fourBytes);
            int value = BitConverter.ToInt32(fourBytes, 0) & int.MaxValue;
            return value;
        }
        public int NextInt(int min, int max)
        {
            return min + (NextInt() % (max - min));
        }
        public int NextInt(int max)
        {
            if (max <= 0) throw new ArgumentOutOfRangeException(nameof(max));
            return NextInt() % max;
        }

        public double NextDouble()
        {
            byte[] eightBytes = new byte[8];
            randomNumberGenerator.GetBytes(eightBytes);
            double value = BitConverter.ToUInt64(eightBytes, 0) / (ulong.MaxValue + 1.0);
            return value;
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
            byte[] fourBytes = new byte[4];
            randomNumberGenerator.GetBytes(fourBytes);
            float value = BitConverter.ToUInt32(fourBytes, 0) / (uint.MaxValue + 1.0f);
            return value;
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

        public string NextString(int length = 10, string charset = null)
        {
            charset ??= CHARS;
            int charsetLength = charset.Length;

            string code = string.Empty;

            for (int i = 0; i < length; i++)
                code += charset[NextInt(charsetLength)];

            return code;
        }

        public void GenerateBytes(byte[] bytes) => randomNumberGenerator.GetBytes(bytes);
    }
}
