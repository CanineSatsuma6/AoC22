using System.Text;
using Utils;

var code = new Code();

code.Check();
code.Run();

public class Code : Day
{
    public IList<int> Instructions { get; set; }

    public override void ParseInput(string filePath)
    {
        Instructions = new List<int>();

        foreach (var line in File.ReadAllLines(filePath))
        {
            var parts = line.Split();

            int amount = parts.Length switch
            {
                2 => int.Parse(parts[1]),   // addx
                _ => 0                      // noop and default
            };

            Instructions.Add(amount);
        }
    }

    public override void PartOne()
    {
        int cycle = 1;
        int register = 1;
        int sum = 0;

        foreach (var amount in Instructions)
        {
            sum += GetSignalStrength(0, ref cycle, ref register);

            if (amount != 0)
            {
                sum += GetSignalStrength(amount, ref cycle, ref register);
            }
        }

        Console.WriteLine(sum);
    }

    public int GetSignalStrength(int amount, ref int cycle, ref int register)
    {
        int strength = 0;

        if (cycle % 40 == 20)
        {
            strength = cycle * register;
        }

        cycle++;
        register += amount;

        return strength;
    }

    public override void PartTwo()
    {
        var grid = new char[6, 40];
        int cycle = 0;
        int register = 1;

        foreach (var amount in Instructions)
        {
            WriteCRT(0, ref grid, ref cycle, ref register);

            if (amount != 0)
            {
                WriteCRT(amount, ref grid, ref cycle, ref register);
            }
        }

        PrintGrid(grid);
    }

    public void WriteCRT(int amount, ref char[,] grid, ref int cycle, ref int register)
    {
        int column = cycle % 40;
        int row    = cycle / 40;

        grid[row, column] = Math.Abs(column - register) < 2 ? 'O' : ' ';

        cycle++;
        register += amount;
    }

    public void PrintGrid(char[,] grid)
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                sb.Append(grid[i, j]);
            }

            sb.AppendLine();
        }

        Console.WriteLine(sb.ToString());
    }
}
