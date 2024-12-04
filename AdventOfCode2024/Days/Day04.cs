using AdventOfCodeCommon;

namespace AdventOfCode2024.Days
{
    internal class Day04(bool isTest) : DayBase(4, isTest)
    {
        private readonly IReadOnlyList<(int, int)> DiffValues = new List<(int, int)>
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
        private const string SearchValue = "XMAS";

        public override string Part1()
        {
            var input = GetInput();
            var width = input[0].Length;
            var height = input.Length;
            var count = 0;

            for(int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (input[i][j] != SearchValue[0]) continue;

                    foreach (var (diffX, diffY) in DiffValues)
                    {
                        var found = true;
                        for (int k = 1; k < SearchValue.Length; k++)
                        {
                            var x = j + diffX * k;
                            var y = i + diffY * k;
                            if (!IsInBounds(x, y, width, height) || input[y][x] != SearchValue[k])
                            {
                                found = false;
                                break;
                            }
                        }
                        if (found)
                            count++;
                    }
                }
            }

            return count.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();
            var width = input[0].Length;
            var height = input.Length;
            var count = 0;
            var indicesOfM = new List<int>();

            // Don't need to search on edges
            for (int i = 1; i < height - 1; i++)
            {
                for (int j = 1; j < width - 1; j++)
                {
                    if (input[i][j] != 'A') continue;

                    for (int m = 0; m < DiffValues.Count; m++)
                    {
                        // Remove non diagonal directions
                        if (m % 2 == 0)
                            continue;

                        // M check
                        var (diffX, diffY) = DiffValues[m];
                        if (!IsInBounds(i + diffX, j + diffY, width, height) || input[i + diffX][j + diffY] != 'M')
                            continue;

                        // S check
                        var (diffX2, diffY2) = DiffValues[(m + 4) % DiffValues.Count];
                        if (!IsInBounds(i + diffX2, j + diffY2, width, height) || input[i + diffX2][j + diffY2] != 'S')
                            continue;

                        indicesOfM.Add(m);
                    }

                    foreach (var m in indicesOfM)
                    {
                        // Check if we also have a MAS a 90deg-CW turn away
                        if (indicesOfM.Contains((m + 2) % DiffValues.Count)) count++;
                    }

                    indicesOfM.Clear();
                }
            }

            return count.ToString();
        }

        private static bool IsInBounds(int x, int y, int width, int height)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }

        private string[] GetInput()
        {
            return FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
        }
    }
}
