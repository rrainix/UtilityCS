using System.Reflection;

namespace BenScr.Reflection
{
    public static class TypeUtility
    {
        public static BindingFlags BindingFlags =
       BindingFlags.Instance |
       BindingFlags.Static |
       BindingFlags.Public |
       BindingFlags.NonPublic |
       BindingFlags.FlattenHierarchy |
       BindingFlags.GetProperty |
       BindingFlags.SetProperty |
       BindingFlags.GetField |
       BindingFlags.SetField |
       BindingFlags.InvokeMethod |
       BindingFlags.CreateInstance;

        public static string GetTypeName<T>()
        {
            return typeof(T).Name;
        }
        public static string[] GetFieldNames<T>()
        {
            return Linq.MiniLinq.Select(typeof(T).GetFields(BindingFlags), p => p.Name).ToArray();
        }

        public static TResult GetFieldValue<TResult, TSource>(this TSource obj, string fieldName)
        {
            return (TResult)typeof(TSource).GetField(fieldName, BindingFlags).GetValue(obj) ?? throw new NullReferenceException($"Property or Value of {fieldName} is null");
        }

        public static void SetFieldValue<TSource, T2>(this TSource obj, string fieldName, T2 value) where TSource : class
        {
            typeof(TSource).GetField(fieldName, BindingFlags)?.SetValue(obj, value);
        }
    }
}
