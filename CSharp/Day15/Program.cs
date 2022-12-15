using System.Numerics;
using System.Text.RegularExpressions;
using Utils;

var code = new Code();

code.Check();
code.Run();

public class Code : Day
{
    public Dictionary<(long sensorX, long sensorY), (long beaconX, long beaconY)> Report { get; set; }
    public long Row { get; set; }
    public (long lower, long upper) Bounds { get; set; }

    public override void ParseInput(string filePath)
    {
        Regex lineRegex = new Regex(@"Sensor at x=(\-?\d+), y=(\-?\d+): closest beacon is at x=(\-?\d+), y=(\-?\d+)");

        (Row, Bounds) = filePath switch
        {
            "input.txt" => (2000000, (0, 4000000)),
            _           => (10,      (0, 20))
        };

        Report = new Dictionary<(long, long), (long, long)>();

        foreach (var line in File.ReadAllLines(filePath))
        {
            var m = lineRegex.Match(line);

            Report.Add((long.Parse(m.Groups[1].Value), long.Parse(m.Groups[2].Value)), (long.Parse(m.Groups[3].Value), long.Parse(m.Groups[4].Value)));
        }
    }

    public override void PartOne()
    {
        var ranges = Report.Select(kvp => GetVisibleRangeForRow(kvp.Key, kvp.Value, Row)).Where(r => r is not null).Cast<(long start, long end)>();

        var consolidated = ConsolidateRanges(ranges);

        long numSpaces = 0;
        var beaconsInRow = Report.Values.Where(b => b.beaconY == Row).Distinct();

        foreach (var range in consolidated)
        {
            var rangeSpaces = range.end - range.start + 1; // plus 1 because range is inclusive
            var beaconsInRange = beaconsInRow.Where(b => b.beaconX >= range.start && b.beaconX <= range.end).Count();

            rangeSpaces -= beaconsInRange;
            numSpaces += rangeSpaces;
        }

        Console.WriteLine(numSpaces);
    }

    public override void PartTwo()
    {
        for (int i = 0; i < Bounds.upper; i++)
        {
            var ranges = Report.Select(kvp => GetVisibleRangeForRow(kvp.Key, kvp.Value, i)).Where(r => r is not null).Cast<(long start, long end)>();
            var consolidated = ConsolidateRanges(ranges);

            if (consolidated.Count() < 2) continue;

            var beginRange = consolidated.FirstOrDefault(r => r.start <= Bounds.lower && r.end > Bounds.lower);
            var endRange   = consolidated.FirstOrDefault(r => r.start < Bounds.upper && r.end >= Bounds.upper);

            if (beginRange != default && endRange != default)
            {
                BigInteger x = beginRange.end + 1;
                x *= 4000000;
                x += i;

                Console.WriteLine(x);
                break;
            }
        }
    }

    public (long start, long end)? GetVisibleRangeForRow((long x, long y) sensor, (long x, long y) beacon, long row)
    {
        var maxRange = Math.Abs(beacon.x - sensor.x) + Math.Abs(beacon.y - sensor.y);

        var distanceToRow = Math.Abs(sensor.y - row);

        // Sensor can't see row at all
        if (distanceToRow > maxRange)
        {
            return null;
        }

        // Sensor can see the row
        var remainingDistance = maxRange - distanceToRow;

        return (sensor.x - remainingDistance, sensor.x + remainingDistance);
    }

    public IEnumerable<(long start, long end)> ConsolidateRanges(IEnumerable<(long start, long end)> ranges)
    {
        var consolidated = new List<(long, long)>();

        var queue = new Queue<(long start, long end)>();

        (long start, long end)? curRange = null;

        foreach (var range in ranges)
        {
            queue.Enqueue(range);
        }

        int numToCheck = queue.Count;

        while (queue.Count > 0)
        {
            if (curRange is null)
            {
                curRange = queue.Dequeue();
                numToCheck = queue.Count;
                continue;
            }

            var next = queue.Dequeue();

            // Overlaps
            if ((next.start <= curRange.Value.end && next.end >= curRange.Value.start) || 
                (curRange.Value.start <= next.end && curRange.Value.end >= next.start))
            {
                curRange = (Math.Min(next.start, curRange.Value.start), Math.Max(next.end, curRange.Value.end));
                numToCheck = queue.Count;
                continue;
            }

            // Doesn't overlap, but we haven't checked everything yet
            if (numToCheck > 0)
            {
                queue.Enqueue(next);
                numToCheck--;
                continue;
            }

            // Doesn't overlap, but we've looked through everything
            consolidated.Add(curRange.Value);
            curRange = next;
            numToCheck = queue.Count;
        }

        if (curRange is not null)
        {
            consolidated.Add(curRange.Value);
        }

        return consolidated;
    }
}
