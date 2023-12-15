using AdventOfCode2023.Helpers;

namespace AdventOfCode2023.Days;

public abstract class Day01
{
    public static int Part1Answer() =>
        Part1(GetSeparatedInputFromFile());

    public static int Part2Answer() =>
        Part2(GetSeparatedInputFromFile());

    public static int Part1(List<string> input)
    {
        return SumOfFirstAndLastNumbersInList(input);
    }

    public static int Part2(List<string> input)
    {
        var numberList = input
            .Select(x => $"{GetRealNumber(x, Side.First)}{GetRealNumber(x, Side.Last)}").ToList();

        var sum = numberList
            .Select(x => int.Parse(x))
            .Sum();

        return sum;
    }

    private static int SumOfFirstAndLastNumbersInList(List<string> input)
    {
        var numberList = input.Select(
            x => x
                .ToList()
                .Where(char.IsDigit)
                .Aggregate(
                    "",
                    (current, next) => current + next
                )
            );

        var firstAndLastDigitsSum = numberList
            .Select(x => x.ToList().First().ToString() + x.ToList().Last().ToString())
            .Select(x => int.Parse(x))
            .Sum();

        return firstAndLastDigitsSum;
    }


    private static List<string> GetSeparatedInputFromFile() => 
        FileInputHelper.GetStringListFromFile("Day01.txt");

    private static string ReplaceNumberWordsWithNumberForm(string x)
    {
        var newString = x;

        foreach (var keyValuePair in NumberWordsToNumberForm)
        {
            newString = newString.Replace(keyValuePair.Key, keyValuePair.Value);
        }

        return newString;
    }

    private enum Side
    {
        First,
        Last,
    }

    private static string GetRealNumber(string x, Side side)
    {
        var indexFindList = new Dictionary<string, int>();
        foreach (var realNumber in AllRealNumbers)
        {
            var findIndex = side == Side.First
                ? x.IndexOf(realNumber)
                : x.LastIndexOf(realNumber);
            if (findIndex < 0) continue;

            indexFindList.Add(realNumber, findIndex);
        }

        // Get key of lowest index
        var bestKeyValuePair = indexFindList.First();

        if (side == Side.First)
        {
            foreach (var keyValuePair in indexFindList)
            {
                if (keyValuePair.Value < bestKeyValuePair.Value)
                    bestKeyValuePair = keyValuePair;
            }
        }
        else if (side == Side.Last)
        {
            foreach (var keyValuePair in indexFindList)
            {
                if (keyValuePair.Value > bestKeyValuePair.Value)
                    bestKeyValuePair = keyValuePair;
            }
        }
        else throw new Exception("Side not valid");


        return ReplaceNumberWordsWithNumberForm(bestKeyValuePair.Key);
    }

    private static readonly Dictionary<string, string> NumberWordsToNumberForm = new()
    {
        { "one", "1" },
        { "two", "2" },
        { "three", "3" },
        { "four", "4" },
        { "five", "5" },
        { "six", "6" },
        { "seven", "7" },
        { "eight", "8" },
        { "nine", "9" },
    };

    private static readonly List<string> AllRealNumbers = new()
    {
        "1", "2", "3", "4", "5", "6", "7", "8", "9",
        "one", "two", "three", "four", "five", "six", "seven", "eight", "nine",
    };
}
