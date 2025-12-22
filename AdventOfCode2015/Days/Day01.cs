using AdventOfCodeCommon;

namespace AdventOfCode2015.Days
{
    internal class Day01(bool isTest) : DayBase(1, isTest)
    {
        public override string Part1()
        {
            var input = GetInput();
            var openBracketCount = input.Count(c => c == '(');
            var closeBracketCount = input.Count(c => c == ')');

            var difference = openBracketCount - closeBracketCount;

            return difference.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();
            var difference = 0;

            foreach (var (c, index) in input.Select((c, i) => (c, i)))
            {
                difference += c == '(' ? 1 : -1;

                if (difference == -1)
                    return (index + 1).ToString();
            }

            throw new InvalidOperationException("Basement not reached");
        }

        private string GetInput()
            => FileInputAssistant.GetStringFromFile(TextInputFilePath);
    }
}
