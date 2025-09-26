using System.Text;
using UtilityCS;
public class Program
{
    public static void Main(string[] args)
    {
        Serializer.Binary.SaveObject("Test", "Hallo");
        Logger.Message("Welcome", "Hello, World!");
    }
}