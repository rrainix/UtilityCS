

using System.Reflection;

namespace UtilityCS
{
    public class TypeUtility
    {
        public static BindingFlags BindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public static string GetTypeName<T>()
        {
            return typeof(T).Name;
        }
        public static string[] GetFieldNames<T>()
        {
            return MiniLinq.Select(typeof(T).GetFields(BindingFlags), p => p.Name).ToArray();
        }
        public static T GetValueFromField<T, T2>(string fieldName, T2 obj)
        {
            return (T)typeof(T2).GetField(fieldName, BindingFlags).GetValue(obj) ?? throw new NullReferenceException($"Property or Value of {fieldName} is null");
        }

        public static void SetFieldValue<T, T2>(string fieldName, T obj,T2 value) where T : class
        {
            typeof(T).GetField(fieldName, BindingFlags).SetValue(obj, value);
        }
    }
}
