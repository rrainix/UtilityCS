using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityCS
{
    public static class MathHelper
    {
        private static readonly DataTable dataTable = new DataTable();

        // Warning: this function may not be 100% DOS safe
        public static void Evaluate(string calcualtion) => dataTable.Compute(calcualtion, null);
    }
}
