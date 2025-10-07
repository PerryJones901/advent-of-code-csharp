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
            var gridOfBytes = new bool[GRID_DIMENSION, GRID_DIMENSION];

            foreach (var (row, col) in input)
            {
                gridOfBytes[row, col] = true;
            }

            var coordsToConnectedCompId = new Dictionary<(int, int), int>();

            var connectedCompId = 0;

            for (var i = 0; i < GRID_DIMENSION; i++)
            {
                for (var j = 0; j < GRID_DIMENSION; j++)
                {
                    var coords = (i, j);

                    if (gridOfBytes[i, j])
                        continue; // Wall (byte fallen)
                    if (coordsToConnectedCompId.ContainsKey(coords))
                        continue; // Already processed

                    Process(coords, connectedCompId, gridOfBytes, coordsToConnectedCompId);
                    connectedCompId++;
                }
            }

            var inputReversed = input.Reverse().ToArray();

            foreach (var byteCoords in inputReversed)
            {
                // Remove byte and check connected comps
                gridOfBytes[byteCoords.Item1, byteCoords.Item2] = false;

                // Only need to check 4 directions
                // Get all ids of neighbour squares. Next, reassign all ids to the smallest id out of the lot
                var setOfNeighbourCompIds = new HashSet<int>();

                foreach (var diffValue in DiffValues)
                {
                    var neighbourCoords = (byteCoords.Item1 + diffValue.Item1, byteCoords.Item2 + diffValue.Item2);

                    if (coordsToConnectedCompId.TryGetValue(neighbourCoords, out var neighbourCompId))
                        setOfNeighbourCompIds.Add(neighbourCompId);
                }

                var minCompId = setOfNeighbourCompIds.Count > 0 ? setOfNeighbourCompIds.Min() : connectedCompId++;
                var affectedCoords = coordsToConnectedCompId
                    .Where(x => setOfNeighbourCompIds.Contains(x.Value))
                    .Select(x => x.Key)
                    .Concat([byteCoords])
                    .ToArray();

                foreach (var coords in affectedCoords)
                {
                    coordsToConnectedCompId[coords] = minCompId;
                }

                // Check if start and end are connected
                if (coordsToConnectedCompId.TryGetValue((0, 0), out var startCompId) &&
                    coordsToConnectedCompId.TryGetValue((GRID_DIMENSION - 1, GRID_DIMENSION - 1), out var endCompId) &&
                    startCompId == endCompId)
                {
                    return $"{byteCoords.Item1},{byteCoords.Item2}";
                }
            }

            return "";
        }

        private static void Process((int, int) coords, int connectedCompId, bool[,] grid, Dictionary<(int, int), int> coordsToConnectedCompId)
        {
            coordsToConnectedCompId[coords] = connectedCompId;

            foreach (var diffValue in DiffValues)
            {
                var neighbourCoords = (coords.Item1 + diffValue.Item1, coords.Item2 + diffValue.Item2);
                if (neighbourCoords.Item1 < 0 || neighbourCoords.Item1 >= GRID_DIMENSION || neighbourCoords.Item2 < 0 || neighbourCoords.Item2 >= GRID_DIMENSION)
                    continue;

                if (grid[neighbourCoords.Item1, neighbourCoords.Item2])
                    continue; // Wall (byte fallen)

                if (coordsToConnectedCompId.ContainsKey(neighbourCoords))
                    continue;

                Process(neighbourCoords, connectedCompId, grid, coordsToConnectedCompId);
            }
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
