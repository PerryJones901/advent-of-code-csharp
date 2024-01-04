using AdventOfCode2023.Helpers;
using System.Diagnostics;

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
            
            if (inputLine[i] == '#')
            {
                outputLine += new string(Enumerable.Repeat('.', i - (currentNumRocks + indexOfCurrentStartRock)).ToArray());
                outputLine += new string (Enumerable.Repeat('O', currentNumRocks).ToArray());
                outputLine += '#';

                currentNumRocks = 0;
                indexOfCurrentStartRock = i + 1;
            }

            if (i == inputLine.Length - 1)
            {
                // We've reached the end - add remaining spaces and rocks
                outputLine += new string(Enumerable.Repeat('.', inputLine.Length - (currentNumRocks + indexOfCurrentStartRock)).ToArray());
                outputLine += new string(Enumerable.Repeat('O', currentNumRocks).ToArray());
            }
        }

        if (outputLine.Length != inputLine.Length)
            throw new Exception("Output line length does not match input line length");

        return outputLine;
    }

    public static int Part2(List<string> input)
    {
        // Too high: 108098


        const int CYCLE_COUNT = 1_000_000_000;
        var grid = input;
        var hashMap = new Dictionary<int, Info>();

        for (int i = 0; i < CYCLE_COUNT; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                // Make columns the new rows
                var columns = Enumerable.Range(0, grid[0].Length).Select(x => string.Join("", grid.Select(row => row[x]).Reverse())).ToList();
                grid = columns.Select(GetOutputLine).ToList();
            }

            var hash = string.Join("", grid).GetHashCode();
            if (hashMap.ContainsKey(hash))
            {
                // So, work out the original occurance
                var cycleNum = hashMap[hash].CycleNum;
                var cycleDiff = i - cycleNum;

                // Workout num of steps left
                var stepsLeft = (CYCLE_COUNT - 1) - i;
                var cycleIndex = stepsLeft % cycleDiff;

                // Now, we need to find the cycleNum that matches the cycleIndex
                var hello = cycleNum + cycleIndex;

                var loadAmount = hashMap.First(x => x.Value.CycleNum == hello).Value.LoadAmount;
                return loadAmount;
            }

            var columnsHello = Enumerable.Range(0, grid[0].Length).Select(x => string.Join("", grid.Select(row => row[x]).Reverse())).ToList();

            hashMap.Add(hash, new Info {
                CycleNum = i,
                LoadAmount = columnsHello.Sum(GetLoadPart2)
            });
        }
        return 0;
    }

    private static int GetLoadPart2(string inputColumn)
    {
        return inputColumn.Select((x, ind) => x == 'O' ? ind + 1 : 0).Sum();
    }

    [DebuggerDisplay("CycleNum={CycleNum}, LoadAmount={LoadAmount}")]
    private class Info
    {
        public int CycleNum { get; set; }
        public int LoadAmount { get; set; }
    }

    private static List<string> GetSeparatedInputFromFile() =>
        FileInputHelper.GetStringListFromFile("Day14.txt");
}
