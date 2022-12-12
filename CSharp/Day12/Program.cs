using System.Text;
using Utils;
using QuikGraph;
using System.Reflection.Metadata;

var code = new Code();

code.Check();
code.Run();

public class Code : Day
{
    public Node[,] Grid { get; set; } = null!;
    public (int x, int y) Start { get; set; }
    public (int x, int y) End { get; set; }

    public override void ParseInput(string filePath)
    {
        var lines = File.ReadAllLines(filePath);

        Grid = new Node[lines.Length, lines[0].Length];

        for (int i = 0; i < Grid.GetLength(0); i++)
        {
            for (int j = 0; j < Grid.GetLength(1); j++)
            {
                Grid[i, j] = new Node() { Character = lines[i][j] };
                Grid[i, j].Coordinate = (i, j);

                if (i > 0)
                {
                    Grid[i, j].Up = Grid[i - 1, j];
                    Grid[i - 1, j].Down = Grid[i, j];
                }

                if (j > 0)
                {
                    Grid[i, j].Left = Grid[i, j - 1];
                    Grid[i, j - 1].Right = Grid[i, j];
                }

                if (lines[i][j] == 'S')
                {
                    Start = (i, j);
                    Grid[i, j].Character = 'a';
                }
                if (lines[i][j] == 'E')
                {
                    End = (i, j);
                    Grid[i, j].Character = 'z';
                }
            }
        }
    }

    public override void PartOne()
    {
        Dijkstra(Start, false);

        Console.WriteLine(Grid[End.x, End.y].Distance);
    }

    public override void PartTwo()
    {
        Dijkstra(End, true);

        PrintGrid();

        int distance = int.MaxValue;

        foreach (var node in Grid)
        {
            if (node is not null && node.Character == 'a' && node.Distance < int.MaxValue)
            {
                distance = Math.Min(distance, node.Distance);
            }
        }

        Console.WriteLine(distance);
    }

    public void PrintGrid()
    {
        for (int i = 0; i < Grid.GetLength(0); i++)
        {
            for (int j = 0; j < Grid.GetLength(1); j++)
            {
                (Console.ForegroundColor, Console.BackgroundColor) = (Grid[i, j].Distance / 62) switch
                {
                    0  => (ConsoleColor.White,       ConsoleColor.Black),
                    1  => (ConsoleColor.Yellow,      ConsoleColor.Black),
                    2  => (ConsoleColor.Magenta,     ConsoleColor.Black),
                    3  => (ConsoleColor.Red,         ConsoleColor.Black),
                    4  => (ConsoleColor.Cyan,        ConsoleColor.Black),
                    5  => (ConsoleColor.Green,       ConsoleColor.Black),
                    6  => (ConsoleColor.Blue,        ConsoleColor.Black),
                    7  => (ConsoleColor.Gray,        ConsoleColor.Black),
                    8  => (ConsoleColor.DarkYellow,  ConsoleColor.Black),
                    9  => (ConsoleColor.DarkMagenta, ConsoleColor.Black),
                    10 => (ConsoleColor.DarkRed,     ConsoleColor.Black),
                    11 => (ConsoleColor.DarkCyan,    ConsoleColor.Black),
                    12 => (ConsoleColor.DarkGreen,   ConsoleColor.Black),
                    13 => (ConsoleColor.DarkBlue,    ConsoleColor.Black),
                    14 => (ConsoleColor.DarkGray,    ConsoleColor.Black),
                    _  => (ConsoleColor.Black,       ConsoleColor.White)
                };

                Console.Write(Grid[i, j].DistanceChar);
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(' ');
        }
    }

    public void Dijkstra((int y, int x) start, bool backward)
    {
        IList<Node> queue = new List<Node>();

        foreach (var node in Grid)
        {
            node.Distance = int.MaxValue;
            node.Previous = null!;
            node.Visited  = false;

            queue.Add(node);
        }

        Grid[start.y, start.x].Distance = 0;

        while (queue.Count > 0)
        {
            queue = queue.OrderBy(x => x.Distance).ToList();

            var curNode = queue[0];
            queue.RemoveAt(0);
            curNode.Visited = true;

            foreach (var neighbor in curNode.GetNeighbors().Where(n => !n.Visited))
            {
                int distance = curNode.CalculateNeighborDistance(neighbor, backward);

                if (distance < neighbor.Distance)
                {
                    neighbor.Distance = distance;
                    neighbor.Previous = curNode;
                }
            }
        }
    }

    public class Node
    {
        public char Character { get; set; }
        public int Distance { get; set; } = int.MaxValue;
        public Node Previous { get; set; } = null!;
        public bool Visited { get; set; } = false;

        public Node Up { get; set; }
        public Node Down { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }

        public (int Y, int X) Coordinate { get; set; }

        public char DistanceChar
        {
            get
            {
                return (Distance % 62) switch
                {
                    0 => '0',
                    1 => '1',
                    2 => '2',
                    3 => '3',
                    4 => '4',
                    5 => '5',
                    6 => '6',
                    7 => '7',
                    8 => '8',
                    9 => '9',
                    10 => 'a',
                    11 => 'b',
                    12 => 'c',
                    13 => 'd',
                    14 => 'e',
                    15 => 'f',
                    16 => 'g',
                    17 => 'h',
                    18 => 'i',
                    19 => 'j',
                    20 => 'k',
                    21 => 'l',
                    22 => 'm',
                    23 => 'n',
                    24 => 'o',
                    25 => 'p',
                    26 => 'q',
                    27 => 'r',
                    28 => 's',
                    29 => 't',
                    30 => 'u',
                    31 => 'v',
                    32 => 'w',
                    33 => 'x',
                    34 => 'y',
                    35 => 'z',
                    36 => 'A',
                    37 => 'B',
                    38 => 'C',
                    39 => 'D',
                    40 => 'E',
                    41 => 'F',
                    42 => 'G',
                    43 => 'H',
                    44 => 'I',
                    45 => 'J',
                    46 => 'K',
                    47 => 'L',
                    48 => 'M',
                    49 => 'N',
                    50 => 'O',
                    51 => 'P',
                    52 => 'Q',
                    53 => 'R',
                    54 => 'S',
                    55 => 'T',
                    56 => 'U',
                    57 => 'V',
                    58 => 'W',
                    59 => 'X',
                    60 => 'Y',
                    61 => 'Z',
                    _ => '.'
                };
            }
        }

        public IEnumerable<Node> GetNeighbors()
        {
            if (Up is not null) yield return Up;
            if (Down is not null) yield return Down;
            if (Left is not null) yield return Left;
            if (Right is not null) yield return Right;
        }

        public int CalculateNeighborDistance(Node dest, bool backward)
        {
            int distance = int.MaxValue;

            if (backward)
            {
                if (Character - dest.Character < 2)
                {
                    if (Distance < int.MaxValue)
                    {
                        distance = Distance + 1;
                    }
                }
            }
            else
            {
                if (dest.Character - Character < 2)
                {
                    if (DistanceChar < int.MaxValue)
                    {
                        distance = Distance + 1;
                    }
                }
            }

            return distance;
        }

        public override string ToString()
        {
            return $"{Character}, {Distance}";
        }
    }
}
