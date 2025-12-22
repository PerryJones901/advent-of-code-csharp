using AdventOfCodeCommon;

namespace AdventOfCode2025.Days
{
    internal class Day12(bool isTest) : DayBase(12, isTest)
    {
        private const string NEW_LINE_SEPARATOR = "\r\n";

        public override string Part1()
        {
            var input = GetInput();
            var shapes = GetShapes(input);
            var regionPuzzles = GetRegionPuzzles(input);

            var lowerBoundRegionCount = 0;
            var upperBoundRegionCount = 0;

            foreach (var regionPuzzle in regionPuzzles)
            {
                var highestMultipleOf3Width = (regionPuzzle.Width / 3) * 3;
                var highestMultipleOf3Height = (regionPuzzle.Height / 3) * 3;

                var areaCoveredBy3x3Squares = highestMultipleOf3Width * highestMultipleOf3Height;
                var total3x3SquareCount = areaCoveredBy3x3Squares / (3*3);

                if (regionPuzzle.ShapeCounts.Sum() <= total3x3SquareCount)
                    lowerBoundRegionCount++;

                var totalCellsCoveredByShapesInPuzzle = regionPuzzle.ShapeCounts
                    .Select((count, index) => count * shapes[index].Sum(x => x.Count(y => y == '#')))
                    .Sum();

                if (totalCellsCoveredByShapesInPuzzle <= regionPuzzle.Width * regionPuzzle.Height)
                    upperBoundRegionCount++;
            }

            var output = $"Lower bound: {lowerBoundRegionCount}. Upper bound: {upperBoundRegionCount}.";

            return output.ToString();
        }

        public override string Part2() => "Merry Christmas!";

        private string[][] GetShapes(string[] shapeInputs)
            => shapeInputs
                .SkipLast(1)
                .Select(lines => lines
                    .Split(NEW_LINE_SEPARATOR)
                    .Skip(1)
                    .ToArray())
                .ToArray();

        private IEnumerable<RegionPuzzle> GetRegionPuzzles(string[] input)
            => input
                .Last()
                .Split(NEW_LINE_SEPARATOR)
                .Select(line => new RegionPuzzle
                {
                    Width = int.Parse(line.Split(": ")[0].Split('x')[0]),
                    Height = int.Parse(line.Split(": ")[0].Split('x')[1]),
                    ShapeCounts = line
                        .Split(": ")[1]
                        .Split(' ')
                        .Select(int.Parse),
                });

        private class RegionPuzzle
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public IEnumerable<int> ShapeCounts { get; set; } = [];
        }

        private string[] GetInput()
            => FileInputAssistant.GetStringArrayFromFile(
                TextInputFilePath, 
                $"{NEW_LINE_SEPARATOR}{NEW_LINE_SEPARATOR}");
    }
}
