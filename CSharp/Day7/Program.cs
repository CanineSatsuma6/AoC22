using System.Numerics;
using System.Text.RegularExpressions;

using Utils;

namespace Day7;

public class Program : Day
{
   private readonly Regex CD   = new Regex(@"\$\scd\s(\S+)", RegexOptions.Compiled);
   private readonly Regex DIR  = new Regex(@"dir\s(\S+)", RegexOptions.Compiled);
   private readonly Regex FILE = new Regex(@"(\d+)\s(\S+)", RegexOptions.Compiled);

   private List<Folder> _directories = new List<Folder>();
   private Folder       _root        = null!;

   private IEnumerable<string> _puzzleInput = null!;
    
   public static void Main(string[] args)
   {
      var code = new Program();

      code.Check();
      code.Run();
   }

   public override void ParseInput(string filePath)
   {
      _puzzleInput = File.ReadAllLines(filePath);
   }

   public override void PartOne()
   {
      EvaluateInstructions();

      Console.WriteLine(_directories.Where(d => d.Size <= 100000).Sum(d => (long)d.Size));
   }

   public override void PartTwo()
   {
      EvaluateInstructions();

      var sysSize    = BigInteger.Parse("70000000");
      var neededSize = BigInteger.Parse("30000000");
      var usedSize   = _root.Size;

      var deleteSize = neededSize - (sysSize - usedSize);

      Console.WriteLine(_directories.Where(d => d.Size > deleteSize).Min(d => d.Size));
   }

   private void EvaluateInstructions()
   {
      Folder? curDirectory = null;
      _root = new Folder() { Name = "/" };

      _directories = new List<Folder>() { _root };

      foreach (var line in _puzzleInput)
      {
         var cdMatch = CD.Match(line);
         var dirMatch = DIR.Match(line);
         var filMatch = FILE.Match(line);

         if (cdMatch.Success)
         {
            var cdName = cdMatch.Groups[1].Value;

            switch (cdName)
            {
               case "/":
                  curDirectory = _root;
                  break;

               case "..":
                  if (curDirectory is not null)
                  {
                     curDirectory = curDirectory.Parent;
                  }
                  break;

               default:
                  var child = curDirectory?.Directories?.FirstOrDefault(d => d.Name == cdName);

                  if (child is null && curDirectory is not null)
                  {
                     child = new Folder() { Name = cdName, Parent = curDirectory! };
                     _directories.Add(child);
                  }

                  curDirectory = child;
                  break;
            }
         }

         if (dirMatch.Success)
         {
            var dirName = dirMatch.Groups[1].Value;

            if (curDirectory is not null && !curDirectory.Directories.Any(d => d.Name == dirName))
            {
               var child = new Folder() { Name = dirName, Parent = curDirectory! };
               curDirectory!.Directories.Add(child);
               _directories.Add(child);
            }
         }

         if (filMatch.Success)
         {
            var fileName = filMatch.Groups[2].Value;
            var size     = filMatch.Groups[1].Value;

            if (curDirectory is not null && !curDirectory.Files.Any(f => f.Name == fileName))
            {
               curDirectory!.Files.Add(new ComputerFile() { Name = fileName, Size = BigInteger.Parse(size) });
            }
         }
      }

      _directories.ForEach(d => d.RefreshSize());
   }

   public class Folder
   {
      public string Name { get; set; } = "";
      public List<Folder> Directories { get; set; } = new List<Folder>();
      public List<ComputerFile> Files { get; set; } = new List<ComputerFile>();
      public Folder Parent { get; set; } = null!;
      public BigInteger Size { get; private set; } = 0;

      public BigInteger RefreshSize()
      {
         BigInteger total = 0;

         foreach (var file in Files)
         {
            total += file.Size;
         }

         foreach (var directory in Directories)
         {
            total += directory.RefreshSize();
         }

         Size = total;

         return total;
      }
   }

   public class ComputerFile 
   {
      public string Name { get; set; } = "";
      public BigInteger Size { get; set; } = 0;
   }
}