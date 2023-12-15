using AdventOfCode2023.Helpers;

namespace AdventOfCode2023.Days;

public abstract class Day05
{
    public static long Part1Answer() =>
        Part1(GetSeparatedInputFromFile());

    public static long Part2Answer() =>
        Part2(GetSeparatedInputFromFile());

    public static long Part1(List<string> input)
    {
        var seedList = GetSeeds(input[0]);
        var mapList = input.Skip(1).Select(GetMap).ToList();
        var locationsList = seedList.Select(x => GetSeedLocation(x, mapList)).ToList();

        return locationsList.Min();
    }

    private static List<long> GetSeeds(string initialSectionString)
    {
        return initialSectionString.Split(' ').Skip(1).Select(long.Parse).ToList();
    }

    private static List<List<long>> GetMap(string sectionString)
    {
        /* 
         String is of form:
        seed-to-soil map:
        3680121696 1920754815 614845600
        1920754815 3846369604 448597692
        193356576 570761634 505124585
        ...
         */
        // Assume we use the maps in the order of the input
        return sectionString
            .Split('\n')
            .Skip(1)
            .Select(
                x => x.Split(' ').Select(long.Parse).ToList()
            ).ToList();
    }

    private static List<List<long>> GetOrderedMap(string sectionString)
    {
        // Assume maps do NOT overlap
        return sectionString
            .Split('\n')
            .Skip(1)
            .Select(
                x => x.Split(' ').Select(long.Parse).ToList()
            )
            .OrderBy(x => x[1]) // OrderBy the SOURCE number
            .ToList();
    }

    private static long GetSeedLocation(long seed, List<List<List<long>>> maps)
    {
        var num = seed;
        foreach (var map in maps)
        {
            num = GetMappedOutput(num, map);
        }

        return num;
    }

    private static long GetMappedOutput(long input, List<List<long>> map)
    {
        foreach (var mapEntry in map)
        {
            /*
             * mapEntry is of form { 3680121696 1920754815 614845600 }
             * where 0th num: destination start
             * where 1st num: source range start
             * where 2nd num: range length
             */

            var destStart = mapEntry[0];
            var sourceStart = mapEntry[1];
            var length = mapEntry[2];

            if (sourceStart <= input && input < sourceStart + length)
                // We're within range
                return input + (destStart - sourceStart);
        }

        // We never got within range, so return same number
        return input;
    }

    public static long Part2(List<string> input)
    {
        var seedList = GetPart2Seeds(input[0]);
        var mapList = input.Skip(1).Select(GetOrderedMap).ToList();
        var locationsList = seedList.Select(x => GetSmallestValue(x.Start, x.Length, 0, mapList)).ToList();

        return locationsList.Min();
    }

    private static List<Part2Seed> GetPart2Seeds(string input)
    {
        var numSegments = input.Split(' ').Skip(1).ToList();
        var seedList = new List<Part2Seed>();

        for (int i = 0; i < numSegments.Count; i += 2)
        {
            seedList.Add(new Part2Seed
            {
                Start = long.Parse(numSegments[i]),
                Length = long.Parse(numSegments[i+1]),
            });
        }

        return seedList;
    }

    private class Part2Seed
    {
        public long Start { get; set; }
        public long Length { get; set; }
    }

    private const int HIGHEST_MAPS_INDEX = 7; // 7 maps in total

    public static long GetSmallestValue(long rangeStart, long rangeLength, int mapIndex, List<List<List<long>>> maps)
    {
        // If mapIndex == 7, we've reached the end
        if (mapIndex == HIGHEST_MAPS_INDEX) return rangeStart;

        // Get current map - we can assume the currentMap is ordered by source
        var currentMap = maps[mapIndex];

        long smallestDestValue = long.MaxValue;

        // Maybe not as efficient:
        var entriesThatCollide = currentMap
            .Where(x => !(x[1] + x[2] < rangeStart) && !(rangeStart + rangeLength <= x[1]))
            .ToList();

        var currentRangeStart = rangeStart;
        // Now, we need to call GetSmallestValue iteratively on all ranges
        for (int i = 0; i < entriesThatCollide.Count; i++)
        {
            // First: Check if there's a range BEFORE the map
            var mapEntryThatCollides = entriesThatCollide[i];
            if (currentRangeStart < mapEntryThatCollides[1])
            {
                var nextRangeStart = Math.Min(mapEntryThatCollides[1], rangeStart + rangeLength);
                var num = GetSmallestValue(currentRangeStart, nextRangeStart - currentRangeStart, mapIndex + 1, maps);

                if (num < smallestDestValue)
                    smallestDestValue = num;
                   
                currentRangeStart = nextRangeStart;
            }

            // Now check range in map entry:
            var nextRangeStartt = Math.Min(mapEntryThatCollides[1] + mapEntryThatCollides[2], rangeStart + rangeLength);
            if (nextRangeStartt < currentRangeStart)
                throw new Exception("nextRangeStart smaller than current");

            // Start of Range needs to be converted to the new map, i.e. 
            var numm = GetSmallestValue(currentRangeStart + (mapEntryThatCollides[0] - mapEntryThatCollides[1]), nextRangeStartt - currentRangeStart, mapIndex + 1, maps);

            if (numm < smallestDestValue)
                smallestDestValue = numm;

            currentRangeStart = nextRangeStartt;
        }

        if (currentRangeStart < rangeStart + rangeLength)
        {
            // Final part:
            var num = GetSmallestValue(currentRangeStart, (rangeStart + rangeLength) - currentRangeStart, mapIndex + 1, maps);
            if (num < smallestDestValue)
                smallestDestValue = num;
        }

        return smallestDestValue;
    }

    private static List<string> GetSeparatedInputFromFile() => 
        FileInputHelper.GetStringListFromFile("Day05.txt", "\n\n");
}
