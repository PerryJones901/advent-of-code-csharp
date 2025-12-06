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

            var validIngredientCount = ingredientIds
                .Count(id => ranges.Any(r => r.Start <= id && id <= r.End));

            return validIngredientCount.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();
            var ranges = GetRanges(input[0]);
            var edges = GetEdges(ranges);

            var mergedRanges = new List<Range>();
            var startEdgeStackCount = 0;
            long? currentStartPosition = null;

            foreach (var edge in edges)
            {
                if (startEdgeStackCount == 0)
                    currentStartPosition = edge.Position;

                startEdgeStackCount += edge.IsStart ? 1 : -1;

                if (startEdgeStackCount == 0)
                {
                    if (!currentStartPosition.HasValue)
                        throw new Exception("Invalid state");

                    var range = new Range(currentStartPosition.Value, edge.Position);
                    mergedRanges.Add(range);
                    currentStartPosition = null;
                }
            }

            var totalFreshIdCount = mergedRanges.Sum(x => x.End - x.Start + 1);

            return totalFreshIdCount.ToString();
        }

        private static IEnumerable<Range> GetRanges(string rangesInput)
            => rangesInput
                .Split(NEW_LINE_SEPARATOR)
                .Select(line => 
                    new Range(
                        long.Parse(line.Split('-')[0]),
                        long.Parse(line.Split('-')[1])
                    )
                ).OrderBy(x => x.Start);

        private static IEnumerable<long> GetIngredientIds(string ingredientsInput)
            => ingredientsInput
                .Split(NEW_LINE_SEPARATOR)
                .Select(long.Parse);

        private static IEnumerable<Edge> GetEdges(IEnumerable<Range> ranges)
            => ranges.Select(ranges => new Edge(ranges.Start, isStart: true))
                .Concat(ranges.Select(ranges => new Edge(ranges.End, isStart: false)))
                .OrderBy(edge => edge.Position)
                .ThenBy(edge => edge.IsStart ? 0 : 1);

        private string[] GetInput()
            => FileInputAssistant.GetStringArrayFromFile(TextInputFilePath, $"{NEW_LINE_SEPARATOR}{NEW_LINE_SEPARATOR}");

        [DebuggerDisplay("Start = {Start}, End = {End}")]
        private class Range(long start, long end)
        {
            public long Start { get; } = start;
            public long End { get; } = end;
        }

        [DebuggerDisplay("Position = {Position}, IsStart = {IsStart}")]
        private class Edge(long position, bool isStart)
        {
            public long Position { get; } = position;
            public bool IsStart { get; } = isStart;
        }
    }
}
