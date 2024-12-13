using AdventOfCodeCommon;
using AdventOfCodeCommon.Models;
using System.Collections.ObjectModel;

namespace AdventOfCode2024.Days
{
    internal class Day10(bool isTest) : DayBase(10, isTest)
    {
        private static readonly IReadOnlyList<(int, int)> DiffValues = new List<(int, int)>
        {
            (-1,  0),
            ( 0,  1),
            ( 1,  0),
            ( 0, -1)
        }.AsReadOnly();

        public override string Part1()
        {
            var input = GetInput();
            var trailHeadCount = 0;

            for (int row = 0; row < input.Length; row++)
            {
                for (int col = 0; col < input[0].Length; col++)
                {
                    var searchValue = input[row][col];

                    if (searchValue == 0)
                    {
                        var reachableNines = new HashSet<(int, int)>();
                        CollectReachableNines(row, col, input, reachableNines);
                        trailHeadCount += reachableNines.Count;
                    }
                }
            }

            return trailHeadCount.ToString();
        }

        public override string Part2()
        {

            var input = GetInput();
            var trailHeadCount = 0;

            for (int row = 0; row < input.Length; row++)
            {
                for (int col = 0; col < input[0].Length; col++)
                {
                    var searchValue = input[row][col];

                    if (searchValue == 0)
                        trailHeadCount += ReachableNinesCount(row, col, input);
                }
            }

            return trailHeadCount.ToString();
        }

        private static int ReachableNinesCount(int row, int col, int[][] input)
        {
            var currentValue = input[row][col];

            if (currentValue == 9) return 1;

            var reachableNineCount = 0;
            foreach (var diff in DiffValues)
            {
                var searchRow = row + diff.Item1;
                var searchCol = col + diff.Item2;

                if (searchRow < 0 || searchRow >= input.Length || searchCol < 0 || searchCol >= input[0].Length)
                    continue;

                var searchValue = input[searchRow][searchCol];

                if (searchValue != currentValue + 1)
                    continue;

                reachableNineCount += ReachableNinesCount(searchRow, searchCol, input);
            }

            return reachableNineCount;
        }

        private static void CollectReachableNines(int row, int col, int[][] input, HashSet<(int, int)> reachableNinesCoords)
        {
            var currentValue = input[row][col];

            if (currentValue == 9)
            {
                reachableNinesCoords.Add((row, col));
                return;
            }

            foreach (var diff in DiffValues)
            {
                var searchRow = row + diff.Item1;
                var searchCol = col + diff.Item2;

                if (searchRow < 0 || searchRow >= input.Length || searchCol < 0 || searchCol >= input[0].Length)
                    continue;

                var searchValue = input[searchRow][searchCol];

                if (searchValue != currentValue + 1)
                    continue;

                CollectReachableNines(searchRow, searchCol, input, reachableNinesCoords);
            }
        }

        private int[][] GetInput()
            => FileInputAssistant.GetIntArrayRowsFromFile(TextInputFilePath);
    }
}
