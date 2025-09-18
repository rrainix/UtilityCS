
namespace UtilityCS
{
    public static class Trigonometry
    {
        public const double PI = 3.14159265358979323846;
        public const double Deg2Rad = PI / 180.0;
        public const double Rad2Deg = 180.0 / PI;

        public static double Sin(double degrees) => System.Math.Sin(degrees * Deg2Rad);
        public static double Cos(double degrees) => System.Math.Cos(degrees * Deg2Rad);
        public static double Tan(double degrees) => System.Math.Tan(degrees * Deg2Rad);
        public static double Atan2(double y, double x) => System.Math.Atan2(y, x) * Rad2Deg;
        public static double Sqrt(double value) => System.Math.Sqrt(value);
        public static double ToRadians(double degrees) => degrees * Deg2Rad;
        public static double ToDegrees(double radians) => radians * Rad2Deg;
    }
}
