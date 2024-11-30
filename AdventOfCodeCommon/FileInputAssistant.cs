using System.Text.RegularExpressions;

namespace AdventOfCodeCommon
{
    public class FileInputAssistant
    {
        private const string LINE_SEPARATOR = "\r\n";

        public static string GetStringFromFile(string filePath)
            => File.ReadAllText(filePath);

        public static string[] GetStringArrayFromFile(
            string fileName,
            string splitSeparator = LINE_SEPARATOR)
            => GetStringFromFile(fileName)
                .Split(splitSeparator);

        public static IEnumerable<T> GetEnumerableFromFile<T>(
            string fileName,
            string splitSeparator,
            Func<string, T> parseFunc)
            => GetStringArrayFromFile(fileName, splitSeparator)
                .Select(parseFunc);

        public static char[][] GetCharArrayRowsFromFile(
            string filePath,
            string splitSeparator = LINE_SEPARATOR)
        {
            return GetEnumerableFromFile(
                filePath,
                splitSeparator,
                (string x) => x.ToCharArray()
            ).ToArray();
        }

        public static int[] GetIntRowsFromFile(
            string filePath,
            string splitSeparator = LINE_SEPARATOR)
        {
            return GetEnumerableFromFile(
                filePath,
                splitSeparator,
                int.Parse
            ).ToArray();
        }

        public static int[][] GetIntArrayRowsFromFile(
            string filePath,
            string splitSeparator = LINE_SEPARATOR)
        {
            return GetEnumerableFromFile(
                filePath,
                splitSeparator,
                (string x) => x.Select(c => int.Parse(c.ToString())).ToArray()
            ).ToArray();
        }

        public static string[][] GetParamListsFromRegexFromFile(
            string fileName,
            string regexPattern,
            string splitSeparator = LINE_SEPARATOR)
        {
            var r = new Regex(regexPattern);

            return GetStringArrayFromFile(fileName, splitSeparator)
                .Select(
                    x => r.Match(x).Groups.Values.Skip(1).Select(
                        x => x.Captures[0].Value
                    ).ToArray()
                )
                .ToArray();
        }
    }
}
