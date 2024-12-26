using AdventOfCodeCommon;
using AdventOfCodeCommon.Models;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

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
            // Plan:
            // 1. Use Dijkstra's to work out distances from END of the map.
            // 2. Then, for each non-wall spaces, look at squares dist of 2 away (1 is useless). All these squares can be reached through cheats
            // 3. Then, work out save saved when moving to these new spaces (accounting for the 2 cheated moves)
            //      OLD TIME: dist[current]
            //      NEW TIME: dist[newSpace] + 2
            //      DIFF IN TIME: (dist[current]) - (dist[newSpace] + 2)
            // 4. Increment "Super cheat" count if we save at least 100 pico secs
            
            var input = GetInput();
            var reachableCoords = GetReachableCoords(input);
            var startingPoint = GetPositionOfChar(input, 'S');
            var endingPoint = GetPositionOfChar(input, 'E');
            var dist = reachableCoords.ToDictionary(x => x, _ => int.MaxValue);
            var prev = reachableCoords.ToDictionary(x => x, _ => ((int, int)?)null);

            var remainingCoords = reachableCoords.ToHashSet();

            dist[endingPoint] = 0;

            // Apply Dijkstra's
            while (remainingCoords.Count > 0)
            {
                var position = remainingCoords.OrderBy(x => dist[x]).First();
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
                    var tempDist = dist[position] == int.MaxValue ? int.MaxValue : dist[position] + 1;
                    if (tempDist < dist[neighbourPosition])
                    {
                        dist[neighbourPosition] = tempDist;
                        prev[neighbourPosition] = position;
                    }
                }
            }

            // A "Super cheat" is a cheat that saves more than 100 picoseconds
            var superCheatCount = 0;

            // Now, for each reachable coord, scout out spaces 2 dist away, and work out timeSave.
            foreach (var cheatStartPosition in reachableCoords)
            {
                foreach (var (twoDiffRow, twoDiffCol) in TwoDiffValues)
                {
                    var cheatEndRow = cheatStartPosition.Item1 + twoDiffRow;
                    var cheatEndCol = cheatStartPosition.Item2 + twoDiffCol;

                    // Check if out of bounds
                    if (cheatEndRow < 0 || cheatEndRow >= input.Length || cheatEndCol < 0 || cheatEndCol >= input[0].Length)
                        continue;

                    var cheatEndPosition = (cheatEndRow, cheatEndCol);

                    // Needs to be still in reachableCoords
                    if (!reachableCoords.Contains(cheatEndPosition))
                        continue;

                    // DIFF IN TIME: (dist[current]) - (dist[newSpace] + 2)
                    var timeSave = dist[cheatStartPosition] - (dist[cheatEndPosition] + 2);

                    if (timeSave >= 100) superCheatCount++;
                }
            }

            return superCheatCount.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();
            var reachableCoords = GetReachableCoords(input);
            var startingPoint = GetPositionOfChar(input, 'S');
            var endingPoint = GetPositionOfChar(input, 'E');
            var dist = reachableCoords.ToDictionary(x => x, _ => int.MaxValue);
            var prev = reachableCoords.ToDictionary(x => x, _ => ((int, int)?)null);

            var remainingCoords = reachableCoords.ToHashSet();

            dist[endingPoint] = 0;

            // Apply Dijkstra's
            while (remainingCoords.Count > 0)
            {
                var position = remainingCoords.OrderBy(x => dist[x]).First();
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
                    var tempDist = dist[position] == int.MaxValue ? int.MaxValue : dist[position] + 1;
                    if (tempDist < dist[neighbourPosition])
                    {
                        dist[neighbourPosition] = tempDist;
                        prev[neighbourPosition] = position;
                    }
                }
            }

            // A "Super cheat" is a cheat that saves more than 100 picoseconds
            var superCheatCount = 0;

            foreach (var cheatStartPosition in reachableCoords)
            {
                // Now, do a WHERE to find those within a certain dist
                Console.WriteLine(cheatStartPosition);

                var reachableCoordsWithin20WithDist = reachableCoords.Where(x =>
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
