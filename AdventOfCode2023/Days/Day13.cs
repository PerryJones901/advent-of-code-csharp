using AdventOfCode2023.Helpers;

namespace AdventOfCode2023.Days;

public abstract class Day13
{
    private const string NEWLINE_CHAR = "\r\n";
    public static int Part1Answer() =>
        Part1(GetSeparatedInputFromFile());

    public static int Part2Answer() =>
        Part2(GetSeparatedInputFromFile());

    public static int Part1(List<string> input)
    {
        // 10100 -- not correct
        // 92826 -- not correct
        var inputGrids = input.Select(x => x.Split(NEWLINE_CHAR).ToList()).ToList();

        return inputGrids.Sum(GetSummary);
    }

    private static int GetSummary(List<string> inputGrid)
    {
        // Start with rows:
        var summaryInt = GetNumLinesBeforeReflection(inputGrid) * 100;

        var listOfColumns = Enumerable.Range(0, inputGrid[0].Length).Select(x => string.Join("", inputGrid.Select(row => row[x]))).ToList();
        summaryInt += GetNumLinesBeforeReflection(listOfColumns);

        return summaryInt;
    }

    private static int GetNumLinesBeforeReflection(List<string> inputGrid)
    {
        for (int i = 0; i < inputGrid.Count - 1; i++)
        {
            var nonMirrorPairFound = false;
            for (int j = 0; j < inputGrid.Count; j++)
            {
                var topIndex = i - j;
                var bottomIndex = i + (j + 1);
                var isTopInBounds = IsInBounds(topIndex, inputGrid.Count);
                var isBottomInBounds = IsInBounds(bottomIndex, inputGrid.Count);

                if (!isTopInBounds || !isBottomInBounds) break;

                if (inputGrid[topIndex] != inputGrid[bottomIndex])
                {
                    nonMirrorPairFound = true;
                    break;
                }
            }
            if (!nonMirrorPairFound)
            {
                return (i + 1);
            }
        }

        // hacky bit: return 0 if there's no reflections.
        return 0;
    }

    private static bool IsInBounds(int index, int size)
    {
        return (0 <= index && index < size);
    }

    public static int Part2(List<string> input)
    {
        var inputGrids = input.Select(x => x.Split(NEWLINE_CHAR).ToList()).ToList();

        return inputGrids.Sum(GetPart2Summary);
    }

    private static int GetPart2Summary(List<string> inputGrid)
    {
        // Start with rows:
        var summaryInt = GetNumLinesBeforeSmudgedReflection(inputGrid) * 100;

        var listOfColumns = Enumerable.Range(0, inputGrid[0].Length).Select(x => string.Join("", inputGrid.Select(row => row[x]))).ToList();
        summaryInt += GetNumLinesBeforeSmudgedReflection(listOfColumns);

        return summaryInt;
    }

    private static int GetNumLinesBeforeSmudgedReflection(List<string> inputGrid)
    {
        var inputGridAsCharGrid = inputGrid.Select(x => x.ToCharArray()).ToArray();

        for (int i = 0; i < inputGrid.Count - 1; i++)
        {
            var diffs = 0;
            for (int j = 0; j < inputGrid.Count; j++)
            {
                var topIndex = i - j;
                var bottomIndex = i + (j + 1);
                var isTopInBounds = IsInBounds(topIndex, inputGrid.Count);
                var isBottomInBounds = IsInBounds(bottomIndex, inputGrid.Count);

                if (!isTopInBounds || !isBottomInBounds) break;

                diffs += inputGrid[topIndex].Zip(inputGrid[bottomIndex]).Count(x => x.First != x.Second);
                if (diffs > 1) break;
            }
            if (diffs == 1)
            {
                return (i + 1);
            }
        }

        // hacky bit: return 0 if there's no reflections.
        return 0;
    }

    private static List<string> GetSeparatedInputFromFile() =>
        FileInputHelper.GetStringListFromFile("Day13.txt", $"{NEWLINE_CHAR}{NEWLINE_CHAR}");
}
