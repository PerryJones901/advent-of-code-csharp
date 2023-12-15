using AdventOfCode2022.Helpers;

namespace AdventOfCode2022.Days
{
    public static class Day6
    {
        public static int Part1Answer() =>
            FindMarkerForNGroup(4);

        public static int Part2Answer() =>
            FindMarkerForNGroup(14);

        public static int FindMarkerForNGroup(int n)
        {
            var input = GetSeparatedInputFromFile();

            for (var i = n; i <= input.Length; i++)
            {
                var currentString = input[(i - n)..i];
                var currentSet = new HashSet<char>(currentString);
                if (currentSet.ToList().Count == n) return i;
            }

            return -1;
        }

        private static string GetSeparatedInputFromFile() => FileInputHelper
            .GetStringFromFile("Day6.txt");
    }
}
