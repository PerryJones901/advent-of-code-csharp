using AdventOfCodeCommon;
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
            var width = input[0].Length;
            var height = input.Length;
            var searchValueCount = 0;

            foreach(var (row, col) in GetGridCoords(width, height))
            {
                foreach (var (diffRow, diffCol) in DiffValues)
                {
                    var wordFound = true;

                    for (int charIndex = 0; charIndex < SearchValue.Length; charIndex++)
                    {
                        var searchCol = col + diffCol * charIndex;
                        var searchRow = row + diffRow * charIndex;
                        if (!IsInBounds(searchCol, searchRow, width, height) || input[searchRow][searchCol] != SearchValue[charIndex])
                        {
                            wordFound = false;
                            break;
                        }
                    }

                    if (wordFound)
                        searchValueCount++;
                }
            }

            return searchValueCount.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();
            var width = input[0].Length;
            var height = input.Length;
            var count = 0;
            var indicesOfM = new HashSet<int>();
            var invalidCase = false;

            foreach (var (row, col) in GetGridCoords(width, height, ignoreEdges: true))
            {
                if (input[row][col] != 'A') continue;

                for (int m = 0; m < DiffValues.Count; m++)
                {
                    // Ignore non diagonal directions (took me way too long to realise this)
                    if (m % 2 == 0)
                        continue;

                    var checkCases = new[] {
                        new { Char = 'M', DiffCoords = DiffValues[m] },
                        new { Char = 'S', DiffCoords = DiffValues[(m + 4) % DiffValues.Count] },
                    };

                    invalidCase = false;
                    foreach (var checkCase in checkCases)
                    {
                        var (diffRow, diffCol) = checkCase.DiffCoords;
                        var searchCol = col + diffCol;
                        var searchRow = row + diffRow;
                        if (!IsInBounds(searchCol, searchRow, width, height) || input[searchRow][searchCol] != checkCase.Char)
                        {
                            invalidCase = true;
                            break;
                        }
                    }

                    if (!invalidCase)
                        indicesOfM.Add(m);
                }

                foreach (var m in indicesOfM)
                {
                    // Check if we also have a MAS a 90deg-CW turn away
                    if (indicesOfM.Contains((m + 2) % DiffValues.Count)) count++;
                }

                indicesOfM.Clear();
            }

            return count.ToString();
        }

        private static IEnumerable<(int, int)> GetGridCoords(int width, int height, bool ignoreEdges = false)
        {
            var offset = ignoreEdges ? 1 : 0;

            for (int i = offset; i < height - offset; i++)
            {
                for (int j = offset; j < width - offset; j++)
                {
                    yield return (i, j);
                }
            }
        }

        private static bool IsInBounds(int x, int y, int width, int height)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }

        private static bool CharFoundAtCoords(string[] input, int row, int col, int diffRow, int diffCol, char searchChar)
        {
            var searchCol = col + diffCol;
            var searchRow = row + diffRow;

            return IsInBounds(searchCol, searchRow, input[0].Length, input.Length) && input[searchRow][searchCol] == searchChar;
        }

        private string[] GetInput()
        {
            return FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
        }
    }
}
