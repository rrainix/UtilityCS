

namespace UtilityCS
{
    public class TypeUtility
    {
        public static string GetTypeName<T>(){
            return typeof(T).Name;
        }
        public static string[] GetPropertyNames<T>()
        {
            return MiniLinq.Select(typeof(T).GetProperties(), p => p.Name).ToArray();
        }
        public static T GetValue<T>(string propertyName,T obj)
        {
           return (T)typeof(T).GetProperty(propertyName).GetValue(obj) ?? throw new NullReferenceException($"Property or Value of {propertyName} is null");
        }

        public static void SetValue<T>(string propertyName,T value)
        {
            typeof(T).GetProperty(propertyName);
        }
    }
}
