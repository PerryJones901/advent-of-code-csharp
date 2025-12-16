using AdventOfCodeCommon;
using AdventOfCodeCommon.Extensions;

namespace AdventOfCode2025.Days
{
    internal class Day09(bool isTest) : DayBase(9, isTest)
    {
        public override string Part1()
        {
            var input = GetInput();
            var coords = GetCoords(input);

            var maxArea = coords
                .GetAllPairCombinations()
                .Select(pair => GetAreaOfRectangle(pair.Item1, pair.Item2))
                .Max();

            return maxArea.ToString();
        }

        public override string Part2()
        {
            // Explanation of this madness:
            // Step 1: Grab all possible pairs
            // Step 2: Order by the area of the rectangle they would form, descending
            // Step 3: Next, get list of edges of the polygon formed by the points
            // Step 4: Iterate through the ordered list of pairs, checking that no edge intersects the _interior_ of the rectangle formed
            // -- 'Interior' is important here. We don't mind if it brushes the edge of the rectangle, but do care if it properly crosses into it
            // Step 5: Check that the interior of the rectangle is 'inside' the polygon (could potentially be outside and therefore shouldn't count)
            // Step 6: Once we've found a valid rectangle, return its area

            var input = GetInput();
            var coords = GetCoords(input);

            var pairsOfPoints = coords
                .GetAllPairCombinations()
                .OrderByDescending(pair => GetAreaOfRectangle(pair.Item1, pair.Item2));

            var polygonEdges = GetEdges(coords);

            foreach (var pair in pairsOfPoints)
            {
                var (firstRectanglePoint, secondRectanglePoint) = pair;
                bool edgesIntersect = polygonEdges.Any(edge =>
                    DoEdgesIntersect(
                        firstRectanglePoint, 
                        secondRectanglePoint,
                        edge.First, 
                        edge.Second)
                );

                if (edgesIntersect)
                    continue;

                // To check 'insideness', take the top-left corner of the rectangle's interior and cast a ray to the right
                // I.e. the ray is *~~~>* in the diagram below:
                // +----------------+
                // |*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~>*
                // |                |
                // |                |
                // +----------------+

                var rayStartPoint = (
                    Math.Min(firstRectanglePoint.Item1, secondRectanglePoint.Item1) + 1, 
                    Math.Min(firstRectanglePoint.Item2, secondRectanglePoint.Item2) + 1);

                var rayEndPoint = (int.MaxValue, rayStartPoint.Item2);
                var rayIntersectCount = polygonEdges
                        .Count(edge =>
                            DoEdgesIntersect(
                                rayStartPoint,
                                rayEndPoint,
                                edge.First,
                                edge.Second));

                var isInside = rayIntersectCount % 2 == 1;

                if (isInside)
                {
                    var area = GetAreaOfRectangle(firstRectanglePoint, secondRectanglePoint);
                    return area.ToString();
                }
            }

            throw new InvalidOperationException("No valid rectangle found");
        }

        private static IList<(int, int)> GetCoords(string[] input)
            => [.. input.Select(line =>
                (
                    int.Parse(line.Split(',')[0]),
                    int.Parse(line.Split(',')[1])
                )
            )];

        private static IList<((int, int) First, (int, int) Second)> GetEdges(IList<(int, int)> coords)
            => [.. coords
                .Zip(
                    coords
                    .Skip(1)
                    .Concat(coords.Take(1)))
            ];

        private static long GetAreaOfRectangle((int, int) point1, (int, int) point2)
        {
            long xLength = Math.Abs(point2.Item1 - point1.Item1) + 1;
            long yLength = Math.Abs(point2.Item2 - point1.Item2) + 1;
            return xLength * yLength;
        }

        private static bool DoEdgesIntersect(
            (int, int) firstRectanglePoint,
            (int, int) secondRectanglePoint,
            (int, int) firstEdgePoint,
            (int, int) secondEdgePoint)
        {
            int rectXMin = Math.Min(firstRectanglePoint.Item1, secondRectanglePoint.Item1);
            int rectXMax = Math.Max(firstRectanglePoint.Item1, secondRectanglePoint.Item1);
            int rectYMin = Math.Min(firstRectanglePoint.Item2, secondRectanglePoint.Item2);
            int rectYMax = Math.Max(firstRectanglePoint.Item2, secondRectanglePoint.Item2);

            // Assume that edges are axis-aligned
            if (firstEdgePoint.Item1 == secondEdgePoint.Item1)
            {
                // Vertical edge
                int x = firstEdgePoint.Item1;
                int yMin = Math.Min(firstEdgePoint.Item2, secondEdgePoint.Item2);
                int yMax = Math.Max(firstEdgePoint.Item2, secondEdgePoint.Item2);
                return (rectXMin < x && x < rectXMax && yMin < rectYMax && rectYMin < yMax);
            }

            // Horizontal edge
            int y = firstEdgePoint.Item2;
            int xMin = Math.Min(firstEdgePoint.Item1, secondEdgePoint.Item1);
            int xMax = Math.Max(firstEdgePoint.Item1, secondEdgePoint.Item1);

            return (rectYMin < y && y < rectYMax && xMin < rectXMax && rectXMin < xMax);
        }

        private string[] GetInput()
            => FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
    }
}
