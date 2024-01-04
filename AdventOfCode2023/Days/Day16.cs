using AdventOfCode2023.Helpers;

namespace AdventOfCode2023.Days;

public abstract class Day16
{
    public static int Part1Answer() =>
        Part1(GetSeparatedInputFromFile());

    public static int Part2Answer() =>
        Part2(GetSeparatedInputFromFile());

    public static int Part1(List<string> input)
    {
        // Keep track of output directions at each cell
        // When tracing a ray, if the output is the same as one done prior:
        //   Skip, as we've already done this ray

        // At the end, we tally up on the grid cells where at least one output

        var rayOutputs = Enumerable
            .Repeat(
                () => Enumerable.Repeat(() => "0000", input[0].Length).Select(x => x()).ToArray(),
                input.Count
            )
            .Select(x => x())
            .ToArray();

        // Slight cheat: don't go east (1), but go south (2) instead due to initial mirror
        SendRay(0,0,2, input, rayOutputs);

        return rayOutputs.Sum(x => x.Count(y => y != "0000"));
    }

    private static void SendRay(int row, int col, int outputDir, List<string> map, string[][] outputGrid)
    {
        var currentRow = row;
        var currentCol = col;
        var currentDirCode = outputDir;

        // Check output grid to see if we've already done this ray
        var currentCellOutputs = outputGrid[currentRow][currentCol];
        if (currentCellOutputs[currentDirCode] == '1')
            return;

        // Otherwise, mark this ray outputed in this direction
        outputGrid[currentRow][currentCol] =
            currentCellOutputs[..currentDirCode] + '1' + currentCellOutputs[(currentDirCode + 1)..];

        // Get the next cell
        var currentCoordsDiff = CoordsDiffs[currentDirCode];
        currentRow += currentCoordsDiff.Item1;
        currentCol += currentCoordsDiff.Item2;

        // Check if new cell is in bounds
        if (!IsInBounds(currentRow, currentCol, outputGrid))
            return;

        var currentMirrorChar = map[currentRow][currentCol];

        // If '.', keep going in same direction
        if (currentMirrorChar == '.')
        {
            SendRay(currentRow, currentCol, currentDirCode, map, outputGrid);
        }
        else
        {
            // Get outbound rays
            var outputDirs = MirrorToInputToOutputDirCodes[currentMirrorChar][currentDirCode];

            foreach (var newOutputDir in outputDirs)
            {
                SendRay(currentRow, currentCol, newOutputDir, map, outputGrid);
            }
        }
    }

    private static readonly List<(int, int)> CoordsDiffs = new()
    {
        (-1, 0),
        (0, 1),
        (1, 0),
        (0, -1)
    };

    private static readonly Dictionary<char, List<List<int>>> MirrorToInputToOutputDirCodes = new()
    {
        { '|', new(){
            /*N 0*/ new() { 0 },
            /*E 1*/ new() { 0, 2 },
            /*S 2*/ new() { 2 },
            /*W 3*/ new() { 0, 2 },
        }},
        { '-', new(){
            /*N 0*/ new() { 1, 3 },
            /*E 1*/ new() { 1 },
            /*S 2*/ new() { 1, 3 },
            /*W 3*/ new() { 3 },
        }},
        { '/', new(){
            /*N 0*/ new() { 1 },
            /*E 1*/ new() { 0 },
            /*S 2*/ new() { 3 },
            /*W 3*/ new() { 2 },
        }},
        { '\\', new(){
            /*N 0*/ new() { 3 },
            /*E 1*/ new() { 2 },
            /*S 2*/ new() { 1 },
            /*W 3*/ new() { 0 },
        }},
    };

    private static bool IsInBounds(int row, int col, string[][] outputGrid) =>
        0 <= row && row < outputGrid.Length && 0 <= col && col < outputGrid[0].Length;

    public static int Part2(List<string> input)
    {
        var maxEnergy = 0;

        // N to S
        for (var i = 0; i < input[0].Length; i++)
        {
            var energy = GetEnergy(0, i, 2, input);
            if (energy > maxEnergy)
                maxEnergy = energy;
        }

        // S to N
        for (var i = 0; i < input[0].Length; i++)
        {
            var energy = GetEnergy(input.Count - 1, i, 0, input);
            if (energy > maxEnergy)
                maxEnergy = energy;
        }

        // E to W
        for (var i = 0; i < input.Count; i++)
        {
            var energy = GetEnergy(i, input[0].Length - 1, 3, input);
            if (energy > maxEnergy)
                maxEnergy = energy;
        }

        // W to E
        for (var i = 0; i < input.Count; i++)
        {
            var energy = GetEnergy(i, 0, 1, input);
            if (energy > maxEnergy)
                maxEnergy = energy;
        }

        return maxEnergy;
    }

    private static int GetEnergy(int row, int col, int outputDir, List<string> input)
    {
        var rayOutputs = Enumerable
            .Repeat(
                () => Enumerable.Repeat(() => "0000", input[0].Length).Select(x => x()).ToArray(),
                input.Count
            )
            .Select(x => x())
            .ToArray();

        var dirCode = outputDir;
        var mirrorChar = input[row][col];
        // If '.', keep going in same direction
        if (mirrorChar == '.')
        {
            SendRay(row, col, dirCode, input, rayOutputs);
        }
        else
        {
            // Get outbound rays
            var outputDirs = MirrorToInputToOutputDirCodes[mirrorChar][dirCode];

            foreach (var newOutputDir in outputDirs)
            {
                SendRay(row, col, newOutputDir, input, rayOutputs);
            }
        }

        var rayOutputsStr = rayOutputs.Select(x => string.Join("", x.Select(x => x == "0000" ? "O" : "X").ToList())).ToArray();

        return rayOutputs.Sum(x => x.Count(y => y != "0000"));
    }   

    private static List<string> GetSeparatedInputFromFile() =>
        FileInputHelper.GetStringListFromFile("Day16.txt");
}
