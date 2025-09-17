using UtilityCS;
public class Program
{
    public static void Main(string[] args)
    {
        int[] values = { 1, 2, 3, 4, 4, 4, 12, 999, 123, 55 };
        Console.WriteLine(MiniLinq.Count(values, value => value == 4));
        MiniLinq.Select(values, value => value += 1);
        Console.WriteLine(MiniLinq.Count(values, value => value == 1));
    }
}