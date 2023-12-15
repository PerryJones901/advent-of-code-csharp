using AdventOfCode2023.Helpers;

namespace AdventOfCode2023.Days;

public abstract class Day12
{
    public static int Part1Answer() =>
        Part1(GetSeparatedInputFromFile());

    public static int Part2Answer() =>
        Part2(GetSeparatedInputFromFile());

    public static int Part1(List<string> input)
    {
        // 7993 -- too high
        var sum = 0;
        foreach(var record in input)
        {
            var springAmounts = record.Split(' ')[1].Split(',').Select(int.Parse).ToList();

            sum += NumWaysForArrangement(record.Split(' ')[0], springAmounts);
        }
        return sum;
    }

    private static int NumWaysForArrangement(string record, List<int> springAmounts)
    {
        // Trim any '.'
        record = record.Trim('.');

        // Short circuits
        if (springAmounts.Count == 0 && record.Length == 0) return 1; // Reached end
        var minSpaceRequired = springAmounts.Sum() + springAmounts.Count - 1; // Count - 1 is the total space between the spring groups
        if (record.Length < minSpaceRequired) return 0;

        var brokenCount = record.Count(x => x == '#');
        if (brokenCount > springAmounts.Sum()) return 0; // Too many existing broken parts

        else if (brokenCount == springAmounts.Sum())
        {
            // Check if the broken parts match up to the springAmounts array
            var hello = record.Split('?', '.').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Length);
            var zip = hello.Zip(springAmounts);
            return zip.All(x => x.First == x.Second) ? 1 : 0;
        }

        var sum = 0;
        // Check if beginning part has sufficient #
        var lengthOfFirstBrokenGroup = record.Split('?', '.')[0].Length;
        if (lengthOfFirstBrokenGroup > springAmounts[0])
        {
            // First part is too long
            return 0;
        }
        else if (lengthOfFirstBrokenGroup == springAmounts[0])
        {
            // Then carry on with the rest.
            // Special case: we are at the end
            if (lengthOfFirstBrokenGroup == record.Length) return 1;

            // Now we continue and assume the char at index {lengthOfFirstBrokenGroup} is a . (as it's either . or ?)

            sum += NumWaysForArrangement(record[(lengthOfFirstBrokenGroup+1)..], springAmounts.Skip(1).ToList());
        }
        else if (lengthOfFirstBrokenGroup > 0)
        {
            // We must continue to reach the minimum length of the group
            var nextChar = record[lengthOfFirstBrokenGroup];
            if (nextChar == '.')
            {
                // Section is too short
                return 0;
            }
            if (nextChar != '?')
                throw new Exception("Should never get here");

            var newRecordArray = record.ToCharArray();
            newRecordArray[lengthOfFirstBrokenGroup] = '#';

            sum += NumWaysForArrangement(new string(newRecordArray), springAmounts);
        }
        else if (lengthOfFirstBrokenGroup == 0)
        {
            // Path 1: ? is '.'
            sum += NumWaysForArrangement(record[1..], springAmounts);

            // Path 2: ? is '#'
            sum += NumWaysForArrangement($"#{record[1..]}", springAmounts);
        }
        else
        {
            throw new Exception("Not expecting this case");
        }

        return sum;
    }

    public static int Part2(List<string> input)
    {
        var sum = 0;
        var i = 0;
        foreach (var record in input)
        {
            i++;
            var springAmounts = Enumerable.Repeat(record.Split(' ')[1].Split(',').Select(int.Parse).ToList(), 5).SelectMany(x => x).ToList();
            var newString = string.Join('?', Enumerable.Repeat(record.Split(' ')[0], 5));
            sum += NumWaysForArrangement(newString, springAmounts);
            if (i % 20 == 0)
            {
                Console.WriteLine("Hello");
            }
        }
        return sum;
    }

    private static List<string> GetSeparatedInputFromFile() =>
        FileInputHelper.GetStringListFromFile("Day12.txt");
}
