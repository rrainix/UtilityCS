using BenScr.Serialization.Json;
using System.Text.Json;

namespace BenScr.IO
{
    // Savemanager is a highlevel class simply used for saving and loading data in json.
    // Usage example:
    // int highscore = 0; SaveManager.Save<int>("Highscore", highscore);
    // int highscore = SaveManager.Load<int>("Highscore");

    public static class SaveManager
    {
        public static string MainPath = FileManager.FromLocalAppFolder("BenScr", "SaveManager");

        private static string CreatePathFromType<T>(string key, Extension extension = Extension.json)
            => FileManager.CombinePathWithExtension(extension, MainPath, MainPath, typeof(T).Name, key);

        public static string GetPathOfKey<T>(string key)
            => CreatePathFromType<T>(key);
        public static string GetFolderPathOfKey<T>()
            => FileManager.RemoveLastPathSegment(CreatePathFromType<T>("Key"));

        public static int GetSavedFilesCount() => FileManager.GetDirectoryFiles(MainPath, true).Length;
        public static string[] GetSavedFilesPath() => FileManager.GetDirectoryFiles(MainPath, true);

        public static void ClearAll(bool filesOnly = false) => FileManager.DeleteAllDirectories(MainPath, filesOnly);

        public static void Save<T>(string key, T obj, JsonSerializerOptions? options = null)
        {
            string path = CreatePathFromType<T>(key); 
            Json.Save(path, obj, options);
        }
        public static T Load<T>(string key, T defaultValue = default!, JsonSerializerOptions? options = null)
        {
            string path = CreatePathFromType<T>(key);
            return Json.Load<T>(path, options: options);
        }
        public static void Delete<T>(string key)
        {
            string path = CreatePathFromType<T>(key);

            if (!File.Exists(path))
                throw new FileNotFoundException($"File with key ({key}) at path ({path}) doesn't Exist");

            File.Delete(path);
        }
    }
}
