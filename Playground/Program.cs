using BenScr.Collections;
using BenScr.Debugging;
using BenScr.Diagnostics;
using BenScr.IO;
using BenScr.Linq;
using BenScr.Math;
using BenScr.Random;
using BenScr.Reflection;
using BenScr.Security.Cryptography;
using BenScr.Text;
using System;
using System.Diagnostics;
using System.Reflection;

public class Program
{
    private const string SEPERATOR = "-----------------------------------";

    public class User
    {
        public string Name;
        public string Password;
        public int Age;
    }

    private static Dictionary<string, Delegate> previews = new()
  {
      { "Savemanager Preview", new Action(SaveManagerPreview) },
      { "Random Preview", new Action(RandomPreview) },
      { "Secure Random Preview", new Action(RandomSecurePreview) },
      { "Enctyptor Preview", new Action(EncryptorPreview) },
      { "Type Utility Preview", new Action(TypeUtilityPreview) },
      {"Mini Linq Preview", new Action(MiniLinqPreview) }
  };

    public static void Main(string[] args)
    {
        Run();
    }

    private static void Run()
    {
        RandomCS randomCS = new RandomCS();

        do
        {
            Console.Clear();
            Console.WriteLine(SEPERATOR);

            for (int i = 0; i < previews.Keys.Count; i++)
            {
                var option = previews.Values.ElementAt(i);

                if (option is Action action)
                {
                    Console.WriteLine($"({i + 1}/{previews.Count})");
                    action();
                    EnterForNextPreview(ref i);
                }
            }

            Console.WriteLine("Would you like to continue? (Y/N)");
        } while (Console.ReadLine().ToLower() == "y");
    }

    public static void EnterForNextPreview(ref int i)
    {
        Console.WriteLine(SEPERATOR);
        Console.WriteLine("Press `Enter` to see the next preview `Left Arrow` to go back and `R` to see again");
        ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();

        if (consoleKeyInfo.Key == ConsoleKey.LeftArrow) i = i - 2;
        if (consoleKeyInfo.Key == ConsoleKey.R) i--;

        if (i < -1) i = 4;

        Console.Clear();
        Console.WriteLine(SEPERATOR);
    }

    public static void TypeUtilityPreview()
    {
        Console.WriteLine("-- Type Utility Preview --");
        User user = new User { Age = 25, Name = "Test", Password = "password123" };
        user.SetFieldValue<User, int>("Age", 20);
        int age = user.GetFieldValue<int, User>("Age");
        Console.WriteLine(user.Age + " " + age);
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
        Console.WriteLine($"Random between (0-1000): {random.NextInt(1000)}");
        Console.WriteLine($"Random between (0.0-10.0): {random.NextDouble(0.0, 10.0)}");
        Console.WriteLine($"Random byte {random.NextByte()}");
    }

    public static void RandomSecurePreview()
    {
        Console.WriteLine("-- Random Secure Preview --");
        BenScr.Security.Cryptography.RandomSecure randomSecure = new BenScr.Security.Cryptography.RandomSecure();
        Console.WriteLine("Random between (0.0-10.0):" + randomSecure.Next(0, 10f));
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

    public static void MiniLinqPreview()
    {
        Console.WriteLine("-- Mini Linq Preview --");
        int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        Vector2[,] array2 = { { new Vector2(0, 5), new Vector2(2, 1), new Vector2(1, 2), new Vector2(0, 4) }, { new Vector2(0, 5), new Vector2(2, 1), new Vector2(1, 2), new Vector2(0, 4) } };
        List<int> lis = new List<int>();
        MiniLinq.Find(lis, v => v.Equals(2));
        int index = MiniLinq.IndexOf(array, i => i == 2);
        MiniLinq.Select(array, i => i + 1);
        MiniLinq.Select(array, i => i * 2.0f);
        int count = MiniLinq.Count(array, i => i == 2);
        Console.WriteLine(count);
        Console.WriteLine(array[0]);
        Console.WriteLine(index);
    }
}