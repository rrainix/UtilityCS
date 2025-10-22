using UtilityCS;
using System.Reflection;
using System.Diagnostics;

public class Program
{
    public const string SEPERATOR = "-----------------------------------";

    public static Dictionary<string, Delegate> Previews = new()
  {
      { "Savemanager Preview", new Action(SaveManagerPreview) },
      { "Random Preview", new Action(RandomPreview) },
      { "Secure Random Preview", new Action(RandomSecurePreview) },
      { "Enctyptor Preview", new Action(EncryptorPreview) },
      { "Typeutility Preview", new Action(TypeUtilityPreview) }
  };

    public static void Main(string[] args)
    {
        do
        {
            Console.Clear();
            Console.WriteLine(SEPERATOR);
            int i = 1;

            foreach (var key in Previews.Keys)
            {
                var option = Previews[key];

                if (option is Action action)
                {
                    Console.WriteLine($"Preview {i++}/{Previews.Count}");
                    action();
                    EnterForNextPreview();
                }
            }

            Console.WriteLine("Would you like to continue? (Y/N)");
        } while (Console.ReadLine().ToLower() == "y");

    }

    public static void EnterForNextPreview()
    {
        Console.WriteLine(SEPERATOR);
        Console.WriteLine("Press Enter to see the next preview");
        Console.ReadLine();
        Console.Clear();
        Console.WriteLine(SEPERATOR);
    }

    public static void TypeUtilityPreview()
    {
        Console.WriteLine("-- Type Utility Preview --");
        Vector2 vec2 = new Vector2(10.14f, 1);
        string propertyName = "X";

        float vec2X = TypeUtility.GetValueFromField<float, Vector2>(propertyName, vec2);
        Console.WriteLine(vec2.X);
        Console.WriteLine(vec2X);
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

    public static void RandomSecurePreview()
    {
        Console.WriteLine("-- Random Secure Preview --");
        RandomSecure randomSecure = new RandomSecure();
        Console.WriteLine("Random between (0.0-10.0):" + randomSecure.Next(0, 10f));
        Console.WriteLine("Random Code: " + randomSecure.GenerateCode(20));
    }

    public static void EncryptorPreview()
    {
        Console.WriteLine("-- Encryptor Preview --");
        string message = "Hello World!";
        string encrypedMessage = Encryptor.String.Encrypt(message);
        Console.WriteLine($"Original Message: \"{message}\"");
        Console.WriteLine($"Encrypted Message: \"{encrypedMessage}\"");
        Console.WriteLine($"Decrypted Message: \"{Encryptor.String.Decrypt(encrypedMessage)}\"");
    }
}