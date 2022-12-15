using System.Numerics;
using System.Text.RegularExpressions;

using Utils;

var code = new Code();

code.Check();
code.Run();

public class Code : Day
{
    // Monkey item lists get changed during part 1, which messes up part 2
    // Keep two independent sets of monkeys for each part
    public IList<Monkey> Part1Monkeys { get; set; }
    public IList<Monkey> Part2Monkeys { get; set; }
    public long LCM { get; set; }

    public override void ParseInput(string filePath)
    {
        Part1Monkeys = new List<Monkey>();
        Part2Monkeys = new List<Monkey>();

        var lines = File.ReadAllLines(filePath);

        for (int i = 0; i < lines.Length; i += 7)
        {
            Part1Monkeys.Add(new Monkey(lines.Skip(i).Take(7).ToList()));
            Part2Monkeys.Add(new Monkey(lines.Skip(i).Take(7).ToList()));
        }

        LCM = Helpers.LCM(Part2Monkeys.Select(m => m.Divisor));
    }

    public override void PartOne()
    {
        for (int i = 0; i < 20; i++)
        {
            DoMonkeyBusiness(Part1Monkeys, true);
        }

        Console.WriteLine(Part1Monkeys.Select(m => m.NumInspectedItems).OrderByDescending(n => n).Take(2).Aggregate(1, (product, n) => product * n));
    }

    public override void PartTwo()
    {
        for (int i = 0; i < 10000; i++)
        {
            DoMonkeyBusiness(Part2Monkeys, false);
        }

        Console.WriteLine(Part2Monkeys.Select(m => m.NumInspectedItems).OrderByDescending(n => n).Take(2).Aggregate((BigInteger)1, (product, n) => product * n));
    }

    public void DoMonkeyBusiness(IList<Monkey> monkeys, bool canBeCalmed)
    {
        for (int i = 0; i < monkeys.Count; i++)
        {
            while (monkeys[i].HasItems())
            {
                monkeys[i].NumInspectedItems++;

                var item = monkeys[i].Operation(monkeys[i].TakeItem());

                if (canBeCalmed)
                {
                    item /= 3;
                }
                else
                {
                    // To ensure we stay in a numeric range that can be represented by a long, but without messing up our divisibility by
                    // any of the monkeys' test divisors, we can take the modulo of the current item and the least common multiple of all
                    // the monkeys' test divisors
                    item %= LCM;
                }

                if (item % monkeys[i].Divisor == 0)
                {
                    monkeys[monkeys[i].TrueMonkey].GiveItem(item);
                }
                else
                {
                    monkeys[monkeys[i].FalseMonkey].GiveItem(item);
                }
            }
        }
    }
}

public class Monkey
{
    private static Regex ITEMS_REGEX        = new Regex(@"Starting items: (.*)$",              RegexOptions.Compiled);
    private static Regex OPERATION_REGEX    = new Regex(@"Operation: new = old (\*|\+) (\S+)", RegexOptions.Compiled);
    private static Regex DIVISOR_REGEX      = new Regex(@"Test: divisible by (\d+)",           RegexOptions.Compiled);
    private static Regex TRUE_MONKEY_REGEX  = new Regex(@"If true: throw to monkey (\d+)",     RegexOptions.Compiled);
    private static Regex FALSE_MONKEY_REGEX = new Regex(@"If false: throw to monkey (\d+)",    RegexOptions.Compiled);

    private Queue<long> _items;

    public Func<long, long> Operation         { get; }
    public long             Divisor           { get; }
    public int              TrueMonkey        { get; }
    public int              FalseMonkey       { get; }
    public int              NumInspectedItems { get; set; }

    public Monkey(IList<string> monkeyLines)
    {
        _items = new Queue<long>();
        NumInspectedItems = 0;

        // Get items
        Match match = ITEMS_REGEX.Match(monkeyLines[1]);
        foreach (var num in match.Groups[1].Value.Split(", ").Select(n => long.Parse(n)))
        {
            _items.Enqueue(num);
        }

        // Get operation
        match = OPERATION_REGEX.Match(monkeyLines[2]);
        var op    = match.Groups[1].Value;
        var right = match.Groups[2].Value;
        Operation = (old) => op switch
        {
            "*" => old * (long.TryParse(right, out var result) ? result : old),
            "+" => old + (long.TryParse(right, out var result) ? result : old),
            _   => throw new Exception($"Unrecognized operation {op}")
        };

        // Get divisor
        match = DIVISOR_REGEX.Match(monkeyLines[3]);
        Divisor = int.Parse(match.Groups[1].Value);

        // Get true monkey
        match = TRUE_MONKEY_REGEX.Match(monkeyLines[4]);
        TrueMonkey = int.Parse(match.Groups[1].Value);

        // Get false monkey
        match = FALSE_MONKEY_REGEX.Match(monkeyLines[5]);
        FalseMonkey = int.Parse(match.Groups[1].Value);
    }

    public bool  HasItems()          => _items.Count > 0;
    public long TakeItem()           => _items.Dequeue();
    public void  GiveItem(long item) => _items.Enqueue(item);
}
