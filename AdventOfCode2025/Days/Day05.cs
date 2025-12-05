using AdventOfCodeCommon;
using System.Diagnostics;

namespace AdventOfCode2025.Days
{
    internal class Day05(bool isTest) : DayBase(5, isTest)
    {
        private const string NEW_LINE_SEPARATOR = "\r\n";
        public override string Part1()
        {
            var input = GetInput();
            var ranges = GetRanges(input[0]);
            var ingredientIds = GetIngredientIds(input[1]);

            var validIngredientCount = 0;

            foreach(var ingredientId in ingredientIds)
            {
                foreach (var range in ranges)
                {
                    if (range.Start <= ingredientId && ingredientId <= range.End)
                    {
                        validIngredientCount++;
                        break;
                    }
                }
            }

            return validIngredientCount.ToString();
        }

        public override string Part2()
        {
            return "Part 2";
        }

        private static IEnumerable<Range> GetRanges(string rangesInput)
            => rangesInput
                .Split(NEW_LINE_SEPARATOR)
                .Select(line =>
                {
                    var parts = line.Split('-');
                    return new Range(long.Parse(parts[0]), long.Parse(parts[1]));
                });

        private static IEnumerable<long> GetIngredientIds(string ingredientsInput)
            => ingredientsInput
                .Split(NEW_LINE_SEPARATOR)
                .Select(long.Parse);

        private static bool AreRangesSeparate(Range range1, Range range2)
            => range1.End < range2.Start || range2.End < range1.Start;

        private string[] GetInput()
            => FileInputAssistant.GetStringArrayFromFile(TextInputFilePath, $"{NEW_LINE_SEPARATOR}{NEW_LINE_SEPARATOR}");

        [DebuggerDisplay("Start = {Start}, End = {End}")]
        private class Range(long start, long end)
        {
            public long Start { get; } = start;
            public long End { get; } = end;
        }
    }
}
