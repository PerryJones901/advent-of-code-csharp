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
            var reindeerStateToScore = GetReindeerStateToScoreMap(input);
            var positionOfE = GetPositionOfChar(input, charToFind: 'E');
            var minScore = GetMinScoreAtPosition(reindeerStateToScore, positionOfE);

            return minScore.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();
            var reindeerStateToScore = GetReindeerStateToScoreMap(input);
            var positionOfE = GetPositionOfChar(input, charToFind: 'E');
            var minScore = GetMinScoreAtPosition(reindeerStateToScore, positionOfE);

            var statesAtE = reindeerStateToScore.Where(x => x.Key.Position == positionOfE).Select(x => x.Key);
            var statesAtEWithBestScore = statesAtE.Where(x => reindeerStateToScore[x] == minScore);

            var queue = new Queue<ReindeerState>(statesAtEWithBestScore);
            var bestStates = new HashSet<ReindeerState>(statesAtEWithBestScore);

            while (queue.Count > 0)
            {
                var state = queue.Dequeue();
                var currentScore = reindeerStateToScore[state];

                var rotationTargetScore = currentScore - 1000;
                var moveTargetScore = currentScore - 1;

                var statesWithScoresToCheck = new[] {
                    (state: state.ToClockwise(), targetScore: rotationTargetScore),
                    (state: state.ToAntiClockwise(), targetScore: rotationTargetScore),
                    (state: state.ToMoveBackward(), targetScore: moveTargetScore),
                };

                foreach (var (stateToCheck, targetScore) in statesWithScoresToCheck)
                {
                    if (
                        reindeerStateToScore.TryGetValue(stateToCheck, out int storedScore)
                        && storedScore == targetScore
                        && !bestStates.Contains(stateToCheck))
                    {
                        bestStates.Add(stateToCheck);
                        queue.Enqueue(stateToCheck);
                    }
                }
            }

            var bestSpacesCount = bestStates
                .Select(x => x.Position)
                .Distinct()
                .Count();

            return bestSpacesCount.ToString();
        }

        private static Dictionary<ReindeerState, int> GetReindeerStateToScoreMap(char[][] input)
        {
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

                var newScoreWhenRotated = currentScore + 1000;
                var statesToCheck = new List<ReindeerState>([
                    reindeer.ToClockwise(),
                    reindeer.ToAntiClockwise()
                ]);

                foreach (var state in statesToCheck)
                {
                    if (!reindeerStateToScore.TryGetValue(state, out int storedScore) || newScoreWhenRotated < storedScore)
                    {
                        reindeerStateToScore[state] = newScoreWhenRotated;
                        prioQueue.Enqueue(state, newScoreWhenRotated);
                    }
                }

                // Now, check if in front of us is a '.'. If so, we can move forward.
                var reindeerStateMoveForward = reindeer.ToMoveForward();
                var newScoreWhenMovedForward = currentScore + 1;
                var charInFront = input[reindeerStateMoveForward.Position.Item1][reindeerStateMoveForward.Position.Item2];

                if (
                    (charInFront == '.' || charInFront == 'E')
                    && (!reindeerStateToScore.TryGetValue(reindeerStateMoveForward, out int storedScoreMove) || newScoreWhenMovedForward < storedScoreMove)
                )
                {
                    reindeerStateToScore[reindeerStateMoveForward] = newScoreWhenMovedForward;

                    if (charInFront == '.')
                        prioQueue.Enqueue(reindeerStateMoveForward, newScoreWhenMovedForward);
                }
            }

            return reindeerStateToScore;
        }

        private static int GetMinScoreAtPosition(
            Dictionary<ReindeerState, int> reindeerStateToScore,
            (int, int) position)
        {
            return reindeerStateToScore
                .Where(x => x.Key.Position == position)
                .Select(x => x.Value)
                .Min();
        }

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
                for (int col = 0; col < grid[0].Length; col++)
                    if (grid[row][col] == charToFind)
                        return (row, col);

            throw new Exception($"Char '{charToFind}' could not be found.");
        }

        private char[][] GetInput()
            => FileInputAssistant.GetStringArrayFromFile(TextInputFilePath)
                .Select(x => x.ToCharArray())
                .ToArray();
    }
}
