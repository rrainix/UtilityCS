using System.Security.Cryptography;

namespace BenScr.Security.Cryptography
{
    public sealed class RandomSecure
    {
        private const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvw0123456789";
        private static readonly RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();

        public int NextInt()
        {
            byte[] fourBytes = new byte[4];
            randomNumberGenerator.GetBytes(fourBytes);
            int value = BitConverter.ToInt32(fourBytes, 0) & int.MaxValue;
            return value;
        }
        public double NextDouble()
        {
            byte[] fourBytes = new byte[8];
            randomNumberGenerator.GetBytes(fourBytes);
            double value = BitConverter.ToUInt64(fourBytes, 0) / (ulong.MaxValue + 1.0);
            return value;
        }

        public int Next(int min, int max)
        {
            return min + (NextInt() % (max - min));
        }
        public int Next(int max)
        {
            if (max <= 0) throw new ArgumentOutOfRangeException(nameof(max));
            return NextInt() % max;
        }

        public double Next(double min, double max)
        {
            return min + (NextDouble() * (max - min));
        }

        public bool CoinFlip() => Next(0, 2) == 0;

        public bool Chance(float threshold, float max = 1.0f)
        {
            return Next(0f, max) < threshold;
        }

        public string GenerateCode(int length = 10, string charset = null)
        {
            charset ??= CHARS;
            int charsetLength = charset.Length;

            string code = string.Empty;

            for (int i = 0; i < length; i++)
                code += charset[Next(charsetLength)];

            return code;
        }

        public void GenerateBytes(byte[] bytes) => randomNumberGenerator.GetBytes(bytes);
    }
}
