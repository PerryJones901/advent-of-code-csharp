using AdventOfCodeCommon;
using AdventOfCodeCommon.Models;
using System.Collections.ObjectModel;

namespace AdventOfCode2024.Days
{
    internal class Day15(bool isTest) : DayBase(15, isTest)
    {
        private const string NEW_LINE = "\r\n";
        private static readonly ReadOnlyDictionary<Direction, (int, int)> DirectionToDiffValue = new Dictionary<Direction, (int, int)>
        {
            { Direction.Up, (-1, 0) },
            { Direction.Right, (0, 1) },
            { Direction.Down, (1, 0) },
            { Direction.Left, (0, -1) }
        }.AsReadOnly();

        public override string Part1()
        {
            var input = GetInput();
            var scene = GetScene(input);
            (int, int)? currentAt = null;

            for (int row = 0; row < scene.Grid.Length; row++)
            {
                for (int col = 0; col < scene.Grid[0].Length; col++)
                {
                    if (scene.Grid[row][col] != '@')
                        continue;

                    currentAt = (row, col);
                    break;
                }
            }

            if (currentAt is null)
                throw new Exception("CurrentAt unassigned");

            foreach (var moveDirection in scene.Moves)
            {
                var diffValue = DirectionToDiffValue[moveDirection];
                var charsUntilDot = 0;
                var searchRow = currentAt.Value.Item1;
                var searchCol = currentAt.Value.Item2;

                var charsToMove = new List<char>(['@']);

                while (true)
                {
                    // Move in direction
                    searchRow += diffValue.Item1;
                    searchCol += diffValue.Item2;

                    var searchChar = scene.Grid[searchRow][searchCol];

                    if (searchChar == '#')
                    {
                        // Wall obstructs movement - break.
                        break;
                    }
                    else if (searchChar == '.')
                    {
                        // There's space for all chars thus far to move one unit in the direction

                        // First, replace '@' with '.'
                        scene.Grid[currentAt.Value.Item1][currentAt.Value.Item2] = '.';

                        for (int i = 0; i < charsToMove.Count; i++)
                        {
                            var replaceCharRow = currentAt.Value.Item1 + (i + 1) * diffValue.Item1;
                            var replaceCharCol = currentAt.Value.Item2 + (i + 1) * diffValue.Item2;

                            scene.Grid[replaceCharRow][replaceCharCol] = charsToMove[i];
                        }

                        currentAt = (currentAt.Value.Item1 + diffValue.Item1, currentAt.Value.Item2 + diffValue.Item2);

                        break;
                    }
                    else if (searchChar == 'O')
                    {
                        charsToMove.Add(searchChar);
                    }
                    else
                    {
                        throw new Exception("Unexpected char");
                    }

                    charsUntilDot++;
                }
            }

            var gpsSum = 0;

            for (int row = 1; row < scene.Grid.Length - 1; row++)
            {
                for (int col = 1; col < scene.Grid[0].Length - 1; col++)
                {
                    if (scene.Grid[row][col] != 'O') continue;

                    gpsSum += row * 100 + col;
                }
            }

            return gpsSum.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();

            return "Part 2";
        }

        private class Scene
        {
            public required char[][] Grid { get; set; }
            public required List<Direction> Moves { get; set; }
        }

        private Scene GetScene(string input)
        {
            var gridFlatString = input.Split($"{NEW_LINE}{NEW_LINE}")[0];
            var directionChars = input.Split($"{NEW_LINE}{NEW_LINE}")[1].Replace(NEW_LINE, "");

            var grid = gridFlatString.Split(NEW_LINE).Select(x => x.ToCharArray()).ToArray();
            var directionList = directionChars.Select(GetDirectionFromChar).ToList();

            return new Scene
            {
                Grid = grid,
                Moves = directionList,
            };
        }

        private Direction GetDirectionFromChar(char directionChar)
        {
            return directionChar switch
            {
                '^' => Direction.Up,
                '>' => Direction.Right,
                'v' => Direction.Down,
                '<' => Direction.Left,
                _ => throw new Exception("Char isn't a valid direction"),
            };
        }

        private string GetInput()
        {
            return FileInputAssistant.GetStringFromFile(TextInputFilePath);
        }
    }
}
