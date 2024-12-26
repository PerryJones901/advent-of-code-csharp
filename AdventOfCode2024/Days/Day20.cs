using AdventOfCodeCommon;
using System.Collections.ObjectModel;

namespace AdventOfCode2024.Days
{
    internal class Day20(bool isTest) : DayBase(20, isTest)
    {
        private static readonly ReadOnlyCollection<(int, int)> DiffValues = new List<(int, int)>
        {
            (-1, 0),
            (0, 1),
            (1, 0),
            (0, -1),
        }.AsReadOnly();

        private static readonly ReadOnlyCollection<(int, int)> TwoDiffValues = new List<(int, int)>
        {
            (-2, 0),
            (-1, 1),
            (0, 2),
            (1, 1),
            (2, 0),
            (1, -1),
            (0, -2),
            (-1, -1),
        }.AsReadOnly();

        public override string Part1()
        {
            var input = GetInput();
            var dijkstrasResult = GetDijkstrasResult(input);
            var reachableSpaces = dijkstrasResult.ReachableSpaces;
            var dist = dijkstrasResult.SpaceToDistanceFromEnd;

            // Define "Super cheat" as a cheat which saves more than 100 pico secs
            var superCheatCount = 0;

            foreach (var cheatStartPosition in reachableSpaces)
            {
                foreach (var (twoDiffRow, twoDiffCol) in TwoDiffValues)
                {
                    var cheatEndRow = cheatStartPosition.Item1 + twoDiffRow;
                    var cheatEndCol = cheatStartPosition.Item2 + twoDiffCol;

                    // OOB Check
                    if (cheatEndRow < 0 || cheatEndRow >= input.Length || cheatEndCol < 0 || cheatEndCol >= input[0].Length)
                        continue;

                    var cheatEndPosition = (cheatEndRow, cheatEndCol);

                    // Check if space is unreachable
                    if (!reachableSpaces.Contains(cheatEndPosition))
                        continue;

                    var timeSave = dist[cheatStartPosition] - (dist[cheatEndPosition] + 2);

                    if (timeSave >= 100)
                        superCheatCount++;
                }
            }

            return superCheatCount.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();
            var dijkstrasResult = GetDijkstrasResult(input);
            var reachableSpaces = dijkstrasResult.ReachableSpaces;
            var dist = dijkstrasResult.SpaceToDistanceFromEnd;

            // A "Super cheat" is a cheat that saves more than 100 picoseconds
            var superCheatCount = 0;

            foreach (var cheatStartPosition in reachableSpaces)
            {
                var reachableCoordsWithin20WithDist = reachableSpaces.Where(x =>
                    Math.Abs(x.Item1 - cheatStartPosition.Item1) + Math.Abs(x.Item2 - cheatStartPosition.Item2) <= 20)
                    .ToDictionary(
                        x => x,
                        x => Math.Abs(x.Item1 - cheatStartPosition.Item1) + Math.Abs(x.Item2 - cheatStartPosition.Item2)
                    );

                var cheatedDistDiffs = reachableCoordsWithin20WithDist.ToDictionary(
                    x => x.Key,
                    x => dist[cheatStartPosition] - (x.Value + dist[x.Key]));

                var superCheatCountTemp = cheatedDistDiffs.Count(x => x.Value >= 100);
                superCheatCount += superCheatCountTemp;
            }

            return superCheatCount.ToString();
        }

        private static DijkstrasResult GetDijkstrasResult(char[][] input)
        {
            var reachableSpaces = GetReachableCoords(input);
            var spaceToDistanceFromEnd = reachableSpaces.ToDictionary(x => x, _ => int.MaxValue);

            var endingPoint = GetPositionOfChar(input, 'E');
            spaceToDistanceFromEnd[endingPoint] = 0;

            var remainingCoords = reachableSpaces.ToHashSet();

            // Apply Dijkstra's
            while (remainingCoords.Count > 0)
            {
                var position = remainingCoords.OrderBy(x => spaceToDistanceFromEnd[x]).First();
                remainingCoords.Remove(position);

                foreach (var (diffRow, diffCol) in DiffValues)
                {
                    var neighbourRow = position.Item1 + diffRow;
                    var neighbourCol = position.Item2 + diffCol;

                    // Check if out of bounds
                    if (neighbourRow < 0 || neighbourRow >= input.Length || neighbourCol < 0 || neighbourCol >= input[0].Length)
                        continue;

                    var neighbourPosition = (neighbourRow, neighbourCol);

                    // Needs to be still in remainingCoords
                    if (!remainingCoords.Contains(neighbourPosition))
                        continue;

                    // Decide if distance is shorter
                    var tempDist = spaceToDistanceFromEnd[position] == int.MaxValue
                        ? int.MaxValue
                        : spaceToDistanceFromEnd[position] + 1;

                    if (tempDist < spaceToDistanceFromEnd[neighbourPosition])
                        spaceToDistanceFromEnd[neighbourPosition] = tempDist;
                }
            }

            return new()
            {
                ReachableSpaces = reachableSpaces,
                SpaceToDistanceFromEnd = spaceToDistanceFromEnd
            };
        }

        private class DijkstrasResult
        {
            public required HashSet<(int, int)> ReachableSpaces { get; set; }
            public required Dictionary<(int, int), int> SpaceToDistanceFromEnd { get; set; }
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

        private static HashSet<(int, int)> GetReachableCoords(char[][] grid)
        {
            var reachableCoords = new HashSet<(int, int)>();
            for (int row = 0; row < grid.Length; row++)
                for (int col = 0; col < grid[0].Length; col++)
                    if (grid[row][col] != '#')
                        reachableCoords.Add((row, col));

            return reachableCoords;
        }

        private char[][] GetInput()
        {
            var inputAsStringArray = FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
            var charArrays = inputAsStringArray.Select(x => x.ToCharArray()).ToArray();

            return charArrays;
        }
    }
}
