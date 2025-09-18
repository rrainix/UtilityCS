

namespace UtilityCS
{
    public enum FilterOption { Digits, Letters, Alphanumeric };

    public class TextUtils
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
    }
}
