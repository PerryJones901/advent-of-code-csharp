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
                var searchRow = currentAt.Value.Item1;
                var searchCol = currentAt.Value.Item2;

                var charsToMove = new List<char>(['@']);

                while (true)
                {
                    searchRow += diffValue.Item1;
                    searchCol += diffValue.Item2;

                    var searchChar = scene.Grid[searchRow][searchCol];

                    if (searchChar == '#')
                    {
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
            // TOO LOW: 1505698
            // INCORRECT: 1510000
            // TOO HIGH: 1600000

            // Using checker, example should give: 1512860
            var input = GetInput();
            var scene = GetScene(input, isPart2: true);
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
                var searchRow = currentAt.Value.Item1;
                var searchCol = currentAt.Value.Item2;

                if (moveDirection is Direction.Up || moveDirection is Direction.Down)
                {
                    var colToStartRowAndCharsToMove = new Dictionary<int, (int, List<char>)>
                    {
                        [currentAt.Value.Item2] = (currentAt.Value.Item1, new List<char>(['@']))
                    };

                    var state = new ProcessState();
                    ProcessVerticalMoveAttempt(
                        searchRow + diffValue.Item1,
                        searchCol + diffValue.Item2,
                        diffValue,
                        scene.Grid,
                        colToStartRowAndCharsToMove,
                        state
                    );

                    if (state.IsMoveBlockedByWall) continue;

                    // Now, we have all chars to move in colToStartRowAndCharsToMove
                    foreach (var kvp in colToStartRowAndCharsToMove)
                    {
                        var col = kvp.Key;
                        var startRow = kvp.Value.Item1;
                        var charsToMove = kvp.Value.Item2;

                        // There's space for all chars thus far to move one unit in the direction

                        // First, replace start with '.'
                        scene.Grid[startRow][col] = '.';

                        for (int i = 0; i < charsToMove.Count; i++)
                        {
                            var replaceCharRow = startRow + (i + 1) * diffValue.Item1;

                            scene.Grid[replaceCharRow][col] = charsToMove[i];
                        }
                    }

                    currentAt = (currentAt.Value.Item1 + diffValue.Item1, currentAt.Value.Item2 + diffValue.Item2);
                }
                else
                {
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
                        else if (searchChar == '[' || searchChar == ']')
                        {
                            charsToMove.Add(searchChar);
                        }
                        else
                        {
                            throw new Exception("Unexpected char");
                        }
                    }
                }
            }

            var gpsSum = 0;

            for (int row = 1; row < scene.Grid.Length - 1; row++)
            {
                for (int col = 1; col < scene.Grid[0].Length - 1; col++)
                {
                    if (scene.Grid[row][col] != '[') continue;

                    gpsSum += row * 100 + col;
                }
            }

            return gpsSum.ToString();
        }

        private void ProcessVerticalMoveAttempt(int row, int col, (int, int) diffValue, char[][] grid, Dictionary<int, (int, List<char>)> colToRowAndCharsToMove, ProcessState state)
        {
            // Assumption: Just handle vertical movements (hoping horizontal movements are trivial)
            var spaceChar = grid[row][col];

            if (state.IsMoveBlockedByWall || spaceChar == '.')
                return;

            if (spaceChar == '#')
            {
                state.IsMoveBlockedByWall = true;
                return;
            }

            if (!colToRowAndCharsToMove.ContainsKey(col))
                colToRowAndCharsToMove[col] = (row, new List<char>());

            colToRowAndCharsToMove[col].Item2.Add(spaceChar);

            var colOfOtherSideOfBox = spaceChar == '[' ? col + 1 : col - 1;
            if (!colToRowAndCharsToMove.ContainsKey(colOfOtherSideOfBox))
                ProcessVerticalMoveAttempt(row, colOfOtherSideOfBox, diffValue, grid, colToRowAndCharsToMove, state);

            // Now, continue chain for self
            // We can assume key for row exists
            ProcessVerticalMoveAttempt(row + diffValue.Item1, col + diffValue.Item2, diffValue, grid, colToRowAndCharsToMove, state);
        }

        private class Scene
        {
            public required char[][] Grid { get; set; }
            public required List<Direction> Moves { get; set; }
        }

        private class ProcessState
        {
            public bool IsMoveBlockedByWall { get; set; } = false;
        }

        private Scene GetScene(string input, bool isPart2 = false)
        {
            var gridFlatString = input.Split($"{NEW_LINE}{NEW_LINE}")[0];
            var directionChars = input.Split($"{NEW_LINE}{NEW_LINE}")[1].Replace(NEW_LINE, "");

            var grid = gridFlatString
                .Split(NEW_LINE)
                .Select(x => x
                    .ToCharArray()
                    .SelectMany(x => isPart2 ? CharToPart2Chars(x).ToCharArray() : [x])
                    .ToArray()
                ).ToArray();
            var directionList = directionChars.Select(GetDirectionFromChar).ToList();

            return new Scene
            {
                Grid = grid,
                Moves = directionList,
            };
        }

        private static string CharToPart2Chars(char c)
        {
            return c switch
            {
                '#' => "##",
                'O' => "[]",
                '.' => "..",
                '@' => "@.",
                _ => throw new Exception("Char isn't a valid input"),
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
