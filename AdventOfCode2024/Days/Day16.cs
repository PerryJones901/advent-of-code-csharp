using AdventOfCodeCommon;
using AdventOfCodeCommon.Directions;
using AdventOfCodeCommon.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AdventOfCode2024.Days
{
    internal class Day16(bool isTest) : DayBase(16, isTest)
    {
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
            var positionOfS = GetPositionOfChar(input, charToFind: 'S');
            var initialState = new ReindeerState
            {
                Position = positionOfS,
                Direction = Direction.Right,
            };

            var reindeerStateToScore = new Dictionary<ReindeerState, int> {
                { initialState, 0 }
            };

            var prioQueue = new PriorityQueue<ReindeerState, int>();
            prioQueue.Enqueue(initialState, 0);

            while (prioQueue.Count > 0)
            {
                var reindeer = prioQueue.Dequeue();
                var currentScore = reindeerStateToScore[reindeer];

                // Now, find all possible actions, compare with their values in dict, and if different, add to queue.
                // First: rotate 90CW

                var reindeerStateCW = reindeer.ToClockwise();
                var reindeerStateACW = reindeer.ToAntiClockwise();
                var newScore = currentScore + 1000;

                if (!reindeerStateToScore.TryGetValue(reindeerStateCW, out int storedScoreCW) || newScore < storedScoreCW)
                {
                    // We have a better score. Upsert it in dict and add to queue.
                    reindeerStateToScore[reindeerStateCW] = newScore;
                    prioQueue.Enqueue(reindeerStateCW, newScore);
                }

                if (!reindeerStateToScore.TryGetValue(reindeerStateACW, out int storedScoreACW) || newScore < storedScoreACW)
                {
                    // We have a better score. Upsert it in dict and add to queue.
                    reindeerStateToScore[reindeerStateACW] = newScore;
                    prioQueue.Enqueue(reindeerStateACW, newScore);
                }

                // Now, check if in front of us is a '.'. If so, we can move forward.
                var reindeerStateMoveForward = reindeer.ToMoveForward();
                var newScoreForMove = currentScore + 1;
                var charInFront = input[reindeerStateMoveForward.Position.Item1][reindeerStateMoveForward.Position.Item2];

                if (charInFront == '.' || charInFront == 'E')
                {
                    // We can move forward. Now to check...
                    if (!reindeerStateToScore.TryGetValue(reindeerStateMoveForward, out int storedScoreMove) || newScoreForMove < storedScoreMove)
                    {
                        // We have a better score. Upsert it in dict and add to queue.
                        reindeerStateToScore[reindeerStateMoveForward] = newScoreForMove;

                        // Only add to queue if it's a '.'

                        if (charInFront == '.')
                            prioQueue.Enqueue(reindeerStateMoveForward, newScoreForMove);
                    }
                }
            }

            var positionOfE = GetPositionOfChar(input, charToFind: 'E');
            var minScore = reindeerStateToScore.Where(x => x.Key.Position == positionOfE).Select(x => x.Value).Min();

            return minScore.ToString();
        }

        public override string Part2()
        {
            return "";
        }

        /*
         * Need to revisit Part 2.
         *
         */
        //private static bool IsOnBestPath(
        //    ReindeerState state,
        //    int searchScore,
        //    HashSet<(int, int)> tilesOnABestPath,
        //    char[][] input,
        //    Dictionary<ReindeerState, int> statesToScore,
        //    (int, int) endCoords)
        //{
        //    // Check all 3 possible paths. If at least one returns true, it's linked to the end, add to set.

        //    // 0. Need to have good score.
        //    if (searchScore != statesToScore[state]) return false;

        //    // 1. If state's position is already on best path, return true
        //    if (tilesOnABestPath.Contains(state.Position)) return true;

        //    // 2. If at end with smallest score, return true.
        //    if (searchScore != statesToScore[state]) return false;

        //    // 3. Otherwise, call function iteratively
        //    var forwardState = state.ToMoveForward();
        //    var cwState = state.ToClockwise();
        //    var acwState = state.ToAntiClockwise();

        //    if (tilesOnABestPath.Contains(forwardState.Position))
        //    {
        //        tilesOnABestPath.Add(state.Position);
        //        return true;
        //    }
        //}

        [DebuggerDisplay("Pos=({Position}), Dir=({Direction})")]
        private struct ReindeerState
        {
            public required (int, int) Position { get; set; }
            public required Direction Direction { get; set; }

            public ReindeerState ToClockwise()
            {
                return new ReindeerState
                {
                    Position = Position,
                    Direction = Direction.ToClockwiseDirection(),
                };
            }

            public ReindeerState ToAntiClockwise()
            {
                return new ReindeerState
                {
                    Position = Position,
                    Direction = Direction.ToAntiClockwiseDirection(),
                };
            }

            public ReindeerState ToMoveForward()
            {
                var diffValue = DirectionToDiffValue[Direction];

                return new ReindeerState
                {
                    Position = (Position.Item1 + diffValue.Item1, Position.Item2 + diffValue.Item2),
                    Direction = Direction,
                };
            }

            public ReindeerState ToMoveBackward()
            {
                var diffValue = DirectionToDiffValue[Direction];

                return new ReindeerState
                {
                    Position = (Position.Item1 - diffValue.Item1, Position.Item2 - diffValue.Item2),
                    Direction = Direction,
                };
            }
        }

        private static (int, int) GetPositionOfChar(char[][] grid, char charToFind)
        {
            for (int row = 0; row < grid.Length; row++)
            {
                for (int col = 0; col < grid[0].Length; col++)
                {
                    if (grid[row][col] == charToFind)
                        return (row, col);
                }
            }

            throw new Exception($"Char '{charToFind}' could not be found.");
        }

        private char[][] GetInput()
        {
            var inputAsStringArray = FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
            var charArrays = inputAsStringArray.Select(x => x.ToCharArray()).ToArray();

            return charArrays;
        }
    }
}
