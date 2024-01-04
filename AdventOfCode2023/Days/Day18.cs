using AdventOfCode2023.Helpers;
using System.Collections.Immutable;

namespace AdventOfCode2023.Days;

public abstract class Day18
{
    public static int Part1Answer() =>
        Part1(GetSeparatedInputFromFile());

    public static long Part2Answer() =>
        Part2(GetSeparatedInputFromFile());

    public static int Part1(string input)
    {
        var parts = StringHelper.GetRegexCapturesFromInput(
            input,
            @"(\w) (\d+) (\(#[0-9a-f]{6}\))"
        );

        var minRowIndex = 0;
        var maxRowIndex = 0;
        var minColIndex = 0;
        var maxColIndex = 0;
        var currentRow = 0;
        var currentCol = 0;

        foreach (var part in parts)
        {
            var stepAmount = int.Parse(part[1]);
            switch (part[0])
            {
                case "U": currentRow -= stepAmount; break;
                case "R": currentCol += stepAmount; break;
                case "D": currentRow += stepAmount; break;
                case "L": currentCol -= stepAmount; break;
            }

            if (currentRow < minRowIndex)
                minRowIndex = currentRow;
            if (currentRow > maxRowIndex)
                maxRowIndex = currentRow;
            if (currentCol < minColIndex)
                minColIndex = currentCol;
            if (currentCol > maxColIndex)
                maxColIndex = currentCol;

            Console.WriteLine($"Row: {currentRow}, Col: {currentCol}");
        }

        Console.WriteLine($"Rows: {minRowIndex} to {maxRowIndex}");
        Console.WriteLine($"Cols: {minColIndex} to {maxColIndex}");

        // Create string array of correct size
        var grid = Enumerable
            .Repeat(
                () => Enumerable.Repeat(
                    '.',
                    maxColIndex - minColIndex + 1
                ).Select(x => x).ToArray(),
                maxRowIndex - minRowIndex + 1
            )
            .Select(x => new string(x()))
            .ToArray();

        currentRow = Math.Abs(minRowIndex);
        currentCol = Math.Abs(minColIndex);

        // REMOVE THIS
        grid[currentRow] = $"{grid[currentRow][0..currentCol]}X{grid[currentRow][(currentCol + 1)..]}";

        foreach (var part in parts)
        {
            var stepAmount = int.Parse(part[1]);
            var rowDiff = 0;
            var colDiff = 0;
            switch (part[0])
            {
                case "U": rowDiff = -1; break;
                case "R": colDiff = +1; break;
                case "D": rowDiff = +1; break;
                case "L": colDiff = -1; break;
            }

            for (var i = 1; i <= stepAmount; i++)
            {
                currentRow += rowDiff;
                currentCol += colDiff;

                var row = grid[currentRow];
                grid[currentRow] = $"{row[0..currentCol]}#{row[(currentCol + 1)..]}";
            }

            Console.WriteLine($"Row: {currentRow}, Col: {currentCol}");
        }

        foreach (var row in grid)
            Console.WriteLine(row);

        // Now, to fill everything in
        // Cheat, as we know a good fill point
        var queue = new Queue<(int, int)>();
        queue.Enqueue((Math.Abs(minRowIndex) + 1, Math.Abs(minColIndex) + 1));
        while(queue.Count > 0)
        {
            FloodFill(grid, queue);
        }

        foreach (var row in grid)
            Console.WriteLine(row);

        return grid.Sum(x => x.Count(y => y == '#'));
    }

    public static void FloodFill(string[] grid, Queue<(int, int)> queue)
    {
        var item = queue.Dequeue();
        var fillPointRow = item.Item1;
        var fillPointCol = item.Item2;

        if (!IsInBounds(fillPointRow, fillPointCol, grid.Length, grid[0].Length))
            return;

        if (grid[fillPointRow][fillPointCol] != '.')
            return;

        grid[fillPointRow] = $"{grid[fillPointRow][0..fillPointCol]}#{grid[fillPointRow][(fillPointCol + 1)..]}";

        queue.Enqueue((fillPointRow - 1, fillPointCol));
        queue.Enqueue((fillPointRow + 1, fillPointCol));
        queue.Enqueue((fillPointRow, fillPointCol - 1));
        queue.Enqueue((fillPointRow, fillPointCol + 1));
    }

    private static bool IsInBounds(int row, int col, int maxRow, int maxCol)
    {
        return 0 <= row && row < maxRow && 0 <= col && col < maxCol;
    }

    public static long Part2(string input)
    {
        var colourCodes = StringHelper.GetRegexCapturesFromInput(
            input,
            @"(\w) (\d+) \(#([0-9a-f]{6})\)"
        ).Select(x => x[2]).ToList();

        var data = colourCodes.Select(x => (int.Parse(x[..5], System.Globalization.NumberStyles.HexNumber), int.Parse(x[5].ToString()))).ToList();

        var dict = new Dictionary<int, HashSet<int>>();

        var currentRow = 0;
        var currentCol = 0;

        foreach (var (amount, dirCode) in data)
        {
            if (dirCode == 0)
                currentCol += amount;
            else if (dirCode == 1)
                currentRow += amount;
            else if (dirCode == 2)
                currentCol -= amount;
            else if (dirCode == 3)
                currentRow -= amount;

            if (!dict.ContainsKey(currentRow))
                dict[currentRow] = new HashSet<int>();

            dict[currentRow].Add(currentCol);
        }

        var points = dict.ToImmutableSortedDictionary(x => x.Key, x => x.Value.ToImmutableSortedSet().ToList());

        var endpoints = new SortedSet<int>();

        var pointsPainted = 0L;

        for (int i = 0; i < points.Count - 1; i++)
        {
            var endpointsCopy = new SortedSet<int>(endpoints);
            var (row, colList) = points.ElementAt(i);
            var nextRow = points.ElementAt(i + 1).Key;
            var removedEndpoints = new List<int>();
            // Update endpoints
            foreach (var col in colList)
            {
                if (endpoints.Contains(col))
                {
                    endpoints.Remove(col);
                    removedEndpoints.Add(col);
                }
                else
                    endpoints.Add(col);
            }

            if (endpoints.Count % 2 == 1)
                throw new Exception("We shouldn't get here - should be even number");

            var sumOfRegionWidths = 0L;
            var endpointsList = endpoints.ToList();
            var endpointPairs = new List<(int, int)>();
            var oldEndpointPairs = new List<(int, int)>();
            for (var j = 0; j < endpoints.Count; j += 2)
            {
                // Calculate width of regions
                var start = endpointsList[j];
                var end = endpointsList[j + 1];
                endpointPairs.Add((start, end));
                sumOfRegionWidths += end - start + 1;
            }
            // Do old endpoint pairs too
            for (var j = 0; j < endpointsCopy.Count; j += 2)
            {
                // Calculate width of regions
                var start = endpointsCopy.ElementAt(j);
                var end = endpointsCopy.ElementAt(j + 1);
                oldEndpointPairs.Add((start, end));
            }

            pointsPainted += sumOfRegionWidths * (nextRow - row);

            // There are some lines that may not have been added
            var allEndpoints =
                (endpoints.Select(x => new { Value = x, Removed = false }))
                .Concat(removedEndpoints.Select(x => new { Value = x, Removed = true }))
                .OrderBy(x => x.Value).ToList();

            // Now, go pairwise along the all endpoints list, and check if the region is included in endpointPairs
            var allEndpointsZip = allEndpoints.Zip(allEndpoints.Skip(1), (x, y) => (x, y));

            foreach (var (left, right) in allEndpointsZip)
            {
                if (endpointPairs.Any(x => x.Item1 <= left.Value && x.Item2 >= right.Value))
                    continue;

                // If outside a region, we need to check if the line was new, i.e. both points where added. If not, continue.
                if (!(colList.Contains(left.Value) && colList.Contains(right.Value)))
                    continue;

                // Check if this line is within an old region
                if (!oldEndpointPairs.Any(x => x.Item1 <= left.Value && x.Item2 >= right.Value))
                    continue;

                // Here, we have a region that is not included in endpointPairs
                var extraLineWidths = right.Value - left.Value - 1; // Strictly inside the region
                // Now, decide if we need to add the endpoints
                extraLineWidths += (left.Removed ? 1 : 0);
                extraLineWidths += (right.Removed ? 1 : 0);
                pointsPainted += extraLineWidths;
            }

            // Do last row if needed
            if (i == points.Count - 2)
            {
                // Add on another sumOfRegionWidths
                pointsPainted += sumOfRegionWidths;
            }
        }

        return pointsPainted;
    }

    private static string GetSeparatedInputFromFile() =>
        FileInputHelper.GetStringFromFile("Day18.txt");
}
