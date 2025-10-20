using UtilityCS;
using System.Reflection;

public class Geodata
{
    public float X;
    public float Y;
    public Geodata(float x, float y)
    {
        X = x; Y = y;
    }
}

public class Program
{
    public const string SEPERATOR = "------------------------";

    public static void Main(string[] args)
    {
        RandomSecure randomSecure = new RandomSecure();

        for (int i = 0; i < 10; i++)
            Console.WriteLine(randomSecure.Code());


        SaveManagerPreview();
        Console.WriteLine(SEPERATOR);
        LoggerPreview();
        Console.WriteLine(SEPERATOR);
        RandomPreview();
        Console.WriteLine(SEPERATOR);
        Console.WriteLine(TypeUtility.GetValue<float, Vector2>("X", new Vector2(10.14f, 1)));
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