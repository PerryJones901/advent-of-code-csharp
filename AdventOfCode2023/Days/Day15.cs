using AdventOfCode2023.Helpers;

namespace AdventOfCode2023.Days;

public abstract class Day15
{
    public static int Part1Answer() =>
        Part1(GetInputFromFile());

    public static int Part2Answer() =>
        Part2(GetInputFromFile());

    public static int Part1(string input)
    {
        var inputs = input.Split(",").ToList();
        var totalHashes = inputs.Select(HashCode).Sum();

        return totalHashes;
    }

    private static int HashCode(string input)
    {
        var currentValue = 0;
        foreach (char c in input)
        {
            currentValue += (int)c;
            currentValue = (currentValue * 17) % 256;
        }

        return currentValue;
    }

    public static int Part2(string input)
    {
        // 210423 - not correct
        var inputs = input.Split(",").ToList();
        var totalHashes = inputs.Select(x => x.Split('=', '-')[0]).Select(HashCode).ToList();

        var items = inputs.Zip(totalHashes, (a, b) => new { Label = a, Hash = b }).ToList();

        var boxes = new Dictionary<int, List<string>>();

        foreach(var item in items)
        {
            var lastChar = item.Label.Last();

            if (lastChar == '-')
            {
                if (!boxes.ContainsKey(item.Hash)) continue;

                var indexOfMatch = boxes[item.Hash].FindIndex(
                    x => new string (x.SkipLast(2).ToArray()) == new string (item.Label.SkipLast(1).ToArray()));

                if (indexOfMatch != -1)
                    boxes[item.Hash].RemoveAt(indexOfMatch);
            }
            else
            {
                if (!boxes.ContainsKey(item.Hash))
                    boxes.Add(item.Hash, new List<string>());

                var indexOfMatch = boxes[item.Hash].FindIndex(
                    x => new string(x.SkipLast(2).ToArray()) == new string(item.Label.SkipLast(2).ToArray()));
                if (indexOfMatch >= 0)
                {
                    boxes[item.Hash].RemoveAt(indexOfMatch);
                    boxes[item.Hash].Insert(indexOfMatch, item.Label);
                } else
                {
                    boxes[item.Hash].Add(item.Label);
                }
            }
        }

        return GetPower(boxes);
    }

    private static int GetPower(Dictionary<int, List<string>> boxes)
    {
        return boxes.Select(
            x => (x.Key + 1)
                * x.Value.Select((y, slotIndex) => 
                    (slotIndex + 1) 
                    * int.Parse(y.Last().ToString()))
                .Sum())
            .Sum();
    }

    private static string GetInputFromFile() =>
        FileInputHelper.GetStringFromFile("Day15.txt");
}
