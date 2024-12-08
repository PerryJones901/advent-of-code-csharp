using AdventOfCodeCommon;
using AdventOfCodeCommon.Extensions;
using System.Collections.ObjectModel;

namespace AdventOfCode2024.Days
{
    internal class Day04(bool isTest) : DayBase(4, isTest)
    {
        private const string SearchValue = "XMAS";
        private static readonly ReadOnlyCollection<(int, int)> DiffValues = new List<(int, int)>
        {
            (-1, 0),
            (-1, 1),
            (0, 1),
            (1, 1),
            (1, 0),
            (1, -1),
            (0, -1),
            (-1, -1),
        }.AsReadOnly();

        public override string Part1()
        {
            var input = GetInput();
            var count = 0;

            foreach(var (row, col) in GetGridCoords(input))
            {
                foreach (var (diffRow, diffCol) in DiffValues)
                {
                    var isWordFound = Enumerable
                        .Range(0, SearchValue.Length)
                        .All(charIndex =>
                            IsCharAtSearchCoords(input, row, col, diffRow * charIndex, diffCol * charIndex, SearchValue[charIndex])
                        );

                    if (isWordFound) count++;
                }
            }

            return count.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();
            var count = 0;

            foreach (var (row, col) in GetGridCoords(input, ignoreEdges: true))
            {
                var indicesOfM = new HashSet<int>();

                if (input[row][col] != 'A') continue;

                for (int m = 0; m < DiffValues.Count; m++)
                {
                    // Ignore non diagonal directions (took me way too long to realise this)
                    if (m % 2 == 0) continue;

                    var checkCases = new[] {
                        new { Char = 'M', DiffCoords = DiffValues[m] },
                        new { Char = 'S', DiffCoords = DiffValues[(m + 4) % DiffValues.Count] },
                    };

                    var masFound = checkCases.All(checkCase =>
                        IsCharAtSearchCoords(input, row, col, checkCase.DiffCoords.Item1, checkCase.DiffCoords.Item2, checkCase.Char)
                    );

                    if (masFound) indicesOfM.Add(m);
                }

                foreach (var m in indicesOfM)
                {
                    // Check if we also have a MAS 90deg CW
                    if (indicesOfM.Contains((m + 2) % DiffValues.Count)) count++;
                }
            }

            return count.ToString();
        }

        private static IEnumerable<(int, int)> GetGridCoords(string[] input, bool ignoreEdges = false)
        {
            var width = input[0].Length;
            var height = input.Length;
            var offset = ignoreEdges ? 1 : 0;

            for (int i = offset; i < height - offset; i++)
            {
                for (int j = offset; j < width - offset; j++)
                {
                    yield return (i, j);
                }
            }
        }

        private static bool IsCharAtSearchCoords(string[] input, int row, int col, int diffRow, int diffCol, char searchChar)
        {
            var searchCol = col + diffCol;
            var searchRow = row + diffRow;

            return input.IsInBounds(searchCol, searchRow) && input[searchRow][searchCol] == searchChar;
        }

        private string[] GetInput()
        {
            return FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
        }
    }
}
