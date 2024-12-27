using AdventOfCodeCommon;

namespace AdventOfCode2024.Days
{
    internal class Day23(bool isTest) : DayBase(23, isTest)
    {
        public override string Part1()
        {
            var input = GetInput();
            var nodes = GetNodes(input);
            var tripletStrings = new HashSet<string>();

            foreach (var node in nodes)
            {
                foreach (var neighbour in node.Value)
                {
                    foreach (var neighbourNeighbour in nodes[neighbour])
                    {
                        if (!node.Value.Contains(neighbourNeighbour))
                            continue;

                        var threeNodes = new List<string>([node.Key, neighbour, neighbourNeighbour]);

                        if (threeNodes.All(x => !x.StartsWith('t')))
                            continue;

                        threeNodes = [.. threeNodes.OrderBy(x => x)];

                        tripletStrings.Add(string.Join(",", threeNodes));
                    }
                }
            }

            return tripletStrings.Count.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();
            var nodes = GetNodes(input);
            var maximalCliques = new List<HashSet<string>>();
            BronKerbosch(
                [],
                [.. nodes.Keys],
                [],
                nodes,
                maximalCliques
            );

            // get max
            var maxClique = maximalCliques.MaxBy(x => x.Count);
            var password = string.Join(",", maxClique.OrderBy(x => x));

            return password;
        }

        /// <summary>
        /// Using the Bron-Kerbosch Algorithm to find maximal cliques.
        /// Reference: https://en.wikipedia.org/wiki/Bron%E2%80%93Kerbosch_algorithm
        /// </summary>
        /// <param name="r">Set where all vertices will be included in clique</param>
        /// <param name="p">Set where some vertices will be included in clique</param>
        /// <param name="x">Set where NO vertices will be included in clique</param>
        /// <param name="nodes">Node name to neighbours dictionary</param>
        /// <param name="maximalCliques">List of all maximal cliques (will be populated as the algorithm runs)</param>
        private static void BronKerbosch(
            List<string> r,
            List<string> p,
            List<string> x,
            Dictionary<string, HashSet<string>> nodes,
            List<HashSet<string>> maximalCliques)
        {
            if (p.Count == 0 && x.Count == 0)
            {
                maximalCliques.Add([..r]);
                return;
            }

            // Choose pivot in P union X
            var pivot = x.Count > 0 ? x[0] : p[0];

            var list = p.ToList();
            list.RemoveAll(x => nodes[pivot].Contains(x));

            foreach (var node in list)
            {
                BronKerbosch(
                    [.. r, node],
                    [.. p.Intersect(nodes[node])],
                    [.. x.Intersect(nodes[node])],
                    nodes,
                    maximalCliques);
                p.Remove(node);
                x.Add(node);
            }
        }

        private static Dictionary<string, HashSet<string>> GetNodes(string[] input)
        {
            var nodes = new Dictionary<string, HashSet<string>>();

            foreach (var inputLine in input)
            {
                var split = inputLine.Split("-");
                var leftNode = split[0];
                var rightNode = split[1];

                if (!nodes.ContainsKey(leftNode))
                    nodes.Add(leftNode, []);

                if (!nodes.ContainsKey(rightNode))
                    nodes.Add(rightNode, []);

                nodes[leftNode].Add(rightNode);
                nodes[rightNode].Add(leftNode);
            }

            return nodes;
        }

        private string[] GetInput()
        {
            return FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
        }
    }
}
