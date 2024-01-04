using AdventOfCode2023.Helpers;
using System.Collections.ObjectModel;

namespace AdventOfCode2023.Days;

public abstract class Day12
{
    public static long Part1Answer() =>
        Part1(GetSeparatedInputFromFile());

    public static long Part2Answer() =>
        Part2(GetSeparatedInputFromFile());

    public static long Part1(List<string> input)
    {
        // 7993 -- too high
        var sum = 0L;
        foreach(var record in input)
        {
            var springAmounts = record.Split(' ')[1].Split(',').Select(int.Parse).ToList();
            var cache = new Dictionary<(string, ReadOnlyCollection<int>), long>();
            // sum += NumWaysForArrangement(record.Split(' ')[0], springAmounts);
            sum += Calc(record.Split(' ')[0], springAmounts.AsReadOnly(), cache);
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

    private static int NumWaysForArrangement2(string record, List<int> springAmounts, Dictionary<(string, ReadOnlyCollection<int>), int> cache)
    {
        if (cache.ContainsKey((record, springAmounts.AsReadOnly())))
            return cache[(record, springAmounts.AsReadOnly())];

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

            sum += NumWaysForArrangement2(record[(lengthOfFirstBrokenGroup + 1)..], springAmounts.Skip(1).ToList(), cache);
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

            sum += NumWaysForArrangement2(new string(newRecordArray), springAmounts, cache);
        }
        else if (lengthOfFirstBrokenGroup == 0)
        {
            // Path 1: ? is '.'
            sum += NumWaysForArrangement2(record[1..], springAmounts, cache);

            // Path 2: ? is '#'
            sum += NumWaysForArrangement2($"#{record[1..]}", springAmounts, cache);
        }
        else
        {
            throw new Exception("Not expecting this case");
        }

        cache.Add((record, springAmounts.AsReadOnly()), sum);

        return sum;
    }

    public static long Calc(string record, ReadOnlyCollection<int> springAmounts, Dictionary<(string, ReadOnlyCollection<int>), long> cache)
    {
        // Check cache
        if (cache.ContainsKey((record, springAmounts)))
            return cache[(record, springAmounts)];

        var AddToCacheAndReturn = (long result) =>
        {
            cache.Add((record, springAmounts), result);
            return result;
        };

        // Spring Amounts empty
        if (springAmounts.Count == 0)
            return AddToCacheAndReturn(!record.Contains('#') ? 1 : 0);

        // Record empty but not Sprint Amounts
        if (record.Length == 0)
            return AddToCacheAndReturn(0);

        // Next element
        var nextChar = record[0];
        var nextSpringAmount = springAmounts[0];

        // POTENTIALLY REMOVE:
        // If record is shorter than next spring amount, ditch
        if (record.Length < nextSpringAmount)
            return AddToCacheAndReturn(0);

        // Logic treating first char as #
        var Pound = () =>
        {
            var thisGroup = record[..nextSpringAmount].Replace('?', '#');

            // Abort if this group cannot form the correct shape
            if (thisGroup != Enumerable.Repeat('#', nextSpringAmount).Aggregate("", (x, y) => x + y))
                return 0;

            // If we've reached the end, check if this is the last spring amount. If so, great, otherwise abort
            if (record.Length == nextSpringAmount)
                return springAmounts.Count == 1 ? 1 : 0;

            // Make sure char that follows this group can be a separator
            if ("?.".Contains(record[nextSpringAmount]))
                return Calc(record[(nextSpringAmount + 1)..], springAmounts.Skip(1).ToList().AsReadOnly(), cache);

            return 0;
        };
        var Dot = () => Calc(record[1..], springAmounts, cache);

        return AddToCacheAndReturn(nextChar switch
        {
            '.' => Dot(),
            '#' => Pound(),
            '?' => Dot() + Pound(),
            _ => throw new Exception("Not expecting this case"),
        });
    }

    public static long Part2(List<string> input)
    {
        var sum = 0L;
        var i = 0;
        var cache = new Dictionary<(string, ReadOnlyCollection<int>), long>();

        foreach (var record in input)
        {
            i++;
            var springAmounts = Enumerable.Repeat(record.Split(' ')[1].Split(',').Select(int.Parse).ToList(), 5).SelectMany(x => x).ToList().AsReadOnly();
            var newString = string.Join('?', Enumerable.Repeat(record.Split(' ')[0], 5));
            var result = Calc(newString, springAmounts, cache);
            sum += result;

            Console.WriteLine($"Line {i}: {result}. Current sum: {sum}. Current cache size: {cache.Count}");
        }
        return sum;
    }

    private static List<string> GetSeparatedInputFromFile() =>
        FileInputHelper.GetStringListFromFile("Day12.txt");
}
