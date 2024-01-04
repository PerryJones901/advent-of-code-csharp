using AdventOfCode2023.Helpers;

namespace AdventOfCode2023.Days;

public abstract class Day25
{
    public static int Part1Answer() =>
        Part1(GetSeparatedInputFromFile());

    public static int Part2Answer() =>
        Part2(GetSeparatedInputFromFile());

    public static int Part1(List<string> input)
    {
        var graphDict = new Dictionary<string, List<string>>();
        foreach (var item in input)
        {
            var split = item.Split(": ");
            var firstNode = split[0];
            if (!graphDict.ContainsKey(firstNode))
            {
                graphDict.Add(firstNode, new List<string>());
            }
            var connections = split[1].Split(' ');
            graphDict[split[0]].AddRange(connections);

            foreach (var connection in connections)
            {
                if (!graphDict.ContainsKey(connection))
                {
                    graphDict.Add(connection, new List<string>());
                }
                graphDict[connection].AddRange(connections);
            }
        }

        var orderedDict = graphDict
            .OrderBy(x => x.Value.Count)
            .ToDictionary(x => x.Key, x => x.Value);

        return 0;
    }

    public static int Part2(List<string> input)
    {
        return 0;
    }

    private static List<string> GetSeparatedInputFromFile() =>
        FileInputHelper.GetStringListFromFile("Day25.txt");
}
