using AdventOfCode2022.Helpers;

namespace AdventOfCode2022.Days
{
    public static class Day3
    {
        public static int Part1Answer() =>
            GetSeparatedInputFromFile().Select(x => GetRepeatingChar(x)).Select(x => GetPriorityFromChar(x)).Sum();

        public static int Part2Answer() =>
            GetSeparatedInputFromFile().Chunk(3).Select(x => GetRepeatingCharFromList(x)).Select(x => GetPriorityFromChar(x)).Sum();

        public static List<string> GetSeparatedInputFromFile() => FileInputHelper
                .GetStringListFromFile("Day3.txt");

        public static char GetRepeatingChar(string rucksackString)
        {
            var length = rucksackString.Length;
            var comp1 = rucksackString[..(length / 2)];
            var comp2 = rucksackString[(length / 2)..];

            var setOfCharsInComp1 = new HashSet<char>(comp1);
            foreach (var c in comp2)
            {
                if (setOfCharsInComp1.Contains(c)) return c;
            }

            throw new Exception($"No repeating char found in rucksackString: {rucksackString}");
        }

        public static char GetRepeatingCharFromList(IEnumerable<string> listOfStrings) =>
            (listOfStrings.Aggregate(listOfStrings.First(), (str1, str2) => GetRepeatingChars(str1, str2))).Single();

        public static string GetRepeatingChars(string str1, string str2) =>
            String.Concat((new HashSet<char>(str1)).Intersect(new HashSet<char>(str2))?.ToList() ?? new List<char>());

        public static int GetPriorityFromChar(char c) =>
            (int)c > 96 
                ? ((int)c - 'a' + 1)
                : ((int)c - 'A' + 27);
    }
}
