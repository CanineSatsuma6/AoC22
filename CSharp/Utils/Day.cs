using System.Diagnostics;
using System.Text;

namespace Utils;

public abstract class Day
{
   public abstract void ParseInput(string filePath);
   public abstract void PartOne();
   public abstract void PartTwo();

   private void PrintHeading(string heading, bool printBox = false)
   {
      if (printBox)
      {
         Console.WriteLine(new StringBuilder()
                              .AppendLine("+" + new string('-', heading.Length + 2) + "+")
                              .AppendLine("| " + heading + " |")
                              .AppendLine("+" + new string('-', heading.Length + 2) + "+"));
      }
      else
      {
         Console.WriteLine(new StringBuilder()
                              .AppendLine(heading)
                              .AppendLine(new string('-', heading.Length))
                              .ToString());
      }
   }

   private void TimeExecution(string heading, Action function)
   {
      var timer = new Stopwatch();

      PrintHeading(heading);
      timer.Start();
      function();
      timer.Stop();
      Console.WriteLine(new StringBuilder()
                           .AppendLine()
                           .AppendLine($"Elapsed Time: {timer.ElapsedMilliseconds / 1000}.{timer.ElapsedMilliseconds % 1000}s")
                           .ToString());
   }

   private void Execute(bool testing)
   {
      var filePath = testing ? "example.txt" : "input.txt";

      PrintHeading(testing ? "Check" : "Execute", true);
      ParseInput(filePath);
      TimeExecution("Part 1", PartOne);
      TimeExecution("Part 2", PartTwo);
   }

   public void Run()
   {
      Execute(false);
   }

   public void Check()
   {
      Execute(true);
   }
}
