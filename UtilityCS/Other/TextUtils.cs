

using System.Text.Json;

namespace UtilityCS
{
    public enum FilterOption { Digits, Letters, Alphanumeric };

    public class Geodata
    {

    }

    public static class TextUtils
    {
        public const string DIGITS = "0123456789";
        public const string LETTERS_UPPERCASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string LETTERS_LOWERCASE = "abcdefghijklmnopqrstuvwxyz";
        public const string LETTERS = LETTERS_LOWERCASE + LETTERS_UPPERCASE;

        public const char SPACE = ' ';

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

        public static string ToHex(int value) => $"#{value:X2}";
        public static string ToHex(long value) => $"#{value:X2}";
        public static string ToHex(params int[] values)
        {
            string result = "#";
            foreach (var value in values)
            {
                result += $"{value:X2}";
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

        public static string ToColoredString(string text, string hexColor) // Note: Only useable when rich text editing is beeing supported
        {
            return $"<color=#{hexColor}>{text}</color>";
        }
    }
}