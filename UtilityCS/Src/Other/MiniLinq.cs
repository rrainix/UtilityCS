
using System.Numerics;

namespace BenScr.Linq
{
    public static class MiniLinq
    {
        public static T? Find<T>(IEnumerable<T> items, Func<T, bool> predicate)
        {
            foreach (var item in items)
            {
                if (predicate(item)) return item;
            }

            return default;
        }

        public static int IndexOf<T>(IEnumerable<T> items, T obj, int defaultValue = default!)
        {
            int i = 0;

            foreach (var item in items)
            {
                if (item.Equals(obj)) return i;
                i++;
            }

            return defaultValue;
        }
        public static int IndexOf<T>(IEnumerable<T> items, Func<T, bool> predicate, int defaultValue = default!)
        {
            int i = 0;

            foreach (var item in items)
            {
                if (predicate(item)) return i;
                i++;
            }

            return defaultValue;
        }

        public static IEnumerable<TResult> Select<T, TResult>(IEnumerable<T> items, Func<T, TResult> selector)
        {
            int itemsLength = items.Count();
            var result = new TResult[itemsLength];
            for (int i = 0; i < itemsLength; i++)
            {
                result[i] = selector(items.ElementAt(i));
            }
            return result;
        }

        public static IEnumerable<T> Where<T>(IEnumerable<T> items, Func<T, bool> predicate)
        {
            List<T> list = new List<T>();

            foreach (var item in items)
            {
                if (predicate(item)) list.Add(item);
            }

            return list.ToArray();
        }
        public static int Count<T>(IEnumerable<T> items, Func<T, bool> predicate)
        {
            int count = 0;

            foreach (var item in items)
            {
                if (predicate(item)) count++;
            }

            return count;
        }
        public static bool Contains<T>(IEnumerable<T> items, Func<T, bool> predicate)
        {
            foreach (var item in items)
            {
                if (predicate(item)) return true;
            }

            return false;
        }
        public static bool Contains<T>(IEnumerable<T> items, T obj)
        {
            foreach (var item in items)
            {
                if (item.Equals(obj)) return true;
            }

            return false;
        }


        public static T Average<T>(IEnumerable<T> items) where T : INumber<T>
        {
            T sum = default;
            int itemsLength = items.Count();

            for (int i = 0; i < itemsLength; i++)
            {
                sum += items.ElementAt(i);
            }

            return sum / T.CreateChecked(itemsLength);
        }
    }
}
