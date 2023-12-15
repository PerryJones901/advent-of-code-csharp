using System.Text.RegularExpressions;
using System.Linq;

namespace AdventOfCode2022.Helpers
{
    public static class FileInputHelper
    {
        private const string PATH = "C:\\Users\\Perry\\source\\repos\\AdventOfCode2022\\AdventOfCode2022\\InputFiles";
        private const string LINE_SEPARATOR = "\r\n";

        public static List<int> GetNumberListFromFile(
            string fileName, string splitSeparator)
            => GetStringListFromFile(fileName, splitSeparator)
            .Select(int.Parse)
            .ToList();

        public static List<string> GetStringListFromFile(
            string fileName,
            string splitSeparator = LINE_SEPARATOR)
            => GetStringFromFile(fileName).Split(splitSeparator).ToList();

        public static List<List<string>> GetParamListsFromRegexFromFile(
            string fileName,
            string regexPattern,
            string splitSeparator = LINE_SEPARATOR)
        {
            var r = new Regex(regexPattern);

            return GetStringFromFile(fileName)
                .Split(splitSeparator)
                .Select(
                    x => r.Match(x).Groups.Values.Skip(1).Select(
                        x => x.Captures[0].Value
                    ).ToList()
                )
                .ToList();
        }

        public static string GetStringFromFile(string fileName)
            => File.ReadAllText($"{PATH}\\{fileName}");
    }
}
