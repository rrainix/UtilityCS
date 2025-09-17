using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityCS
{
    public static class MiniLinq
    {
        #region Array
        public static T Find<T>(T[] array, T obj)
        {
            foreach (var item in array)
            {
                if (item.Equals(obj)) return item;
            }

            return default;
        }

        public static TResult[] Select<T, TResult>(this T[] array, Func<T, TResult> selector)
        {
            var result = new TResult[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = selector(array[i]);
            }
            return result;
        }

        public static T Where<T>(T[] array, Func<T, bool> predicate)
        {
            foreach (var item in array)
            {
                if (predicate(item)) return item;
            }

            return default;
        }
        public static int Count<T>(T[] array, Func<T, bool> predicate)
        {
            int count = 0;

            foreach (var item in array)
            {
                if (predicate(item)) count++;
            }

            return count;
        }
        public static bool Contains<T>(T[] array, Func<T, bool> predicate)
        {
            foreach (var item in array)
            {
                if (predicate(item)) return true;
            }

            return false;
        }

        #endregion

        #region List
        public static T Find<T>(List<T> list, T obj)
        {
            foreach (var item in list)
            {
                if (item.Equals(obj)) return item;
            }

            return default;
        }

        public static TResult[] Select<T, TResult>(this List<T> list, Func<T, TResult> selector)
        {
            var result = new TResult[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                result[i] = selector(list[i]);
            }
            return result;
        }
        public static T Where<T>(List<T> list, Func<T, bool> predicate)
        {
            foreach (var item in list)
            {
                if (predicate(item)) return item;
            }

            return default;
        }
        public static int Count<T>(List<T> list, Func<T, bool> predicate)
        {
            int count = 0;

            foreach (var item in list)
            {
                if (predicate(item)) count++;
            }

            return count;
        }
        public static bool Contains<T>(List<T> list, Func<T, bool> predicate)
        {
            foreach (var item in list)
            {
                if (predicate(item)) return true;
            }

            return false;
        }
        #endregion
    }
}
