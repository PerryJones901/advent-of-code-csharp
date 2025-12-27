using AdventOfCodeCommon;

namespace AdventOfCode2015.Days
{
    internal class Day05(bool isTest) : DayBase(5, isTest)
    {
        private static readonly IReadOnlyCollection<char> _vowels = [
            'a', 'e', 'i', 'o', 'u'
        ];

        private static readonly IReadOnlyCollection<string> _bannedSubStrings = [
            "ab", "cd", "pq", "xy"
        ];

        public override string Part1()
        {
            var input = GetInput();
            var niceCount = input.Count(IsNice);

            return niceCount.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();

            return "Part 2";
        }

        private static bool IsNice(string word)
        {
            char? prevChar = null;
            var vowelCount = 0;
            var hasDoubleLetter = false;
            var hasBannedSubString = false;

            foreach (var currentChar in word)
            {
                if (prevChar.HasValue)
                {
                    var pair = $"{prevChar}{currentChar}";

                    if (_bannedSubStrings.Contains(pair))
                    {
                        hasBannedSubString = true;
                        break;
                    }

                    if (prevChar == currentChar)
                        hasDoubleLetter = true;
                }

                if (_vowels.Contains(currentChar))
                    vowelCount++;

                prevChar = currentChar;
            }

            var isNice = !hasBannedSubString && vowelCount >= 3 && hasDoubleLetter;

            return isNice;
        }

        private string[] GetInput()
            => FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
    }
}
