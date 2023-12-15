using AdventOfCode2022.Helpers;

namespace AdventOfCode2022.Days
{
    public static class Day1
    {
        private const string LINE_SEPARATOR = "\r\n";
        public static int Part1Answer() => GetListOfTotalCalsPerElf().Max();

        public static int Part2Answer() => GetListOfTotalCalsPerElf().ToList().OrderByDescending(x => x).Take(3).Sum();

        public static List<string> GetSeparatedInputFromFile() => FileInputHelper
                .GetStringListFromFile("Day1.txt", $"{LINE_SEPARATOR}{LINE_SEPARATOR}");

        public static IEnumerable<int> GetListOfTotalCalsPerElf() =>
            GetSeparatedInputFromFile().Select(x => x.Split("\r\n").Select(y => int.Parse(y)).Sum());
    }
}
