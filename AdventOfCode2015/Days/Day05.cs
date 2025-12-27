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

        public override string Part1() => GetInput().Count(IsNicePart1).ToString();

        public override string Part2() => GetInput().Count(IsNicePart2).ToString();

        private static bool IsNicePart1(string word)
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

        private static bool IsNicePart2(string word)
        {
            var hasRepeatingPair = false;
            var hasRepeatingLetterWithOneBetween = false;

            for (var charIndex = 0; charIndex < word.Length - 2; charIndex++)
            {
                var pair = word.Substring(charIndex, 2);
                var restOfString = word[(charIndex + 2)..];

                if (restOfString.Contains(pair))
                    hasRepeatingPair = true;

                var currentChar = word[charIndex];
                var currentCharTwoAhead = word[charIndex + 2];

                if (currentChar == currentCharTwoAhead)
                    hasRepeatingLetterWithOneBetween = true;
            }

            var isNice = hasRepeatingPair && hasRepeatingLetterWithOneBetween;

            return isNice;
        }

        private string[] GetInput()
            => FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
    }
}
