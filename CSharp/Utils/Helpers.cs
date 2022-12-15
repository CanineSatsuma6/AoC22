namespace Utils;

public static class Helpers
{
    public static long LCM(IEnumerable<long> numbers)
    {
        return numbers.Aggregate(LCM);
    }

    public static long LCM(long first, long second)
    {
        return Math.Abs(first * second) / GCD(first, second);
    }

    public static long GCD(long first, long second)
    {
        return second == 0 ? first : GCD(second, first % second);
    }
}
