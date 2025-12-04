using AdventOfCodeCommon;
using AdventOfCodeCommon.Extensions;

namespace AdventOfCode2025.Days
{
    internal class Day04(bool isTest) : DayBase(4, isTest)
    {
        private const int MAX_ADJACENT_ROLL_COUNT_TO_BE_ACCESSIBLE = 3;

        public override string Part1() => GetAccessibleRollCount(removeAsYouGo: false);
        public override string Part2() => GetAccessibleRollCount(removeAsYouGo: true);

        private string GetAccessibleRollCount(bool removeAsYouGo)
        {
            var input = GetInput();
            var accessibleRollCount = 0;
            var isRollRemoved = true;

            while (isRollRemoved)
            {
                isRollRemoved = false;

                for (int row = 0; row < input.Length; row++)
                {
                    for (int col = 0; col < input[0].Length; col++)
                    {
                        if (input[row][col] != '@')
                            continue;

                        var adjacentRollCount = 0;

                        foreach (var (searchRow, searchCol) in GetSearchCoords(row, col))
                        {
                            if (!input.IsInBounds(searchRow, searchCol))
                                continue;

                            if (input[searchRow][searchCol] == '@')
                                adjacentRollCount++;
                        }

                        if (adjacentRollCount <= MAX_ADJACENT_ROLL_COUNT_TO_BE_ACCESSIBLE)
                        {
                            accessibleRollCount++;

                            if (!removeAsYouGo)
                                continue;

                            isRollRemoved = true;
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
