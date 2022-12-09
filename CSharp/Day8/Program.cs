using Utils;

namespace Day8;

public class Program : Day
{
   public Node[,] Grid { get; set; } = new Node[0, 0];

   public static void Main(string[] args)
   {
      var code = new Program();

      code.Check();
      code.Run();
   }

   public override void ParseInput(string filePath)
   {
      var lines = File.ReadAllLines(filePath);

      Grid = new Node[lines.Length, lines[0].Length];

      for (int y = 0; y < lines.Length; y++)
      {
         for (int x = 0; x < lines[y].Length; x++)
         {
            Grid[y, x] = new Node() { Height = int.Parse(lines[y][x].ToString()) };

            if (y > 0)
            {
               Grid[y, x]!.Up = Grid[y - 1, x];
               Grid[y - 1, x]!.Down = Grid[y, x];
            }

            if (x > 0)
            {
               Grid[y, x]!.Left = Grid[y, x - 1];
               Grid[y, x - 1]!.Right = Grid[y, x];
            }
         }
      }
   }

   public override void PartOne()
   {
      int total = 0;

      for (int y = 0; y < Grid.GetLength(0); y++)
      {
         for (int x = 0; x < Grid.GetLength(1); x++)
         {
            if (Grid[y, x].Visible())
            {
               total++;
            }
         }
      }

      Console.WriteLine(total);
   }

   public override void PartTwo()
   {
      int max = 0;

      for (int y = 0; y < Grid.GetLength(0); y++)
      {
         for (int x = 0; x < Grid.GetLength(1); x++)
         {
            max = Math.Max(max, Grid[y, x].ScenicScore());
         }
      }

      Console.WriteLine(max);
   }

   public class Node
   {
      public enum Direction
      {
         Up,
         Down,
         Left,
         Right
      }

      public Node? Left { get; set; }
      public Node? Right { get; set; }
      public Node? Up { get; set; }
      public Node? Down { get; set; }

      public int Height { get; set; }

      public bool Visible()
      {
         return Enum.GetValues<Direction>().Any(d => CheckDirection(d).visible);
      }

      public int ScenicScore()
      {
         int product = 1;

         foreach (var direction in Enum.GetValues<Direction>())
         {
            product *= CheckDirection(direction).score;
         }

         return product;
      }

      public (bool visible, int score) CheckDirection(Direction direction)
      {
         bool visible = true;
         int directionScore = 0;
         Node? curNode = direction switch
         {
            Direction.Up    => Up,
            Direction.Down  => Down,
            Direction.Left  => Left,
            Direction.Right => Right,
            _               => null
         };

         while (curNode != null)
         {
            directionScore++;

            if (curNode.Height >= Height)
            {
               visible = false;
               break;
            }

            curNode = direction switch
            {
               Direction.Up    => curNode.Up,
               Direction.Down  => curNode.Down,
               Direction.Left  => curNode.Left,
               Direction.Right => curNode.Right,
               _               => null
            };
         }

         return (visible, directionScore);
      }

      public override string ToString()
      {
         return Height.ToString();
      }
   }
}