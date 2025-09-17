using UtilityCS;
public class Program
{
    public static void Main(string[] args)
    {
        UtilityCS.Random random = new UtilityCS.Random();
        for(int i = 0; i < 10; i++)
        Console.WriteLine(random.Next(11, 10));
      
    }
}