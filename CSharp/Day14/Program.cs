using Utils;

var code = new Code();

code.Check();
code.Run();

public class Code : Day
{
    public Dictionary<(int x, int y), char> Cave { get; set; }
    public int MaxDepth { get; set; }

    public override void ParseInput(string filePath)
    {
        Cave = new Dictionary<(int, int), char>();

        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            var coordList = line.Split(" -> ");

            for (int i = 0; i < coordList.Length - 1; i++)
            {
                var left = ToCoordinate(coordList[i]);
                var right = ToCoordinate(coordList[i + 1]);

                // vertical line
                if (left.x == right.x)
                {
                    for (int d = Math.Min(left.y, right.y); d <= Math.Max(left.y, right.y); d++)
                    {
                        Cave[(left.x, d)] = '#';
                    }

                    MaxDepth = Math.Max(Math.Max(left.y, right.y), MaxDepth);
                }
                // horizontal line
                else
                {
                    for (int h = Math.Min(left.x, right.x); h <= Math.Max(left.x, right.x); h++)
                    {
                        Cave[(h, left.y)] = '#';
                    }

                    MaxDepth = Math.Max(left.y, MaxDepth);
                }
            }
        }
    }

    public override void PartOne()
    {
        while (DropSand(false));

        Console.WriteLine(Cave.Values.Where(c => c == 'o').Count());
    }

    public override void PartTwo()
    {
        while (DropSand(true));

        Console.WriteLine(Cave.Values.Where(c => c == 'o').Count());
    }

    public (int x, int y) ToCoordinate(string inputCoord)
    {
        var coords = inputCoord.Split(",");

        return (int.Parse(coords[0]), int.Parse(coords[1]));
    }

    public bool DropSand(bool hasFloor)
    {
        (var x, var y) = (500, 0);
        var placeable = false;

        while (y < MaxDepth || hasFloor)
        {
            if (!Cave.ContainsKey((x, y + 1)) && (y != MaxDepth + 1))
            {
                y++;
                continue;
            }

            if (!Cave.ContainsKey((x - 1, y + 1)) && (y != MaxDepth + 1))
            {
                x--;
                y++;
                continue;
            }

            if (!Cave.ContainsKey((x + 1, y + 1)) && (y != MaxDepth + 1))
            {
                x++;
                y++;
                continue;
            }

            if (Cave.ContainsKey((x, y)))
            {
                break;
            }

            placeable = true;
            break;
        }

        if (placeable)
        {
            Cave.Add((x, y), 'o');
        }

        return placeable;
    }
}
