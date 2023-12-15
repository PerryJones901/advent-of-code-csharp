using AdventOfCode2023.Helpers;

namespace AdventOfCode2023.Days;

public abstract class Day14
{
    public static int Part1Answer() =>
        Part1(GetSeparatedInputFromFile());

    public static int Part2Answer() =>
        Part2(GetSeparatedInputFromFile());

    public static int Part1(List<string> input)
    {
        var listOfColumns = Enumerable.Range(0, input[0].Length).Select(x => string.Join("", input.Select(row => row[x]))).ToList();
        return listOfColumns.Sum(GetLoad);
    }

    private static int GetLoad(string inputColumn)
    {
        var indexOfCurrentStartRock = 0; // Rocks will assemble ontop
        var currentNumRocks = 0;
        var totalLoad = 0;

        for (int i = 0; i < inputColumn.Length; i++)
        {
            if (inputColumn[i] == 'O')
                currentNumRocks++;
            
            if (inputColumn[i] == '#' || i == inputColumn.Length - 1)
            {
                // calculate load so far
                // This will be:
                //  (inputColumn.Length - indexOfCurrentStartRock),
                //  (inputColumn.Length - indexOfCurrentStartRock) - 1,
                //  ...
                //  (inputColumn.Length - indexOfCurrentStartRock) - (currentNumRocks - 1)

                var startAmountIncl = inputColumn.Length - indexOfCurrentStartRock;
                var endAmountExcl = startAmountIncl - currentNumRocks;

                totalLoad +=
                    (startAmountIncl)*(startAmountIncl + 1)/2 - (endAmountExcl)*(endAmountExcl + 1)/2;

                currentNumRocks = 0;
                indexOfCurrentStartRock = i + 1;
            }
        }

        return totalLoad;
    }

    private static string GetOutputLine(string inputLine)
    {
        var indexOfCurrentStartRock = 0; // Rocks will assemble ontop
        var currentNumRocks = 0;
        var outputLine = "";

        for (int i = 0; i < inputLine.Length; i++)
        {
            if (inputLine[i] == 'O')
                currentNumRocks++;
            
            if (inputLine[i] == '#' || i == inputLine.Length - 1)
            {
                outputLine += new string(Enumerable.Repeat('.', i - (currentNumRocks + indexOfCurrentStartRock)).ToArray());
                outputLine += new string (Enumerable.Repeat('O', currentNumRocks).ToArray());
                if (inputLine[i] == '#')
                    outputLine += '#';

                currentNumRocks = 0;
                indexOfCurrentStartRock = i + 1;
            }
        }

        return outputLine;
    }

    public static int Part2(List<string> input)
    {
        const int CYCLE_COUNT = 100;
        var grid = input;

        for (int i = 0; i < CYCLE_COUNT; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                // Make columns the new rows
                var columns = Enumerable.Range(0, grid[0].Length).Select(x => string.Join("", grid.Select(row => row[x]).Reverse())).ToList();
                grid = columns.Select(GetOutputLine).ToList();
            }
        }
        return 0;
    }

    private static List<string> GetSeparatedInputFromFile() =>
        FileInputHelper.GetStringListFromFile("Day14_Test.txt");
}
