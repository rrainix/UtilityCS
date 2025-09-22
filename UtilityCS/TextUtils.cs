

using System.Drawing;

namespace UtilityCS
{
    public enum FilterOption { Digits, Letters, Alphanumeric };

    public static class TextUtils
    {
        public const string DIGITS = "0123456789";
        public const string LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        public const string SPACE = " ";

        public static string RemoveChars(string input, params char[] nonAllowedChars)
        {
            return new string(input.Where(c => !nonAllowedChars.Contains(c)).ToArray());
        }
        public static string KeepOnly(string input, params char[] allowedChars)
        {
            return new string(input.Where(c => allowedChars.Contains(c)).ToArray());
        }
        public static char[] GetFilter(FilterOption filterOption, bool useSpace = true)
        {
            string allowedChars = filterOption == FilterOption.Digits ? DIGITS : (filterOption == FilterOption.Letters ? LETTERS : (DIGITS + LETTERS));
            allowedChars += useSpace ? SPACE : "";
            return allowedChars.ToArray();
        }

        public static string ToHex(int s) => $"#{s:X2}";
        public static string ToHex(long s) => $"#{s:X2}";
        public static string ToHex(params int[] values)
        {
            string result = string.Empty;
            foreach (var i in values)
            {
                result += ToHex(i);
            }
            return result;
        }
        public static string ToHex(params long[] values)
        {
            string result = string.Empty;
            foreach (var l in values)
            {
                result += ToHex(l);
            }
            return result;
        }


        public static int WordsCount(string s) => s.Split(' ').Length;

        public static string ColoredString(string text, string hexColor) // Note: Only useable when rich text editing is beeing supported
        {
            return $"<color=#{hexColor}>{text}</color>";
        }
    }
}
