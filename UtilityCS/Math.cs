using System.Data;

namespace UtilityCS
{
    public static class Math
    {
        private static readonly DataTable dataTable = new DataTable();

        // Warning: this function may not be 100% DOS safe
        public static void Evaluate(string calcualtion) => dataTable.Compute(calcualtion, null);
    }
}
