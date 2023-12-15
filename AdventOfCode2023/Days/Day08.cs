using AdventOfCode2023.Helpers;

namespace AdventOfCode2023.Days;

public abstract class Day08
{
    public static int Part1Answer() =>
        Part1(GetSeparatedInputFromFile());

    public static long Part2Answer() =>
        Part2(GetSeparatedInputFromFile());

    public static int Part1(List<string> input)
    {
        var leftAndRights = input[0]
            .Replace('L', '0')
            .Replace('R', '1')
            .Select(x => int.Parse(x.ToString()))
            .ToArray();

        var dictOfPaths = new Dictionary<string, string[]>();

        foreach (var path in input.Skip(2))
        {
            var key = path[0..3];
            var leftPath = path[7..10];
            var rightPath = path[12..15];

            dictOfPaths.Add(key, new string[] { leftPath, rightPath });
        }

        int numSteps = 0;
        string currentKey = "AAA";

        while (true)
        {
            var indexInLeftAndRights = (numSteps % leftAndRights.Length);
            var index = leftAndRights[indexInLeftAndRights];

            numSteps++;
            currentKey = dictOfPaths[currentKey][index];
            if (currentKey == "ZZZ") break;
        }

        return numSteps;
    }

    public static long Part2(List<string> input)
    {
        var leftAndRights = input[0]
            .Replace('L', '0')
            .Replace('R', '1')
            .Select(x => int.Parse(x.ToString()))
            .ToArray();

        var dictOfPaths = new Dictionary<string, string[]>();

        foreach (var path in input.Skip(2))
        {
            var key = path[0..3];
            var leftPath = path[7..10];
            var rightPath = path[12..15];

            dictOfPaths.Add(key, new string[] { leftPath, rightPath });
        }

        // Find all keys that end in A
        var keys = dictOfPaths.Keys.Where(x => x.EndsWith('A')).ToList();
        var stepsTilFirstZ = Enumerable.Repeat(0L, 6).ToList();

        long numSteps = 0;
        while (true)
        {
            var indexInLeftAndRights = (numSteps % leftAndRights.Length);
            var index = leftAndRights[indexInLeftAndRights];
            numSteps++;

            keys = keys.Select(x => dictOfPaths[x][index]).ToList();
            if (keys.All(x => x.EndsWith('Z')))
                break;

            if (keys.Any(x => x.EndsWith('Z')))
            {
                for (int i = 0; i < keys.Count; i++)
                    if (keys[i].EndsWith('Z') && stepsTilFirstZ[i] == 0)
                        stepsTilFirstZ[i] = numSteps;
                Console.WriteLine(numSteps);
            }

            if (!stepsTilFirstZ.Any(x => x == 0)) break;
        }

        return LowestCommonMultiple(stepsTilFirstZ);
    }

    private static long LowestCommonMultiple(List<long> numbers)
    {
        return numbers.Aggregate(
            1L,
            (acc, current) => LowestCommonMultipleOfTwoNumbers(acc,current)
        );
    }

    static long GreatestCommonDivisor(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    static long LowestCommonMultipleOfTwoNumbers(long a, long b)
    {
        return (a / GreatestCommonDivisor(a, b)) * b;
    }

    private static List<string> GetSeparatedInputFromFile() =>
        FileInputHelper.GetStringListFromFile("Day08.txt");
}
