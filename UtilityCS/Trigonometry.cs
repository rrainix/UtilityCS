using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityCS
{
    public static class Trigonometry
    {
        public const double PI = 3.14159265358979323846;
        public const double Deg2Rad = PI / 180.0;
        public const double Rad2Deg = 180.0 / PI;

        public static double Sin(double degrees) => Math.Sin(degrees * Deg2Rad);
        public static double Cos(double degrees) => Math.Cos(degrees * Deg2Rad);
        public static double Tan(double degrees) => Math.Tan(degrees * Deg2Rad);
        public static double Atan2(double y, double x) => Math.Atan2(y, x) * Rad2Deg;
        public static double Sqrt(double value) => Math.Sqrt(value);
        public static double ToRadians(double degrees) => degrees * Deg2Rad;
        public static double ToDegrees(double radians) => radians * Rad2Deg;
    }
}
