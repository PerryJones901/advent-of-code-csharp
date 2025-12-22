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

            var lowerBound = 0;
            var upperBound = 0;

            foreach (var regionPuzzle in regionPuzzles)
            {
                var lowestMult3Width = (regionPuzzle.Width / 3) * 3;
                var lowestMult3Height = (regionPuzzle.Height / 3) * 3;

                var areaCoveredBy3x3Shapes = lowestMult3Width * lowestMult3Height;
                var maxLazy3x3Shapes = areaCoveredBy3x3Shapes / 9;

                var totalShapesInPuzzle = regionPuzzle.ShapeCounts.Sum();
                if (totalShapesInPuzzle <= maxLazy3x3Shapes)
                    lowerBound++;

                var totalSquaresCoveredByShapesInPuzzle = regionPuzzle.ShapeCounts
                    .Select((count, index) => count * shapes[index].Sum(x => x.Count(y => y == '#')))
                    .Sum();

                if (totalSquaresCoveredByShapesInPuzzle <= regionPuzzle.Width * regionPuzzle.Height)
                    upperBound++;
            }

            var output = $"Lower bound: {lowerBound}. Upper bound: {upperBound}.";

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
