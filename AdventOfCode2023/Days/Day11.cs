using AdventOfCode2023.Helpers;

namespace AdventOfCode2023.Days;

public abstract class Day11
{
    public static int Part1Answer() =>
        Part1(GetSeparatedInputFromFile());

    public static long Part2Answer() =>
        Part2(GetSeparatedInputFromFile());

    public static int Part1(List<string> input)
    {
        // Get list of row indices and col indices that are empty:

        var emptySpaceRows = input
            .Select((rowStr, index) => new { RowText = rowStr, RowIndex = index })
            .Where(x => !x.RowText.Contains('#'))
            .Select(x => x.RowIndex)
            .ToList();

        var emptySpaceCols = Enumerable
            .Range(0, input[0].Length)
            .Select(colInd => new {
                ColumnText = new string(input.Select(x => x[colInd]).ToArray()),
                ColumnIndex = colInd
            })
            .Where(x => !x.ColumnText.Contains('#'))
            .Select(x => x.ColumnIndex)
            .ToList();

        // Get coords of unexpanded space
        // Maybe in a dict to start with, rowInd as key, list of colInds?

        var dictOfGalaxies = new Dictionary<int, List<int>>();

        for (int rowInd = 0; rowInd < input.Count; rowInd++)
        {
            for (int colInd = 0; colInd < input[0].Length; colInd++)
            {
                if (input[rowInd][colInd] != '#') continue;

                if (!dictOfGalaxies.ContainsKey(rowInd))
                    dictOfGalaxies.Add(rowInd, new());

                dictOfGalaxies[rowInd].Add(colInd);
            }
        }

        // Now to update coords:

        var dictOfExpandedGalaxies = new Dictionary<int, List<int>>();
        var expandedSpaceRowLength = 0;

        for (int rowInd = 0; rowInd < input.Count; rowInd++)
        {
            if (!dictOfGalaxies.ContainsKey(rowInd))
            {
                expandedSpaceRowLength++;
                continue;
            }

            var colIndices = dictOfGalaxies[rowInd];

            var expandedSpaceColLength = 0;
            for (int colInd = 0; colInd < input[0].Length; colInd++)
            {
                if (emptySpaceCols.Contains(colInd))
                    expandedSpaceColLength++;

                if (!colIndices.Contains(colInd))
                    continue;

                var newKey = rowInd + expandedSpaceRowLength;
                if (!dictOfExpandedGalaxies.ContainsKey(newKey))
                    dictOfExpandedGalaxies.Add(newKey, new());

                var newValue = colInd + expandedSpaceColLength;
                dictOfExpandedGalaxies[newKey].Add(newValue);
            }
        }

        // Now to find spaces between galaxies

        var sumOfSpaceBetweenGalaxies = 0;

        foreach (var galaxyRow in dictOfExpandedGalaxies)
        {
            var rowInd = galaxyRow.Key;

            var galaxyRowValueCopy = new List<int>(galaxyRow.Value);
            foreach (var colInd in galaxyRowValueCopy)
            {
                // Now, iterate over all existing galaxies
                foreach (var targetGalaxyRow in dictOfExpandedGalaxies)
                {
                    var targetRowInd = targetGalaxyRow.Key;
                    foreach (var targetColInd in targetGalaxyRow.Value)
                    {
                        sumOfSpaceBetweenGalaxies += Math.Abs(targetRowInd - rowInd) + Math.Abs(targetColInd - colInd);
                    }
                }

                // Remove current entry from list as we do not need to consider opposite travel
                galaxyRow.Value.Remove(colInd);
            }
        }

        return sumOfSpaceBetweenGalaxies;
    }

    public static long Part2(List<string> input)
    {
        const int EXPANSION_DIST = 999_999;
        // Get list of row indices and col indices that are empty:

        var emptySpaceRows = input
            .Select((rowStr, index) => new { RowText = rowStr, RowIndex = index })
            .Where(x => !x.RowText.Contains('#'))
            .Select(x => x.RowIndex)
            .ToList();

        var emptySpaceCols = Enumerable
            .Range(0, input[0].Length)
            .Select(colInd => new {
                ColumnText = new string(input.Select(x => x[colInd]).ToArray()),
                ColumnIndex = colInd
            })
            .Where(x => !x.ColumnText.Contains('#'))
            .Select(x => x.ColumnIndex)
            .ToList();

        // Get coords of unexpanded space
        // Maybe in a dict to start with, rowInd as key, list of colInds?

        var dictOfGalaxies = new Dictionary<int, List<int>>();

        for (int rowInd = 0; rowInd < input.Count; rowInd++)
        {
            for (int colInd = 0; colInd < input[0].Length; colInd++)
            {
                if (input[rowInd][colInd] != '#') continue;

                if (!dictOfGalaxies.ContainsKey(rowInd))
                    dictOfGalaxies.Add(rowInd, new());

                dictOfGalaxies[rowInd].Add(colInd);
            }
        }

        // Now to update coords:

        var dictOfExpandedGalaxies = new Dictionary<int, List<int>>();
        var expandedSpaceRowLength = 0;

        for (int rowInd = 0; rowInd < input.Count; rowInd++)
        {
            if (!dictOfGalaxies.ContainsKey(rowInd))
            {
                expandedSpaceRowLength += EXPANSION_DIST;
                continue;
            }

            var colIndices = dictOfGalaxies[rowInd];

            var expandedSpaceColLength = 0;
            for (int colInd = 0; colInd < input[0].Length; colInd++)
            {
                if (emptySpaceCols.Contains(colInd))
                    expandedSpaceColLength += EXPANSION_DIST;

                if (!colIndices.Contains(colInd))
                    continue;

                var newKey = rowInd + expandedSpaceRowLength;
                if (!dictOfExpandedGalaxies.ContainsKey(newKey))
                    dictOfExpandedGalaxies.Add(newKey, new());

                var newValue = colInd + expandedSpaceColLength;
                dictOfExpandedGalaxies[newKey].Add(newValue);
            }
        }

        // Now to find spaces between galaxies

        long sumOfSpaceBetweenGalaxies = 0;

        foreach (var galaxyRow in dictOfExpandedGalaxies)
        {
            var rowInd = galaxyRow.Key;

            var galaxyRowValueCopy = new List<int>(galaxyRow.Value);
            foreach (var colInd in galaxyRowValueCopy)
            {
                // Now, iterate over all existing galaxies
                foreach (var targetGalaxyRow in dictOfExpandedGalaxies)
                {
                    var targetRowInd = targetGalaxyRow.Key;
                    foreach (var targetColInd in targetGalaxyRow.Value)
                    {
                        sumOfSpaceBetweenGalaxies += Math.Abs(targetRowInd - rowInd) + Math.Abs(targetColInd - colInd);
                    }
                }

                // Remove current entry from list as we do not need to consider opposite travel
                galaxyRow.Value.Remove(colInd);
            }
        }

        return sumOfSpaceBetweenGalaxies;
    }

    private static List<string> GetSeparatedInputFromFile() =>
        FileInputHelper.GetStringListFromFile("Day11.txt");
}
