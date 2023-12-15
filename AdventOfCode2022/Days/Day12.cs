using AdventOfCode2022.Helpers;

namespace AdventOfCode2022.Days
{
    public static class Day12
    {
        public static int Part1Answer() =>
            Part1(GetSeparatedInputFromFile());

        public static int Part2Answer() =>
            Part2(GetSeparatedInputFromFile());

        public static int Part1(List<string> input)
        {
            var heatMap = GetHeatMap(input);
            var vertexList = new List<Node>(heatMap.SelectMany(x => x).ToList());

            while(true)
            {
                vertexList = vertexList?.OrderBy(x => x.DistanceFromSource).ToList();
                if (vertexList == null) throw new Exception("Vertex list is null");

                var currentNode = vertexList?.FirstOrDefault();
                if (currentNode == null) break;
                if (currentNode.IsTarget) return currentNode.DistanceFromSource;
                vertexList?.Remove(currentNode);

                foreach (var neighbour in currentNode.Neighbours)
                {
                    if (!vertexList?.Contains(neighbour) ?? true)
                        continue;

                    var alt = currentNode.DistanceFromSource == int.MaxValue 
                        ? int.MaxValue 
                        : currentNode.DistanceFromSource + 1;

                    if (alt < neighbour.DistanceFromSource)
                    {
                        neighbour.DistanceFromSource = alt;
                        neighbour.Previous = currentNode;
                    }
                }
            }
            return 0;
        }

        public static int Part2(List<string> input)
        {
            var adjustedInput = input.Select(x => x.Replace('S', 'a')).ToList();
            var numOfAs = adjustedInput.Sum(x => x.Count(y => y == 'a'));
            var minDistance = int.MaxValue;

            for(int i = 0; i < numOfAs; i++)
            {
                var heatMap = GetHeatMap(adjustedInput);
                var vertexList = new List<Node>(heatMap.SelectMany(x => x).ToList());

                MarkStart(heatMap, i);

                while (true)
                {
                    vertexList = vertexList?.OrderBy(x => x.DistanceFromSource).ToList();
                    if (vertexList == null) throw new Exception("Vertex list is null");

                    var currentNode = vertexList?.FirstOrDefault();
                    if (currentNode == null) break;
                    if (currentNode.IsTarget)
                    {
                        if (currentNode.DistanceFromSource < minDistance)
                            minDistance = currentNode.DistanceFromSource;
                        break;
                    }
                    vertexList?.Remove(currentNode);

                    foreach (var neighbour in currentNode.Neighbours)
                    {
                        if (!vertexList?.Contains(neighbour) ?? true)
                            continue;

                        var alt = currentNode.DistanceFromSource == int.MaxValue
                            ? int.MaxValue
                            : currentNode.DistanceFromSource + 1;

                        if (alt < neighbour.DistanceFromSource)
                        {
                            neighbour.DistanceFromSource = alt;
                            neighbour.Previous = currentNode;
                        }
                    }
                }
            }
            return minDistance;
        }

        private static Node[][] GetHeatMap(List<string> input)
        {
            var hello = input.Select(
                x => x.Select(y => new Node
                {
                    Height = GetHeight(y),
                    DistanceFromSource = (y == 'S' ? 0 : int.MaxValue),
                    IsTarget = (y == 'E'),
                }).ToArray()
            ).ToArray();

            SetNeighbours(hello);
            return hello;
        }

        private static void SetNeighbours(Node[][] nodes)
        {
            var DIRECTIONS = new List<(int, int)> { (1, 0), (0, 1), (-1, 0), (0, -1) };
            var xHeight = nodes.Length;
            var yLength = nodes.First().Length;

            for (int i = 0; i < xHeight; i++)
            {
                for(int j = 0; j < yLength; j++)
                {
                    var currentNode = nodes[i][j];
                    foreach(var vector in DIRECTIONS)
                    {
                        var neighbourSquare = (i + vector.Item1, j + vector.Item2);
                        if (IsInBounds(neighbourSquare, xHeight, yLength))
                        {
                            var neighbour = nodes[neighbourSquare.Item1][neighbourSquare.Item2];
                            if (neighbour.Height <= currentNode.Height + 1)
                                currentNode.Neighbours.Add(neighbour);
                        }
                    }
                }
            }
        }

        private static void MarkStart(Node[][] nodes, int nthAToUse)
        {
            var aCount = 0;
            foreach(var row in nodes)
            {
                foreach (var node in row)
                {
                    if (node.Height == 0)
                        aCount++;
                    if (nthAToUse == aCount - 1)
                        node.DistanceFromSource = 0;
                    break;
                }
                if (nthAToUse <= aCount - 1) break;
            }
        }

        private static bool IsInBounds((int, int) space, int xHeight, int yLength) =>
            0 <= space.Item1 
            && space.Item1 < xHeight 
            && 0 <= space.Item2 
            && space.Item2 < yLength;

        private static int GetHeight(char c)
        {
            return c switch
            {
                'S' => 'a',
                'E' => 'z',
                _ => c,
            } - 'a';
        }

        private static List<string> GetSeparatedInputFromFile() => 
            FileInputHelper.GetStringListFromFile("Day12.txt");
    }

    public class Node
    {
        public int Height { get; set; } = 0;
        public int DistanceFromSource { get; set; } = int.MaxValue;
        public Node? Previous { get; set; } = null;
        public bool IsTarget { get; set; } = false;
        public List<Node> Neighbours { get; set; } = new List<Node>();
    }
}
