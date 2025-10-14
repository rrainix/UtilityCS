using UtilityCS;
public class Program
{
    public const string SEPERATOR = "------------------------";

    public static void Main(string[] args)
    {
        SaveManagerPreview();
        Console.WriteLine(SEPERATOR);
        LoggerPreview();
        Console.WriteLine(SEPERATOR);
        RandomPreview();
        Console.WriteLine(SEPERATOR);
    }

    public static void SaveManagerPreview()
    {
        // Saving
        {
            Console.WriteLine("-- Savemanager Preview --");
            Vector2 vector2 = new Vector2(15.0f, 25.0f);
            SaveManager.Save("Position", vector2);
            Console.WriteLine($"Succesfully Saved Vector2{vector2}, at path '{SaveManager.GetPathOfKey<Vector2>("Position")}'.");
        }

        // Loading
        {
            Vector2 loadedVector2 = SaveManager.Load<Vector2>("Position");
            Console.WriteLine($"Succesfully Loaded Vector2{loadedVector2}.");
        }
    }

    public static void LoggerPreview()
    {
        Logger.WriteColored("-- Logger Preview --", ConsoleColor.Gray);
        Logger.Message("Hello World");
        Logger.Warning("Warning Preview");
        Logger.Error("Error Preview");
    }

    public static void RandomPreview()
    {
        Console.WriteLine("-- Random Preview --");
        RandomCS random = new RandomCS();
        Console.WriteLine($"Random between (0-1000): {random.Next(1000)}");
        Console.WriteLine($"Random between (0.0-10.0): {random.Next(0.0, 10.0)}");
    }
}