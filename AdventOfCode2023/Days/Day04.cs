using AdventOfCode2023.Helpers;

namespace AdventOfCode2023.Days;

public abstract class Day04
{
    public static int Part1Answer() =>
        Part1(GetSeparatedInputFromFile());

    public static int Part2Answer() =>
        Part2(GetSeparatedInputFromFile());

    public static int Part1(List<string> input)
    {
        var sum = 0;

        foreach (var scratchCardString in input)
        {
            var sides = scratchCardString.Split(" | ");
            var leftSideList = sides[0]
                .Split(' ')
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Skip(2) // To skip "Card" and e.g. "23:"
                .Select(int.Parse)
                .ToList();

            var rightSideList = sides[1]
                .Split(' ')
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(int.Parse)
                .ToList();

            var winningNumberCount = 0;
            foreach (var num in rightSideList)
            {
                if (leftSideList.Contains(num))
                    winningNumberCount++;
            }

            if (winningNumberCount > 0)
                sum += (int)Math.Pow(2, winningNumberCount - 1);
        }
        return sum;
    }

    public static int Part2(List<string> input)
    {
        var winningNumList = new List<int>();

        foreach (var scratchCardString in input)
        {
            var sides = scratchCardString.Split(" | ");
            var leftSideList = sides[0]
                .Split(' ')
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Skip(2) // To skip "Card" and e.g. "23:"
                .Select(int.Parse)
                .ToList();

            var rightSideList = sides[1]
                .Split(' ')
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(int.Parse)
                .ToList();

            var winningNumberCount = 0;
            foreach (var num in rightSideList)
            {
                if (leftSideList.Contains(num))
                    winningNumberCount++;
            }

            winningNumList.Add(winningNumberCount);
        }

        var scratchCardItemTotals = Enumerable.Repeat(1, winningNumList.Count).ToList();
        for (int i = 0; i < winningNumList.Count; i++)
        {
            var winningNum = winningNumList[i];
            var cuurentScratchCardItemTotal = scratchCardItemTotals[i];

            for (int j = i + 1; j < i + 1 + winningNum; j++)
            {
                if (j >= winningNumList.Count) break;

                scratchCardItemTotals[j] += cuurentScratchCardItemTotal;
            }
        }

        return scratchCardItemTotals.Sum();
    }

    private static List<string> GetSeparatedInputFromFile() => 
        FileInputHelper.GetStringListFromFile("Day04.txt");
}
