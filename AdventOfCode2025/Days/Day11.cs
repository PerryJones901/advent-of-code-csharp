using AdventOfCodeCommon;

namespace AdventOfCode2025.Days
{
    internal class Day11(bool isTest) : DayBase(11, isTest)
    {
        private const string END_NODE = "out";

        public override string Part1()
        {
            var input = GetInput();
            var nodes = GetNodes(input);
            var nodePathCounts = new Dictionary<string, long> { { END_NODE, 1 } };

            var pathCount = GetPathCountPart1(
                "you",
                nodes,
                nodePathCounts);

            return pathCount.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();
            var nodes = GetNodes(input);
            var nodePathCounts = new Dictionary<(string, bool, bool), long> {
                { (END_NODE, false, false), 0 },
                { (END_NODE, false,  true), 0 },
                { (END_NODE,  true, false), 0 },
                { (END_NODE,  true,  true), 1 }
            };

            var pathCount = GetPathCountPart2(
                "svr",
                nodes,
                nodePathCounts,
                hasVisitedDac: false,
                hasVisitedFft: false);

            return pathCount.ToString();
        }

        private static long GetPathCountPart1(string currentNode, Dictionary<string, List<string>> nodes, Dictionary<string, long> nodePathCounts)
        {
            if (nodePathCounts.TryGetValue(currentNode, out long value))
                return value;

            var pathCount = nodes[currentNode].Sum(nextNode => GetPathCountPart1(nextNode, nodes, nodePathCounts));
            nodePathCounts.Add(currentNode, pathCount);

            return pathCount;
        }

        private static long GetPathCountPart2(
            string currentNode,
            Dictionary<string, List<string>> nodes,
            Dictionary<(string, bool, bool), long> nodePathCounts,
            bool hasVisitedDac,
            bool hasVisitedFft)
        {
            var newHasVisitedDac = hasVisitedDac || currentNode == "dac";
            var newHasVisitedFft = hasVisitedFft || currentNode == "fft";

            if (nodePathCounts.TryGetValue((currentNode, newHasVisitedDac, newHasVisitedFft), out long value))
                return value;

            var pathCount = nodes[currentNode].Sum(nextNode => GetPathCountPart2(nextNode, nodes, nodePathCounts, newHasVisitedDac, newHasVisitedFft));
            nodePathCounts.Add((currentNode, newHasVisitedDac, newHasVisitedFft), pathCount);

            return pathCount;
        }

        private static Dictionary<string, List<string>> GetNodes(string[] input)
        {
            var nodes = new Dictionary<string, List<string>>();

            foreach (var nodeInfo in input)
            {
                var parts = nodeInfo.Split(": ");
                var nodeName = parts[0];
                var outputs = parts[1].Split(" ").ToList();

                nodes[nodeName] = outputs;
            }

            return nodes;
        }

        private string[] GetInput()
            => FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
    }
}
