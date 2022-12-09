using Utils;

namespace Day9;

public class Program : Day
{
   public static void Main()
   {
      var code = new Program();

      code.Check();
      code.Run();
   }

   public IList<(char, int)> Instructions { get; set; } = null!;

   public override void ParseInput(string filePath)
   {
      Instructions = new List<(char, int)>();

      foreach (var line in File.ReadAllLines(filePath))
      {
         var parts = line.Split();

         Instructions.Add((char.Parse(parts[0]), int.Parse(parts[1])));
      }
   }

   public override void PartOne()
   {
      MoveRope(2);
   }

   public override void PartTwo()
   {
      MoveRope(10);
   }

   public void MoveRope(int ropeLength)
   {
      var positions = new HashSet<(int, int)>() { (0, 0) };

      (var head, var tail) = CreateRope(ropeLength);

      foreach ((char direction, int distance) in Instructions)
      {
         for (int i = 0; i < distance; i++)
         {
            MoveOneStep(head, direction);
            positions.Add((tail.X, tail.Y));
         }
      }

      Console.WriteLine(positions.Count);
   }

   public (Knot head, Knot tail) CreateRope(int length)
   {
      Knot[] rope = new Knot[length];

      for (int i = length - 1; i >= 0; i--)
      {
         rope[i] = new Knot();

         if (i < length - 1)
         {
            rope[i].Next = rope[i + 1];
         }
      }

      return (rope[0], rope[length - 1]);
   }

   public void MoveOneStep(Knot head, char direction)
   {
      head.Y += direction switch 
      {
         'U' =>  1,
         'D' => -1,
         _   =>  0
      };

      head.X += direction switch
      {
         'L' => -1,
         'R' =>  1,
         _   =>  0
      };

      var curKnot = head;

      while (curKnot.Next is not null)
      {
         MoveNextKnot(curKnot);
         curKnot = curKnot.Next;
      }
   }

   public void MoveNextKnot(Knot leader)
   {
      if (Math.Abs(leader.X - leader.Next!.X) > 1 || Math.Abs(leader.Y - leader.Next.Y) > 1)
      {
         if (leader.Y > leader.Next.Y && leader.X > leader.Next.X)
         {
            leader.Next.X++;
            leader.Next.Y++;
         }
         else if (leader.Y > leader.Next.Y && leader.X < leader.Next.X)
         {
            leader.Next.X--;
            leader.Next.Y++;
         }
         else if (leader.Y < leader.Next.Y && leader.X > leader.Next.X)
         {
            leader.Next.X++;
            leader.Next.Y--;
         }
         else if (leader.Y < leader.Next.Y && leader.X < leader.Next.X)
         {
            leader.Next.X--;
            leader.Next.Y--;
         }
         else if (leader.Y > leader.Next.Y + 1)
         {
            leader.Next.Y++;
         }
         else if (leader.Y < leader.Next.Y - 1)
         {
            leader.Next.Y--;
         }
         else if (leader.X > leader.Next.X + 1)
         {
            leader.Next.X++;
         }
         else if (leader.X < leader.Next.X - 1)
         {
            leader.Next.X--;
         }
      }
   }

   public class Knot
   {
      public int X { get; set; } = 0;
      public int Y { get; set; } = 0;

      public Knot? Next { get; set; }
   }
}