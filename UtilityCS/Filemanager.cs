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
}
