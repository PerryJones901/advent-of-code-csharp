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

        public override string Part1()
        {
            var input = GetInput();
            var height = input.Length;
            var width = input[0].Length;
            var currentId = 0;
            var coordsToPlantGroupId = new Dictionary<(int, int), int>();
            var plantGroupIdToFenceCount = new Dictionary<int, int>();

            for (var row = 0; row < height; row++)
            {
                for (var col = 0; col < width; col++)
                {
                    var currentCoord = (row, col);
                    if (!coordsToPlantGroupId.ContainsKey(currentCoord))
                    {
                        var coordQueue = new Queue<(int, int)>([currentCoord]);
                        var fenceCount = 0;

                        while (coordQueue.Count > 0)
                        {
                            var searchCoord = coordQueue.Dequeue();
                            coordsToPlantGroupId[searchCoord] = currentId;
                            fenceCount += GetFenceCount(searchCoord.Item1, searchCoord.Item2, input, coordsToPlantGroupId, coordQueue);
                        }

                        plantGroupIdToFenceCount.Add(currentId, fenceCount);
                        currentId++;
                    }
                }
            }

            var plantGroupIdToPlantCount = coordsToPlantGroupId
                .GroupBy(kvp => kvp.Value)
                .ToDictionary(g => g.Key, g => g.Count());

            var price = plantGroupIdToFenceCount.Aggregate(0, (current, kvp) => current + kvp.Value * plantGroupIdToPlantCount[kvp.Key]);

            return price.ToString();
        }

        public override string Part2()
        {
            return "Part 2";
        }

        private static int GetFenceCount(
            int row,
            int col,
            string[] input,
            Dictionary<(int, int), int> coordsToPlantGroupId,
            Queue<(int, int)> spaceQueue)
        {
            var currentSpaceChar = input[row][col];
            var fenceCount = 0;

            foreach (var (rowDiff, colDiff) in DiffValues)
            {
                var newRow = row + rowDiff;
                var newCol = col + colDiff;

                if (newRow < 0 || newRow >= input.Length || newCol < 0 || newCol >= input[0].Length)
                {
                    fenceCount++;
                    continue;
                }

                var newSpaceChar = input[newRow][newCol];

                if (newSpaceChar != currentSpaceChar)
                    fenceCount++;
                else if (!coordsToPlantGroupId.ContainsKey((newRow, newCol)) && !spaceQueue.Contains((newRow, newCol)))
                    spaceQueue.Enqueue((newRow, newCol));
            }

            return fenceCount;
        }

        private string[] GetInput()
        {
            return FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
        }
    }
}
