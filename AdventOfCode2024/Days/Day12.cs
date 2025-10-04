using AdventOfCodeCommon;
using System.Collections.ObjectModel;

namespace AdventOfCode2024.Days
{
    internal class Day12(bool isTest) : DayBase(12, isTest)
    {
        private static readonly ReadOnlyCollection<(int, int)> DiffValues = new List<(int, int)>
        {
            (-1, 0),
            (0, 1),
            (1, 0),
            (0, -1),
        }.AsReadOnly();

        public override string Part1() => GetPrice(PartNumber.One);
        public override string Part2() => GetPrice(PartNumber.Two);

        private string GetPrice(PartNumber partNumber)
        {
            var input = GetInput();
            var height = input.Length;
            var width = input[0].Length;
            var currentId = 0;
            var coordsToPlantGroupId = new Dictionary<(int, int), int>();
            var plantGroupIdToFenceCount = new Dictionary<int, int>();

            foreach (var searchCoords in GetAllGridCoords(height, width))
            {
                if (coordsToPlantGroupId.ContainsKey(searchCoords)) continue;

                var regionCoordsToProcess = new Queue<(int, int)>([searchCoords]);
                var cornerCount = 0;

                while (regionCoordsToProcess.Count > 0)
                {
                    var currentCoords = regionCoordsToProcess.Dequeue();

                    coordsToPlantGroupId[currentCoords] = currentId;
                    cornerCount += GetFenceCount(
                        partNumber,
                        currentCoords.Item1,
                        currentCoords.Item2,
                        input,
                        coordsToPlantGroupId,
                        regionCoordsToProcess);
                }

                plantGroupIdToFenceCount.Add(currentId, cornerCount);
                currentId++;
            }

            var plantGroupIdToPlantCount = coordsToPlantGroupId
                .GroupBy(kvp => kvp.Value)
                .ToDictionary(g => g.Key, g => g.Count());

            var price = plantGroupIdToFenceCount.Sum(
                (kvp) => kvp.Value * plantGroupIdToPlantCount[kvp.Key]
            );

            return price.ToString();
        }

        private static int GetFenceCount(
            PartNumber partNumber,
            int row,
            int col,
            string[] input,
            Dictionary<(int, int), int> coordsToPlantGroupId,
            Queue<(int, int)> spaceQueue)
        {
            var currentSpaceChar = input[row][col];
            var cornerCount = 0;
            var diffValuesWithFence = new List<(int, int)>();

            foreach (var (rowDiff, colDiff) in DiffValues)
            {
                var newRow = row + rowDiff;
                var newCol = col + colDiff;

                if (newRow < 0 || newRow >= input.Length || newCol < 0 || newCol >= input[0].Length)
                {
                    diffValuesWithFence.Add((rowDiff, colDiff));
                    continue;
                }

                var newSpaceChar = input[newRow][newCol];

                if (newSpaceChar != currentSpaceChar)
                    diffValuesWithFence.Add((rowDiff, colDiff));
                else if (!coordsToPlantGroupId.ContainsKey((newRow, newCol)) && !spaceQueue.Contains((newRow, newCol)))
                    spaceQueue.Enqueue((newRow, newCol));
            }

            if (partNumber == PartNumber.One)
                return diffValuesWithFence.Count;

            for (var diffValueIndex = 0; diffValueIndex < DiffValues.Count; diffValueIndex++)
            {
                // Check for direct corner
                var firstDiffValue = DiffValues[diffValueIndex];
                var secondDiffValue = DiffValues[(diffValueIndex + 1) % DiffValues.Count];
                if (diffValuesWithFence.Contains(firstDiffValue) && diffValuesWithFence.Contains(secondDiffValue))
                    cornerCount++;

                // Check for indirect corner
                else if (!diffValuesWithFence.Contains(firstDiffValue) && !diffValuesWithFence.Contains(secondDiffValue))
                {
                    var indirectCornerRow = row + firstDiffValue.Item1 + secondDiffValue.Item1;
                    var indirectCornerCol = col + firstDiffValue.Item2 + secondDiffValue.Item2;

                    if (currentSpaceChar != input[indirectCornerRow][indirectCornerCol])
                        cornerCount++;
                }
            }

            return cornerCount;
        }

        private enum PartNumber
        {
            One,
            Two
        }

        private static IEnumerable<(int, int)> GetAllGridCoords(int rowCount, int colCount)
        {
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                for (int columnIndex = 0; columnIndex < colCount; columnIndex++)
                    yield return (rowIndex, columnIndex);
        }

        private string[] GetInput() => FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
    }
}
