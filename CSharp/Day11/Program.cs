using System.Numerics;
using Utils;

var code = new Code();

code.Check();
code.Run();

public class Code : Day
{
    public Dictionary<int, Monkey> Monkeys { get; set; } = new Dictionary<int, Monkey>();

    public override void ParseInput(string filePath)
    {
        if (filePath == "input.txt")
        {
            Monkeys = new Dictionary<int, Monkey>()
            {
                {
                    0,
                    new Monkey()
                    {
                        Items = new List<BigInteger> { 54, 82, 90, 88, 86, 54 },
                        Operation = (old) => old * 7,
                        Test = (level) => level % 11 == 0
                    }
                },
                {
                    1,
                    new Monkey()
                    {
                        Items = new List<BigInteger> { 91, 65 },
                        Operation = (old) => old * 13,
                        Test = (level) => level % 5 == 0
                    }
                },
                {
                    2,
                    new Monkey()
                    {
                        Items = new List<BigInteger> { 62, 54, 57, 92, 83, 63, 63 },
                        Operation = (old) => old + 1,
                        Test = (level) => level % 7 == 0
                    }
                },
                {
                    3,
                    new Monkey()
                    {
                        Items = new List<BigInteger> { 67, 72, 68 },
                        Operation = (old) => old * old,
                        Test = (level) => level % 2 == 0
                    }
                },
                {
                    4,
                    new Monkey()
                    {
                        Items = new List<BigInteger> { 68, 89, 90, 86, 84, 57, 72, 84 },
                        Operation = (old) => old + 7,
                        Test = (level) => level % 17 == 0
                    }
                },
                {
                    5,
                    new Monkey()
                    {
                        Items = new List<BigInteger> { 79, 83, 64, 58 },
                        Operation = (old) => old + 6,
                        Test = (level) => level % 13 == 0
                    }
                },
                {
                    6,
                    new Monkey()
                    {
                        Items = new List<BigInteger> { 96, 72, 89, 70, 88 },
                        Operation = (old) => old + 4,
                        Test = (level) => level % 3 == 0
                    }
                },
                {
                    7,
                    new Monkey()
                    {
                        Items = new List<BigInteger> { 79 },
                        Operation = (old) => old + 8,
                        Test = (level) => level % 19 == 0
                    }
                }
            };

            Monkeys[0].TrueMonkey = Monkeys[2];
            Monkeys[0].FalseMonkey = Monkeys[6];

            Monkeys[1].TrueMonkey = Monkeys[7];
            Monkeys[1].FalseMonkey = Monkeys[4];

            Monkeys[2].TrueMonkey = Monkeys[1];
            Monkeys[2].FalseMonkey = Monkeys[7];

            Monkeys[3].TrueMonkey = Monkeys[0];
            Monkeys[3].FalseMonkey = Monkeys[6];

            Monkeys[4].TrueMonkey = Monkeys[3];
            Monkeys[4].FalseMonkey = Monkeys[5];

            Monkeys[5].TrueMonkey = Monkeys[3];
            Monkeys[5].FalseMonkey = Monkeys[0];

            Monkeys[6].TrueMonkey = Monkeys[1];
            Monkeys[6].FalseMonkey = Monkeys[2];

            Monkeys[7].TrueMonkey = Monkeys[4];
            Monkeys[7].FalseMonkey = Monkeys[5];
        }
        else
        {
            Monkeys = new Dictionary<int, Monkey>()
            {
                {
                    0,
                    new Monkey()
                    {
                        Items = new List<BigInteger> { 79, 98 },
                        Operation = (old) => old * 19,
                        Test = (level) => level % 23 == 0
                    }
                },
                {
                    1,
                    new Monkey()
                    {
                        Items = new List<BigInteger> { 54, 65, 75, 74 },
                        Operation = (old) => old + 6,
                        Test = (level) => level % 19 == 0
                    }
                },
                {
                    2,
                    new Monkey()
                    {
                        Items = new List<BigInteger> { 79, 60, 97 },
                        Operation = (old) => old * old,
                        Test = (level) => level % 13 == 0
                    }
                },
                {
                    3,
                    new Monkey()
                    {
                        Items = new List<BigInteger> { 74 },
                        Operation = (old) => old + 3,
                        Test = (level) => level % 17 == 0
                    }
                }
            };

            Monkeys[0].TrueMonkey = Monkeys[2];
            Monkeys[0].FalseMonkey = Monkeys[3];

            Monkeys[1].TrueMonkey = Monkeys[2];
            Monkeys[1].FalseMonkey = Monkeys[0];

            Monkeys[2].TrueMonkey = Monkeys[1];
            Monkeys[2].FalseMonkey = Monkeys[3];

            Monkeys[3].TrueMonkey = Monkeys[0];
            Monkeys[3].FalseMonkey = Monkeys[1];
        }
    }

    public override void PartOne()
    {
        BigInt num = "321681435089146815617";

        num += "821681435089146815617";

        //for (int i = 0; i < 10000; i++)
        //{
        //    foreach ((int id, Monkey monkey) in Monkeys.OrderBy(kvp => kvp.Key))
        //    {
        //        monkey.InspectAllItems();
        //    }
        //}

        //var monkeys = Monkeys.Values.OrderByDescending(m => m.InspectedItems).Take(2).ToList();

        //Console.WriteLine(monkeys[0].InspectedItems * monkeys[1].InspectedItems);
    }

    public override void PartTwo()
    {

    }
}



public class Monkey
{
    public int ID { get; set; }

    public List<BigInteger> Items { get; set; } = new List<BigInteger>();
    public Func<BigInteger, BigInteger> Operation { get; set; }
    public Func<BigInteger, bool> Test { get; set; }
    public Monkey TrueMonkey { get; set; }
    public Monkey FalseMonkey { get; set; }

    public int InspectedItems { get; set; } = 0;

    public void InspectAllItems()
    {
        int itemCount = Items.Count;

        for (int i = 0; i < itemCount; i++)
        {
            InspectOneItem();
        }
    }

    public void InspectOneItem()
    {
        BigInteger item = Items[0];

        item = Operation(item);

//        item /= 3;

        if (Test(item))
        {
            TossItem(item, TrueMonkey);
        }
        else
        {
            TossItem(item, FalseMonkey);
        }

        InspectedItems++;
    }

    public void TossItem(BigInteger curWorryLevel, Monkey destMonkey)
    {
        Items.RemoveAt(0);

        destMonkey.Items.Add(curWorryLevel);
    }
}
