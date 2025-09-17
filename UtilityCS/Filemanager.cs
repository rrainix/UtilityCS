using System.Text.Json;


namespace UtilityCS
{
    public enum MemoryUnit
    {
        Bit = 1,
        Byte = 1,
        KiloByte = 1024,
        MegaByte = 1024 * 1024,
        GigaByte = 1024 * 1024 * 1024,
    }
    public enum Extension
    {
        txt, json, dat, bin, png
    }

    public static class Filemanager
    {
        public static string DesktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        public static string LocalAppFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public static string FromGameFolder(params string[] strings) => Path.Combine(LocalAppFolder, Path.Combine(strings));
        public static string FromGameFolder(Extension extension, params string[] strings) => SetExtensionPath(Path.Combine(LocalAppFolder, Path.Combine(strings)), extension);
        public static string SetExtensionPath(string path, Extension dataType) => Path.ChangeExtension(path, dataType.ToString());
        public static float GetFileSize(string path, MemoryUnit memoryUnit)
        {
            if (File.Exists(path))
            {
                long fileSizeInBytes = new FileInfo(path).Length;
                return fileSizeInBytes / (float)((int)memoryUnit);
            }

            return -1;
        }
        public static void DeleteAllFromDirectory(string directoryPath, bool onlyFiles)
        {
            if (Directory.GetDirectories(directoryPath).Length > 0)
                DeleteAllDirectories(directoryPath, onlyFiles);

            string[] files = Directory.GetFiles(directoryPath);

            foreach (string file in files)
            {
                File.Delete(file);
            }
        }
        public static void DeleteAllDirectories(string path, bool onlyFiles)
        {
            try
            {
                string[] directories = Directory.GetDirectories(path);

                foreach (string directory in directories)
                {
                    DeleteAllFromDirectory(directory, false);

                    if (!onlyFiles)
                        Directory.Delete(directory);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public static string[] GetDirectoryFiles(string directoryPath) => Directory.GetFiles(directoryPath);
        public static string[] GetDirectories(string path) => Directory.GetDirectories(path);
        public static string[] GetDirectoryFilesRecursive(string path)
        {
            List<string> files = new List<string>();

            foreach (var dir in GetDirectories(path))
            {
                files.AddRange(GetDirectoryFiles(dir));
            }

            return files.ToArray();
        }

        public static string RemoveLastPathSegment(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            string trimmed = path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string parent = Path.GetDirectoryName(trimmed) ?? string.Empty;
            if (string.IsNullOrEmpty(parent) && Path.IsPathRooted(trimmed))
                return Path.GetPathRoot(trimmed) ?? string.Empty;

            return parent;
        }
    }


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
                throw new FileNotFoundException("File doesn't Exist");

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
                           ?? throw new InvalidDataException("Deserialisierung ergab null");
                }
                catch
                {
                    return defaultValue;
                }
            }
        }
    }

}
