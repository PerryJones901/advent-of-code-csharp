using AdventOfCode2022.Helpers;

namespace AdventOfCode2022.Days
{
    public static class DayTemplate
    {
        public static int Part1Answer() =>
            Part1(GetSeparatedInputFromFile());

        public static int Part2Answer() =>
            Part2(GetSeparatedInputFromFile());

        public static int Part1(List<string> input)
        {
            return 0;
        }

        public static int Part2(List<string> input)
        {
            return 0;
        }

        private static List<string> GetSeparatedInputFromFile() => 
            FileInputHelper.GetStringListFromFile("Day0.txt");
    }
}
