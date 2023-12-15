using AdventOfCode2023.Helpers;
using MathNet.Numerics.LinearAlgebra.Double;

namespace AdventOfCode2023.Days;

public abstract class Day09
{
    public static long Part1Answer() =>
        Part1(GetSeparatedInputFromFile());

    public static long Part2Answer() =>
        Part2(GetSeparatedInputFromFile());

    public static long Part1(List<string> input) => DoTheThing(input);
    public static long Part2(List<string> input) => DoTheThing(input, isPart2: true);

    private static long DoTheThing(List<string> input, bool isPart2 = false)
    {
        var sum = 0L;

        foreach (var seqString in input)
        {
            var splitArr = seqString.Split(' ').Select(int.Parse).ToList();
            if (isPart2) splitArr.Reverse();

            var currentLevel = splitArr;
            var lastEntriesOfLevels = new List<int> { splitArr.Last() };
            while (true)
            {
                var nextLevel = currentLevel.Zip(
                    currentLevel.Skip(1),
                    (a, b) => b - a
                ).ToList();

                if (nextLevel.All(x => x == 0))
                    break;

                lastEntriesOfLevels.Add(nextLevel.Last());
                currentLevel = nextLevel;
            }

            sum += lastEntriesOfLevels.Sum();
        }

        return sum;
    }

    private static List<string> GetSeparatedInputFromFile() =>
        FileInputHelper.GetStringListFromFile("Day09.txt");
}
