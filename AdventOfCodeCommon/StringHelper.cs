using System.Text.RegularExpressions;

namespace AdventOfCodeCommon
{
    public class StringHelper
    {
        public const string LINE_SEPARATOR = "\r\n";

        public static List<List<string>> GetRegexCapturesFromInput(
            string input,
            string regexPattern,
            string splitSeparator = LINE_SEPARATOR)
        {
            var r = new Regex(regexPattern);

            return input
                .Split(splitSeparator)
                .Select(
                    x => r.Match(x).Groups.Values.Skip(1).Select(
                        x => x.Captures[0].Value
                    ).ToList()
                )
                .ToList();
        }

        public static List<int> GetNumberListFromString(string input) =>
            input.Select(x => int.Parse(x.ToString())).ToList();
    }
}
