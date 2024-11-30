using AdventOfCode2023.Helpers;

namespace AdventOfCode2023.Days;

// ---- WIP ----
public abstract class Day23
{
    public static int Part1Answer() =>
        Part1(GetSeparatedInputFromFile());

    public static int Part2Answer() =>
        Part2(GetSeparatedInputFromFile());

    private static IReadOnlyCollection<(int, int)> SearchDiffs = new List<(int, int)>
    {
        (-1,  0),
        ( 0,  1),
        ( 1,  0),
        ( 0, -1),
    }.AsReadOnly();

    private static IReadOnlyDictionary<(int, int), char> SearchDiffToValidChar = new Dictionary<(int, int), char>
    {
        { (-1,  0), '^' },
        { ( 0,  1), '>' },
        { ( 1,  0), 'v' },
        { ( 0, -1), '<' },
    };

    public static int Part1(List<string> input)
    {
        // Assumptions:
        //  - There's only one '.' in the first row (our start)
        //  - As we search, we need to understand what tile we've come from, so that we don't go back on ourselves
        //      - With the previous assumption, we assume we don't ever circle back on ourselves
        //  - If we ever approach a slope that is against us, we kill off that route (i.e. it's invalid, and returns 'null' or something for distance)
        //  - We'll do DFS with iterative function calls. We can also memoise
        //      - Only need to memoise "forks" in the road for efficiency.
        //  - The iterative function call only needs to occur at forks

        // Dictionary from (row, col) to Distance from end.
        // If entry doesn't exist, hasn't been calculated yet.
        // Assume we only need to save coords of forks
        var coordsToDistanceFromEnd = new Dictionary<(int, int), int>();

        var indexOfFirstDot = GetIndexOfFirstDot(input);
        var firstStep = (0, indexOfFirstDot);
        var secondStep = (1, indexOfFirstDot);

        var distanceFromSecondStep = CalculateLongestDistanceFromEnd(
            prevCoords: firstStep,
            coords: secondStep,
            input: input,
            coordsToDistanceFromEnd: coordsToDistanceFromEnd
         );

        var distanceFromFirstStep = distanceFromSecondStep + 1;

        return distanceFromFirstStep;
    }

    public static int Part2(List<string> input)
    {
        var coordsToDistanceFromEnd = new Dictionary<(int, int), int>();

        var indexOfFirstDot = GetIndexOfFirstDot(input);
        var firstStep = (0, indexOfFirstDot);
        var secondStep = (1, indexOfFirstDot);

        var distanceFromSecondStep = CalculateLongestDistanceFromEndDry(
            prevCoords: firstStep,
            coords: secondStep,
            input: input,
            // coordsToDistanceFromEnd: coordsToDistanceFromEnd,
            forksVisited: new HashSet<(int, int)>()
         );

        var distanceFromFirstStep = distanceFromSecondStep + 1;

        return distanceFromFirstStep;
    }

    private static int GetIndexOfFirstDot(List<string> input)
    {
        var indexOfFirstDot = input[0].IndexOf('.');
        if (indexOfFirstDot == -1)
            throw new Exception("The dot in the first row was not found");

        return indexOfFirstDot;
    }

    public static int CalculateLongestDistanceFromEnd(
        (int, int) coords,
        (int, int) prevCoords,
        List<string> input,
        Dictionary<(int, int), int> coordsToDistanceFromEnd)
    {
        // Step 1: If current coords has max distance already calculated, use it
        if (coordsToDistanceFromEnd.ContainsKey(coords))
            return coordsToDistanceFromEnd[coords];

        // Step 2: If we're at the last row, we can return 0 (as it's the end)
        if (coords.Item1 == input.Count - 1)
            return 0;

        // Step 3: Gather list of searchable neighbour squares
        //  NOTE: Normally, we'd check if coords are out of bounds, but if we do things smart, we don't have to do this (due to the # border)

        var listOfNextCoords = new List<(int, int)>();
        foreach (var diff in SearchDiffs)
        {
            var searchCoords = (coords.Item1 + diff.Item1, coords.Item2 + diff.Item2);

            // Ignore square we were just on
            if (prevCoords == searchCoords)
                continue;

            var charAtSearch = input[searchCoords.Item1][searchCoords.Item2];
            if (charAtSearch == '.' || charAtSearch == SearchDiffToValidChar[diff])
                listOfNextCoords.Add(searchCoords);
        }

        // Step 4: If we can go nowhere, return 0
        if (listOfNextCoords.Count == 0)
            return 0;

        // Step 5: If we can go somewhere, take the maximum distance from all paths, and add 1 (to include this current step)
        var largestDistance = listOfNextCoords
            .Select(x =>
                CalculateLongestDistanceFromEnd(
                    coords: x,
                    prevCoords: coords,
                    input,
                    coordsToDistanceFromEnd
                )
            ).Max() + 1;

        // Step 6: Add the distance to the dictionary, so we can reuse it later
        coordsToDistanceFromEnd.Add(coords, largestDistance);

        return largestDistance;
    }

    public static int CalculateLongestDistanceFromEndDry(
        (int, int) coords,
        (int, int) prevCoords,
        List<string> input,
        // Dictionary<(int, int), int> coordsToDistanceFromEnd,
        HashSet<(int, int)> forksVisited)
    {
        // Step 0: If visited already, return MinValue?
        if (forksVisited.Contains(coords))
            return int.MinValue;

        //// Step 1: If current coords has max distance already calculated, use it
        //if (coordsToDistanceFromEnd.ContainsKey(coords))
        //    return coordsToDistanceFromEnd[coords];

        // Step 2: If we're at the last row, we can return 0 (as it's the end)
        if (coords.Item1 == input.Count - 1)
            return 0;

        // Step 3: Gather list of searchable neighbour squares
        //  NOTE: Normally, we'd check if coords are out of bounds, but if we do things smart, we don't have to do this (due to the # border)

        var listOfNextCoords = new List<(int, int)>();
        foreach (var diff in SearchDiffs)
        {
            var searchCoords = (coords.Item1 + diff.Item1, coords.Item2 + diff.Item2);

            // Ignore square we were just on
            if (prevCoords == searchCoords)
                continue;

            var charAtSearch = input[searchCoords.Item1][searchCoords.Item2];
            if (charAtSearch != '#')
                listOfNextCoords.Add(searchCoords);
        }

        // Step 4a: If we can go nowhere, return 0
        if (listOfNextCoords.Count == 0)
            return 0;

        // Step 4b: If there's more than 1, list that we've visited this fork
        forksVisited.Add(coords);

        // Step 5: If we can go somewhere, take the maximum distance from all paths, and add 1 (to include this current step)
        var largestDistance = listOfNextCoords
            .Select(x =>
                CalculateLongestDistanceFromEndDry(
                    coords: x,
                    prevCoords: coords,
                    input,
                    // coordsToDistanceFromEnd,
                    forksVisited
                )
            ).Max() + 1;

        //// Step 6: Add the distance to the dictionary, so we can reuse it later
        //coordsToDistanceFromEnd.Add(coords, largestDistance);

        // Step 7: We can remove the coords from forksVisited
        forksVisited.Remove(coords);

        return largestDistance;
    }

    private static List<string> GetSeparatedInputFromFile() =>
        FileInputHelper.GetStringListFromFile("Day23.txt");
}
