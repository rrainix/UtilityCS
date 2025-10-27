using System.Data;

namespace BenScr.Math
{
    public static class MathCS
    {
        private static readonly DataTable dataTable = new DataTable();

        // Warning: this function may not be 100% DOS safe
        public static void Evaluate(string calcualtion) => dataTable.Compute(calcualtion, null);

        public static int Max(params int[] values)
        {
            int max = int.MinValue;

            foreach (var item in values)
            {
                if (item > max)
                {
                    max = item;
                }
            }

            return max;
        }
        public static float Max(params float[] values)
        {
            float max = float.MinValue;

            foreach (var item in values)
            {
                if (item > max)
                {
                    max = item;
                }
            }

            return max;
        }
        public static int Min(params int[] values)
        {
            int min = int.MaxValue;

            foreach (var item in values)
            {
                if (item < min)
                {
                    min = item;
                }
            }

            return min;
        }
        public static float Min(params float[] values)
        {
            float min = float.MaxValue;

            foreach (var item in values)
            {
                if (item < min)
                {
                    min = item;
                }
            }

            return min;
        }
    }
}
