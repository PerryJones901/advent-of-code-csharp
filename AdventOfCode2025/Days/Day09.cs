using AdventOfCodeCommon;
using AdventOfCodeCommon.Extensions;

namespace AdventOfCode2025.Days
{
    internal class Day09(bool isTest) : DayBase(9, isTest)
    {
        public override string Part1()
        {
            var input = GetInput();
            var coords = input.Select(line => 
                (int.Parse(line.Split(',')[0]), int.Parse(line.Split(',')[1])
            )).ToList();

            var maxArea = 0L;

            foreach (var (coords0, coords1) in coords.GetAllPairCombinations())
            {
                var area = GetAreaOfRectangle(coords0, coords1);

                if (area > maxArea)
                    maxArea = area;
            }

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
            var coords = input.Select(line =>
                (int.Parse(line.Split(',')[0]), int.Parse(line.Split(',')[1])
            )).ToList();

            var pairsOfPoints = coords.GetAllPairCombinations().OrderByDescending(pair =>
            {
                var (coords0, coords1) = pair;
                return GetAreaOfRectangle(coords0, coords1);
            }).ToList();

            var polygonEdges = coords
                .Select((point, index) => (point, coords[(index + 1) % coords.Count]))
                .ToList();

            foreach (var pair in pairsOfPoints)
            {
                var (rectPoint1, rectPoint2) = pair;
                bool edgesIntersect = polygonEdges.Any(edge =>
                    DoEdgesIntersect(rectPoint1, rectPoint2, edge.Item1, edge.Item2)
                );
                if (edgesIntersect)
                    continue;

                // Check that one of the corners is inside the polygon
                var testRayStart = (Math.Min(rectPoint1.Item1, rectPoint2.Item1) + 1, Math.Min(rectPoint1.Item2, rectPoint2.Item2) + 1);
                // I.e. test point is * in the diagram below:
                // +----------------+
                // |*               |
                // |                |
                // |                |
                // +----------------+
                var testRayEnd = (int.MaxValue, testRayStart.Item2);
                // I.e. the test ray is ~~~~> in the diagram below:
                // +----------------+
                // |*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~>
                // |                |
                // |                |
                // +----------------+
                if (polygonEdges.Count(edge =>
                        DoEdgesIntersect(testRayStart, testRayEnd, edge.Item1, edge.Item2)
                ) % 2 == 1)
                {
                    var area = GetAreaOfRectangle(rectPoint1, rectPoint2);
                    return area.ToString();
                }
            }

            throw new InvalidOperationException("No valid rectangle found");
        }

        private static long GetAreaOfRectangle((int, int) point1, (int, int) point2)
        {
            long xLength = Math.Abs(point2.Item1 - point1.Item1) + 1;
            long yLength = Math.Abs(point2.Item2 - point1.Item2) + 1;
            return xLength * yLength;
        }

        private static bool DoEdgesIntersect((int, int) rectPoint1, (int, int) rectPoint2, (int, int) edgePoint1, (int, int) edgePoint2)
        {
            int rectXMin = Math.Min(rectPoint1.Item1, rectPoint2.Item1);
            int rectXMax = Math.Max(rectPoint1.Item1, rectPoint2.Item1);
            int rectYMin = Math.Min(rectPoint1.Item2, rectPoint2.Item2);
            int rectYMax = Math.Max(rectPoint1.Item2, rectPoint2.Item2);

            // Assume that edges are axis-aligned
            if (edgePoint1.Item1 == edgePoint2.Item1)
            {
                // Vertical edge
                int x = edgePoint1.Item1;
                int yMin = Math.Min(edgePoint1.Item2, edgePoint2.Item2);
                int yMax = Math.Max(edgePoint1.Item2, edgePoint2.Item2);
                return (rectXMin < x && x < rectXMax && yMin < rectYMax && rectYMin < yMax);
            }

            // Horizontal edge
            int y = edgePoint1.Item2;
            int xMin = Math.Min(edgePoint1.Item1, edgePoint2.Item1);
            int xMax = Math.Max(edgePoint1.Item1, edgePoint2.Item1);

            return (rectYMin < y && y < rectYMax && xMin < rectXMax && rectXMin < xMax);
        }

        private string[] GetInput()
            => FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
    }
}
