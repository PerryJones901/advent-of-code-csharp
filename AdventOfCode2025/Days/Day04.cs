using AdventOfCodeCommon;
using AdventOfCodeCommon.Extensions;

namespace AdventOfCode2025.Days
{
    internal class Day04(bool isTest) : DayBase(4, isTest)
    {
        private const int MAX_ACCESSIBLE_COUNT = 3;
        public override string Part1()
        {
            var input = GetInput();

            var rowCount = input.Length;
            var colCount = input[0].Length;

            var accessibleRollCount = 0;

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    var currentChar = input[row][col];
                    if (currentChar != '@')
                        continue;

                    var adjacentRollCount = 0;

                    foreach (var (searchRow, searchCol) in GetSearchCoords(row, col))
                    {
                        if (!input.IsInBounds(searchRow, searchCol))
                            continue;

                        var searchChar = input[searchRow][searchCol];
                        if (searchChar == '@')
                            adjacentRollCount++;
                    }

                    if (adjacentRollCount <= MAX_ACCESSIBLE_COUNT)
                        accessibleRollCount++;
                }
            }

            return accessibleRollCount.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();

            var rowCount = input.Length;
            var colCount = input[0].Length;

            var accessibleRollCount = 0;
            var isRollRemoved = true;

            while(isRollRemoved)
            {
                isRollRemoved = false;

                for (int row = 0; row < rowCount; row++)
                {
                    for (int col = 0; col < colCount; col++)
                    {
                        var currentChar = input[row][col];
                        if (currentChar != '@')
                            continue;

                        var adjacentRollCount = 0;

                        foreach (var (searchRow, searchCol) in GetSearchCoords(row, col))
                        {
                            if (!input.IsInBounds(searchRow, searchCol))
                                continue;

                            var searchChar = input[searchRow][searchCol];
                            if (searchChar == '@')
                                adjacentRollCount++;
                        }

                        if (adjacentRollCount <= MAX_ACCESSIBLE_COUNT)
                        {
                            isRollRemoved = true;
                            accessibleRollCount++;
                            var inputRowChars = input[row].ToCharArray();
                            inputRowChars[col] = '.';
                            input[row] = new string(inputRowChars);
                        }
                    }
                }
            }           

            return accessibleRollCount.ToString();
        }

        private static IList<(int, int)> GetSearchCoords(int row, int col)
            =>
            [
                (row - 1, col    ),
                (row - 1, col + 1),
                (row    , col + 1),
                (row + 1, col + 1),
                (row + 1, col    ),
                (row + 1, col - 1),
                (row    , col - 1),
                (row - 1, col - 1)
            ];

        private string[] GetInput()
            => FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
    }
}
