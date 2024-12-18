using AdventOfCodeCommon;
using System.Collections.ObjectModel;

namespace AdventOfCode2024.Days
{
    internal class Day18(bool isTest) : DayBase(18, isTest)
    {
        private const int GRID_DIMENSION = 71;
        private static readonly ReadOnlyCollection<(int, int)> DiffValues = new List<(int, int)>
        {
            (-1, 0),
            (0, 1),
            (1, 0),
            (0, -1),
        }.AsReadOnly();

        public override string Part1()
        {
            var input = GetInput();
            var reachableCoords = GetReachableCoords(input, takeFirstCount: 1024);
            var dist = reachableCoords.ToDictionary(x => x, _ => int.MaxValue);
            var prev = reachableCoords.ToDictionary(x => x, _ => ((int, int)?)null);

            var remainingCoords = reachableCoords.ToHashSet();
            dist[(0, 0)] = 0;

            while (remainingCoords.Count > 0)
            {
                var position = remainingCoords.OrderBy(x => dist[x]).First();
                remainingCoords.Remove(position);

                foreach (var (diffRow, diffCol) in DiffValues)
                {
                    var neighbourRow = position.Item1 + diffRow;
                    var neighbourCol = position.Item2 + diffCol;

                    // Check if out of bounds
                    if (neighbourRow < 0 || neighbourRow >= GRID_DIMENSION || neighbourCol < 0 || neighbourCol >= GRID_DIMENSION)
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

            var distToEnd = dist[(GRID_DIMENSION - 1, GRID_DIMENSION - 1)];

            return distToEnd.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();
            var reachableCoords = GetReachableCoords(input, takeFirstCount: 0);
            var dist = reachableCoords.ToDictionary(x => x, _ => int.MaxValue);
            var prev = reachableCoords.ToDictionary(x => x, _ => ((int, int)?)null);
            var remainingCoords = reachableCoords.ToHashSet();

            dist[(0, 0)] = 0;

            foreach (var newWall in input)
            {
                // Reset all affected positions
                var queueOnPath = new Queue<(int, int)>([newWall]);
                while (queueOnPath.Count > 0)
                {
                    var coordsOnPath = queueOnPath.Dequeue();
                    dist[coordsOnPath] = int.MaxValue;
                    prev[coordsOnPath] = null;

                    var affectedNeighbours = prev.Where(x => x.Value == coordsOnPath).Select(x => x.Key);

                    foreach (var affectedNeighbour in affectedNeighbours)
                    {
                        remainingCoords.Add(affectedNeighbour);
                        queueOnPath.Enqueue(affectedNeighbour);
                    }
                }

                // Now, remove the new wall
                dist.Remove(newWall);
                prev.Remove(newWall);

            //    while (remainingCoords.Count > 0)
            //    {
            //        var position = remainingCoords.OrderBy(x => dist[x]).First();
            //        remainingCoords.Remove(position);

            //        foreach (var (diffRow, diffCol) in DiffValues)
            //        {
            //            var neighbourRow = position.Item1 + diffRow;
            //            var neighbourCol = position.Item2 + diffCol;

            //            // Check if out of bounds
            //            if (neighbourRow < 0 || neighbourRow >= GRID_DIMENSION || neighbourCol < 0 || neighbourCol >= GRID_DIMENSION)
            //                continue;

            //            var neighbourPosition = (neighbourRow, neighbourCol);

            //            // Needs to be still in remainingCoords
            //            if (!remainingCoords.Contains(neighbourPosition))
            //                continue;

            //            // Decide if distance is shorter
            //            var tempDist = dist[position] == int.MaxValue ? int.MaxValue : dist[position] + 1;
            //            if (tempDist < dist[neighbourPosition])
            //            {
            //                dist[neighbourPosition] = tempDist;
            //                prev[neighbourPosition] = position;
            //            }
            //        }
            //    }

            //    var distToEnd = dist[(GRID_DIMENSION - 1, GRID_DIMENSION - 1)];
            }

            return "";
        }

        private static HashSet<(int, int)> GetReachableCoords(IEnumerable<(int, int)> input, int takeFirstCount)
        {
            var grid = Enumerable.Range(0, GRID_DIMENSION)
                .SelectMany(row => Enumerable.Range(0, GRID_DIMENSION).Select(col => (row, col)).ToArray());
            var set = new HashSet<(int, int)>(grid);

            var byteCoords = input.Take(takeFirstCount);
            set.RemoveWhere(x => byteCoords.Contains(x));

            return set;
        }

        private IEnumerable<(int, int)> GetInput()
        {
            return FileInputAssistant.GetStringArrayFromFile(TextInputFilePath).Select(x =>
                (int.Parse(x.Split(",")[0]), int.Parse(x.Split(",")[1]))
            );
        }
    }
}
