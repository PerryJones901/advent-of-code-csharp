using AdventOfCode2023.Helpers;

namespace AdventOfCode2023.Days;

public abstract class Day17
{
    public static int Part1Answer() =>
        Part1(GetSeparatedInputFromFile());

    public static int Part2Answer() =>
        Part2(GetSeparatedInputFromFile());

    public static int Part1(List<string> input)
    {
        var numberGrid = input.Select(x => StringHelper.GetNumberListFromString(x)).ToList();


        return 0;
    }

    public static int Part2(List<string> input)
    {
        return 0;
    }

    private static List<string> GetSeparatedInputFromFile() =>
        FileInputHelper.GetStringListFromFile("Day17.txt");
}
