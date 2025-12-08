using AdventOfCodeCommon;
using AdventOfCodeCommon.Vectors;
using System.Diagnostics;

namespace AdventOfCode2025.Days
{
    internal class Day08(bool isTest) : DayBase(8, isTest)
    {
        public override string Part1()
        {
            var input = GetInput();
            var nodes = GetNodesFromInput(input);
            var pairsToDistances = new Dictionary<(Node, Node), double>();
            for (int firstIndex = 0; firstIndex < nodes.Count - 1; firstIndex++)
            {
                for (int secondIndex = firstIndex + 1; secondIndex < nodes.Count; secondIndex++)
                {
                    var firstNode = nodes[firstIndex];
                    var secondNode = nodes[secondIndex];
                    var distance = firstNode.Position.DistanceTo(secondNode.Position);
                    pairsToDistances[(firstNode, secondNode)] = distance;
                }
            }

            var pairsOrderedByDistance = pairsToDistances
                .OrderBy(x => x.Value)
                .Take(1000)
                .ToList();

            foreach (var pair in pairsOrderedByDistance)
            {
                var firstNode = pair.Key.Item1;
                var secondNode = pair.Key.Item2;

                firstNode.Neighbors.Add(secondNode);
                secondNode.Neighbors.Add(firstNode);
            }

            var nodeToConnectedCompId = new Dictionary<Node, int>();
            var currentCompId = 0;
            foreach (var node in nodes)
            {
                if (nodeToConnectedCompId.ContainsKey(node))
                {
                    continue;
                }

                var nodeQueue = new Queue<Node>([node]);

                while (nodeQueue.Count > 0)
                {
                    var currentNode = nodeQueue.Dequeue();
                    if (nodeToConnectedCompId.ContainsKey(currentNode))
                    {
                        continue;
                    }
                    nodeToConnectedCompId[currentNode] = currentCompId;
                    foreach (var neighbor in currentNode.Neighbors)
                    {
                        if (!nodeToConnectedCompId.ContainsKey(neighbor))
                        {
                            nodeQueue.Enqueue(neighbor);
                        }
                    }
                }

                currentCompId++;
            }

            var threeLargestConnectedCompCounts = nodes
                .GroupBy(x => nodeToConnectedCompId[x])
                .Select(x => x.Count())
                .OrderDescending()
                .Take(3);

            var answer = threeLargestConnectedCompCounts.Aggregate(1, (a, b) => a * b);

            return answer.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();
            var nodes = GetNodesFromInput(input);
            var pairsToDistances = new Dictionary<(Node, Node), double>();
            for (int firstIndex = 0; firstIndex < nodes.Count - 1; firstIndex++)
            {
                for (int secondIndex = firstIndex + 1; secondIndex < nodes.Count; secondIndex++)
                {
                    var node1 = nodes[firstIndex];
                    var node2 = nodes[secondIndex];
                    var distance = node1.Position.DistanceTo(node2.Position);
                    pairsToDistances[(node1, node2)] = distance;
                }
            }

            var pairsOrderedByDistance = pairsToDistances
                .OrderBy(x => x.Value)
                .Select(x => x.Key)
                .ToList();

            var nodeToConnectedCompId = new Dictionary<Node, int>();
            for (int index = 0; index < nodes.Count; index++)
            {
                nodeToConnectedCompId[nodes[index]] = index;
            }

            Node? firstNode = null, secondNode = null;

            foreach (var pair in pairsOrderedByDistance)
            {
                firstNode = pair.Item1;
                secondNode = pair.Item2;

                var firstCompId = nodeToConnectedCompId[firstNode];
                var secondCompId = nodeToConnectedCompId[secondNode];

                if (firstCompId == secondCompId)
                    continue;

                var mergedCompId = Math.Min(firstCompId, secondCompId);
                int compIdToChange = Math.Max(firstCompId, secondCompId);

                var nodesInConnectedCompToChange = nodeToConnectedCompId
                    .Where(x => x.Value == compIdToChange)
                    .Select(x => x.Key)
                    .ToList();

                foreach (var node in nodesInConnectedCompToChange)
                {
                    nodeToConnectedCompId[node] = mergedCompId;
                }

                if (nodeToConnectedCompId.Values.Distinct().Count() == 1)
                    break;
            }

            if (firstNode is null || secondNode is null)
                throw new Exception("No nodes found");

            var answer = firstNode.Position.X * secondNode.Position.X;

            return answer.ToString();
        }

        private static List<Node> GetNodesFromInput(int[][] input)
        {
            var nodes = input.Select(
                x => new Node
                {
                    Position = new Vector3
                    {
                        X = x[0],
                        Y = x[1],
                        Z = x[2],
                    }
                }).ToList();

            return nodes;
        }

        [DebuggerDisplay("Position = {Position}, NeighbourCount = {NeighbourCount}")]
        private class Node
        {
            public required Vector3 Position { get; set; }
            public HashSet<Node> Neighbors { get; set; } = [];
            public int NeighbourCount => Neighbors.Count;
        }

        private int[][] GetInput()
            => FileInputAssistant.GetParamListsFromRegexFromFile(
                TextInputFilePath,
                @"(\d+),(\d+),(\d+)"
            ).Select(x => x.Select(int.Parse).ToArray()).ToArray();
    }
}
