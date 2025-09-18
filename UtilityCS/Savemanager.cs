using System.Text.Json;

namespace UtilityCS
{
    // Savemanager is a highlevel class simply used for saving and loading data generically.
    // Usage example:
    // int highscore = 0; SaveManager.Save<int>("Highscore", highscore);
    // int highscore = SaveManager.Load<int>("Highscore");

    public static class SaveManager
    {
        private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true,
            WriteIndented = false
        };
        public static readonly string MainPath = Filemanager.FromGameFolder("SaveManager Storage");

        public static string GetType<T>()
        {
            Type type = typeof(T);

            if (type.IsClass)
                return "Class";
            else if (type.IsValueType && !type.IsPrimitive)
                return "Nonprimtive Struct";
            else if (type.IsValueType)
                return "Primitive Struct";
            else
                return "Undefined Type";
        }

        private static string CreatePathFromType<T>(string key)
            => Filemanager.SetExtensionPath(Path.Combine(MainPath, MainPath, GetType<T>().ToString(), typeof(T).Name, key), Extension.json);

        public static int FilesCount => Filemanager.GetDirectoryFilesRecursive(MainPath).Length;

        public static void ClearAll(bool filesOnly = false) => Filemanager.DeleteAllDirectories(MainPath, filesOnly);

        public static void Delete<T>(string key)
        {
            string path = CreatePathFromType<T>(key);

            if (!File.Exists(path))
                throw new FileNotFoundException($"File with key ({key}) at path ({path}) doesn't Exist");

            File.Delete(path);
        }
        public static void Save<T>(string key, T obj, JsonSerializerOptions? options = null)
        {
            string path = CreatePathFromType<T>(key);
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            using FileStream fs = new FileStream(
                path, FileMode.Create, FileAccess.Write, FileShare.None,
                bufferSize: 1 << 20, FileOptions.SequentialScan);

            options ??= jsonOptions;

            using (fs)
            {
                System.Text.Json.JsonSerializer.Serialize(fs, obj, options);
            }
        }
        public static T Load<T>(string key, T defaultValue = default!, JsonSerializerOptions? options = null)
        {
            string path = CreatePathFromType<T>(key);

            if (!File.Exists(path)) return defaultValue;

            using FileStream fs = new FileStream(
                path, FileMode.Open, FileAccess.Read, FileShare.Read,
                bufferSize: 1 << 20, FileOptions.SequentialScan);

            options ??= jsonOptions;

            using (fs)
            {
                try
                {
                    return System.Text.Json.JsonSerializer.Deserialize<T>(fs, options)!
                               ?? throw new InvalidDataException("Deserialization resulted in null");
                }
                catch
                {
                    return defaultValue;
                }
            }
        }
    }
}
