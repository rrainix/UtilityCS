using System.Security.Cryptography;

namespace UtilityCS
{
    [Flags]
    public enum IncludeFlags
    {
        None = 0,
        Digits = 1 << 0,
        Uppercase = 1 << 1,
        Lowercase = 1 << 2,
        Symbols = 1 << 3,
        All = Digits | Uppercase | Lowercase | Symbols
    }

    public class Password
    {
        private const string DIGITS = "0123456789";
        private const string UPPER_CASE_LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string LOWER_CASE_LETTERS = "abcdefghijklmnopqrstuvwxyz";
        private const string SPECIAL_CHARS = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";

        private static readonly RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
        private string includeCharset = string.Empty;
        private string excludeCharset = string.Empty;

        public byte Length = 16;
        public IncludeFlags Flags = IncludeFlags.None;

        public static Password Default() 
            => new Password().IncludeDigits().IncludeLowercase().IncludeUppercase().IncludeSymbols().LengthRequired(16);

        public string Charset()
        {
            string charset = includeCharset;

            if (Flags.HasFlag(IncludeFlags.Digits))
                charset += DIGITS;

            if (Flags.HasFlag(IncludeFlags.Uppercase))
                charset += UPPER_CASE_LETTERS;

            if (Flags.HasFlag(IncludeFlags.Lowercase))
                charset += LOWER_CASE_LETTERS;

            if (Flags.HasFlag(IncludeFlags.Symbols))
                charset += SPECIAL_CHARS;

            if (charset.Length == 0)
                throw new InvalidOperationException("Charset empty");


            if (excludeCharset.Length > 0)
                charset.Replace(excludeCharset, "");

            return charset;
        }
        public Password SetIncludeFlags(IncludeFlags flags)
        {
            Flags = flags;
            return this;
        }

        public Password IncludeUppercase()
        {
            Flags |= IncludeFlags.Uppercase;
            return this;
        }
        public Password IncludeLowercase()
        {
            Flags |= IncludeFlags.Lowercase;
            return this;
        }
        public Password IncludeSymbols()
        {
            Flags |= IncludeFlags.Symbols;
            return this;
        }
        public Password IncludeDigits()
        {
            Flags |= IncludeFlags.Digits;
            return this;
        }

        public Password ExcludeUppercase()
        {
            Flags &= ~IncludeFlags.Uppercase;
            return this;
        }
        public Password ExcludeLowercase()
        {
            Flags &= ~IncludeFlags.Lowercase;
            return this;
        }
        public Password ExcludeSymbols()
        {
            Flags &= ~IncludeFlags.Symbols;
            return this;
        }
        public Password ExcludeDigits()
        {
            Flags &= ~IncludeFlags.Digits;
            return this;
        }

        public Password LengthRequired(byte length)
        {
            Length = length;
            return this;
        }
        public Password IncludeCharset(string charset)
        {
            includeCharset = charset;
            return this;
        }
        public Password ExcludeCharset(string charset)
        {
            excludeCharset = charset;
            return this;
        }


        public string Next()
        {
            if (Length == 0) return "0";

            string pwd = "";

            string charset = Charset();
            byte charsetLength = (byte)charset.Length;

            for (byte i = 0; i < Length; i++)
            {
                pwd += charset[NextByte(charsetLength)];
            }

            return pwd;
        }

        private byte NextByte(byte max)
        {
            byte[] bytes = new byte[1];
            randomNumberGenerator.GetBytes(bytes);
            return (byte)((bytes[0] & byte.MaxValue) % max);
        }

        public override string ToString()
        {
            return $"Length:{Length} Lowercase:{Flags.HasFlag(IncludeFlags.Lowercase)} Uppercase:{Flags.HasFlag(IncludeFlags.Uppercase)} Digits:{Flags.HasFlag(IncludeFlags.Digits)} Symbols: {Flags.HasFlag(IncludeFlags.Symbols)} IncludeCharset:\"{includeCharset}\" ExcludeCharset:\"{excludeCharset}\"";
        }
    }
}
