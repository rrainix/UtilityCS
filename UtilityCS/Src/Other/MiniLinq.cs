
namespace BenScr.Linq
{
    public static class MiniLinq
    {
        public static T Find<T>(T[] array, Func<T, bool> predicate, T defaultValue = default!)
        {
            foreach (var item in array)
            {
                if (predicate(item)) return item;
            }

            return defaultValue;
        }

        public static int IndexOf<T>(T[] array, T obj, int defaultValue = default!)
        {
            int i = 0;

            foreach (var item in array)
            {
                if (item.Equals(obj)) return i;
                i++;
            }

            return defaultValue;
        }
        public static int IndexOf<T>(T[] array, Func<T, bool> predicate, int defaultValue = default!)
        {
            int i = 0;

            foreach (var item in array)
            {
                if (predicate(item)) return i;
                i++;
            }

            return defaultValue;
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

        public static T[] Where<T>(T[] array, Func<T, bool> predicate)
        {
            List<T> list = new List<T>();

            foreach (var item in array)
            {
                if (predicate(item)) list.Add(item);
            }

            return list.ToArray();
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
        public static bool Contains<T>(T[] array, T obj)
        {
            foreach (var item in array)
            {
                if (item.Equals(obj)) return true;
            }

            return false;
        }
    }
}
