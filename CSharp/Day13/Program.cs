using Utils;

var code = new Code();

code.Check();
code.Run();

public class Code : Day
{
    public List<(Value left, Value right)> Pairs { get; set; }

    public override void ParseInput(string filePath)
    {
        Pairs = new List<(Value, Value)>();

        var lines = File.ReadAllLines(filePath);

        for (int i = 0; i < lines.Length; i += 3)
        {
            Pairs.Add((new ListValue(lines[i]), new ListValue(lines[i + 1])));
        }
    }

    public override void PartOne()
    {
        int result = 0;

        for (int i = 0; i < Pairs.Count; i++)
        {
            var pair = Pairs[i];
            if (pair.left.CompareTo(pair.right) < 0)
            {
                result += i + 1;
            }
        }

        Console.WriteLine(result);
    }

    public override void PartTwo()
    {
        var dividerPacket2 = new ListValue("[[2]]");
        var dividerPacket6 = new ListValue("[[6]]");

        var allPackets = new List<Value>()
        {
            dividerPacket2,
            dividerPacket6
        };

        foreach ((var left, var right) in Pairs)
        {
            allPackets.Add(left);
            allPackets.Add(right);
        }

        allPackets.Sort();

        Console.WriteLine((allPackets.IndexOf(dividerPacket2) + 1) * (allPackets.IndexOf(dividerPacket6) + 1));
    }
}

public abstract class Value : IComparable<NumValue>, IComparable<ListValue>, IComparable<Value>
{
    public abstract int CompareTo(NumValue other);
    public abstract int CompareTo(ListValue other);

    public int CompareTo(Value other)
    {
        return other switch
        {
            ListValue list  => CompareTo(list),
            NumValue  value => CompareTo(value),
            _               => throw new Exception()
        };
    }
}

public class NumValue : Value, IComparable<NumValue>, IComparable<ListValue>
{
    public int Value { get; set; }

    public NumValue(int value)
    {
        Value = value;
    }

    public ListValue ToListValue()
    {
        return new ListValue(new NumValue[] { this });
    }

    public override int CompareTo(NumValue other)
    {
        return Value.CompareTo(other.Value);
    }

    public override int CompareTo(ListValue other)
    {
        var list = this.ToListValue();

        return list.CompareTo(other);
    }
}

public class ListValue : Value, IComparable<NumValue>, IComparable<ListValue>
{
    public IList<Value> Values { get; set; }

    public ListValue(string value)
    {
        Values = new List<Value>();

        // Remove leading/trailing '[' and ']'
        value = value[1..(value.Length - 1)];

        string curVal = "";
        int childDepth = 0;

        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] == '[')
            {
                curVal += '[';
                childDepth++;
            }
            else if (value[i] == ']')
            {
                curVal += ']';
                childDepth--;
            }
            else if (value[i] == ',' && childDepth == 0)
            {
                if (curVal.StartsWith('['))
                {
                    Values.Add(new ListValue(curVal));
                }
                else
                {
                    Values.Add(new NumValue(int.Parse(curVal)));
                }

                curVal = "";
            }
            else
            {
                curVal += value[i];
            }
        }

        if (!string.IsNullOrWhiteSpace(curVal))
        {
            if (curVal.StartsWith('['))
            {
                Values.Add(new ListValue(curVal));
            }
            else
            {
                Values.Add(new NumValue(int.Parse(curVal)));
            }
        }
    }

    public ListValue(IList<Value> values)
    {
        Values = values;
    }

    public override int CompareTo(NumValue other)
    {
        var list = other.ToListValue();

        return this.CompareTo(list);
    }

    public override int CompareTo(ListValue other)
    {
        int i = 0;

        for (; i < Values.Count; i++)
        {
            if (i >= other.Values.Count) // other ran out of items
            {
                return 1;
            }

            var left = Values[i];
            var right = other.Values[i];
            var result = left.CompareTo(right);

            if (result != 0)
            {
                return result;
            }
        }

        if (i < other.Values.Count) // left ran out of items
        {
            return -1;
        }

        return 0;
    }
}