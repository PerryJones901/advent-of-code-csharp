using AdventOfCode2022.Helpers;

namespace AdventOfCode2022.Days
{
    public static class Day13
    {
        private const string LINE_SEPARATOR = "\r\n";

        public static int Part1Answer() =>
            Part1(GetSeparatedInputFromFile(isPart1: true));

        public static int Part2Answer() =>
            Part2(GetSeparatedInputFromFile(isPart1: false));

        public static int Part1(List<string> input)
        {
            return GetNodePairs(input)
                .Select((nodePairs, index) => new { nodePairs, index = index + 1 })
                .Where(x => CompareNodes(x.nodePairs.Item1, x.nodePairs.Item2) == -1)
                .Sum(x => x.index);
        }

        public static int Part2(List<string> input)
        {
            var hello = input
                .Where(x => x != "")
                .Select(GetNodeFromString)
                .ToList();
            var divider1 = GetNodeFromString("[[2]]");
            var divider2 = GetNodeFromString("[[6]]");
            hello.Add(divider1);
            hello.Add(divider2);

            hello.Sort(CompareNodes);
            return (hello.IndexOf(divider1) + 1) * (hello.IndexOf(divider2) + 1);
        }

        private static List<(Node, Node)> GetNodePairs(List<string> input)
        {
            return input.Select(x =>
                (
                    GetNodeFromString(x.Split(LINE_SEPARATOR)[0]),
                    GetNodeFromString(x.Split(LINE_SEPARATOR)[1])
                )
            ).ToList();
        }

        private static Node GetNodeFromString(string input)
        {
            // After looking at input, I can assume no line is simply a number
            
            Node? currentNode = null;
            var currentNumberString = "";
            Node? rootNode = null;
            foreach(var c in input)
            {
                if (c == '[')
                {
                    var newNode = new Node();
                    newNode.Parent = currentNode;
                    currentNode = newNode;

                    if (rootNode == null) rootNode = currentNode;
                }
                else if (c == ']')
                {
                    if (currentNumberString != "")
                    {
                        currentNode?.Nodes.Add(
                            new Node { Number = int.Parse(currentNumberString) }
                        );
                        currentNumberString = "";
                    }

                    currentNode?.Parent?.Nodes.Add(currentNode);
                    currentNode = currentNode?.Parent;
                }
                else if (c == ',')
                {
                    if (currentNumberString == "") continue;

                    currentNode?.Nodes.Add(
                        new Node { Number = int.Parse(currentNumberString) }
                    );
                    currentNumberString = "";
                }
                else
                {
                    // Should be a number now
                    currentNumberString += c.ToString();
                }
            }
            return rootNode;
        }

        private class Tracker
        {
            public bool? IsInRightOrder = null;
        }

        private static int CompareNodes(Node node0, Node node1)
        {
            // Case 1: Both ints
            if (node0.Number != null && node1.Number != null)
                return node0.Number.Value.CompareTo(node1.Number.Value);

            // Case 2: Left is a number, right is list
            if (node0.Number != null)
            {
                return CompareNodes(
                    new Node { Nodes = new List<Node>() { node0 } },
                    node1
                );
            }

            // Case 3: Left is list, right is int
            if (node1.Number != null)
            {
                return CompareNodes(
                    node0,
                    new Node { Nodes = new List<Node>() { node1 } }
                );
            }

            // Case 4: Both lists
            var pairsOfNodes = node0.Nodes.Zip(node1.Nodes, (first, second) => (first, second));
            foreach (var pair in pairsOfNodes)
            {
                var compare = CompareNodes(pair.first, pair.second);
                if (compare != 0) return compare;
            }

            // Case 4a: Goes to count comparison
            return node0.Nodes.Count.CompareTo(node1.Nodes.Count);
        }

        private static List<string> GetSeparatedInputFromFile(bool isPart1) => 
            FileInputHelper.GetStringListFromFile("Day13.txt", isPart1 ? $"{LINE_SEPARATOR}{LINE_SEPARATOR}" : LINE_SEPARATOR);

        public class Node
        {
            // Assume if a Node has non-null Number, it is a number
            public int? Number { get; set; }
            public List<Node> Nodes { get; set; } = new List<Node>();
            public Node? Parent { get; set; } = null;
        }
    }
}
