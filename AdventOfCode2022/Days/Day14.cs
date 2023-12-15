using AdventOfCode2022.Helpers;

namespace AdventOfCode2022.Days
{
    public static class Day14
    {
        public static int Part1Answer() =>
            Part1(GetSeparatedInputFromFile());

        public static int Part2Answer() =>
            Part2(GetSeparatedInputFromFile());

        public static int Part1(List<string> input)
        {
            var rockList = input.Select(GetCornersFromString).ToList();
            // Assuming we start at 0, 0 (i.e. no negatives)
            // Note: X then Y (not the usual Y then X for coords)
            var grid = GetGrid(rockList);

            //foreach(var hello in grid.GetRange(490, 21).Select(x => new string(x.ToArray()))) {
            //    Console.WriteLine(hello);
            //}
            return SandsAtRestBeforeAbyss(grid);
        }

        public static int Part2(List<string> input)
        {
            var rockList = input.Select(GetCornersFromString).ToList();
            var grid = GetGrid(rockList);

            return SandsAtRestWhenFull(grid);
        }

        private static List<(int, int)> GetCornersFromString(string input)
            => input
                .Split(" -> ")
                .Select(x => (
                    int.Parse(x.Split(",")[0]),
                    int.Parse(x.Split(",")[1])
                )).ToList();

        private static List<List<char>> GetGrid(List<List<(int, int)>> rockList)
        {
            //var width = rockList.SelectMany(x => x.Select(x => x.Item1)).Max() + 1;
            var width = rockList.SelectMany(x => x.Select(x => x.Item1)).Max() + 1000;
            //var height = rockList.SelectMany(x => x.Select(x => x.Item2)).Max() + 1;
            var height = rockList.SelectMany(x => x.Select(x => x.Item2)).Max() + 3;

            var grid = Enumerable.Range(0, width)
                .Select(x =>
                    Enumerable.Repeat('.', height).ToList()
                ).ToList();

            // Not correct - we're only setting corners, not squares in between!!
            foreach (var rock in rockList)
            {
                foreach(var (pt1, pt2) in rock.Zip(rock.Skip(1)))
                {
                    var currentCoords = pt1;
                    var deltaX = Math.Sign(pt2.Item1 - pt1.Item1);
                    var deltaY = Math.Sign(pt2.Item2 - pt1.Item2);
                    grid[currentCoords.Item1][currentCoords.Item2] = '#';

                    do
                    {
                        currentCoords.Item1 += deltaX;
                        currentCoords.Item2 += deltaY;
                        grid[currentCoords.Item1][currentCoords.Item2] = '#';
                    } while (
                        !(currentCoords.Item1 == pt2.Item1
                        && currentCoords.Item2 == pt2.Item2)
                    );
                }
            }

            foreach (var column in grid)
            {
                column[height - 1] = '#';
            }
            return grid;
        }

        private static int SandsAtRestBeforeAbyss(List<List<char>> grid)
        {
            var sandSource = (500, 0);
            var restfulSandCount = 0;
            var currentSandLocation = sandSource;
            var rowCount = grid.First().Count;

            while (true)
            {
                // We return if we reach abyss
                if (currentSandLocation.Item2 + 1 >= rowCount)
                    return restfulSandCount;

                if (grid
                    [currentSandLocation.Item1]
                    [currentSandLocation.Item2 + 1] == '.'
                )
                {
                    // Sand can fall
                    currentSandLocation = (
                        currentSandLocation.Item1, 
                        currentSandLocation.Item2 + 1
                    );
                } 
                else if (grid
                    [currentSandLocation.Item1 - 1]
                    [currentSandLocation.Item2 + 1] == '.'
                )
                {
                    // Sand can fall left
                    currentSandLocation = (
                        currentSandLocation.Item1 - 1,
                        currentSandLocation.Item2 + 1
                    );
                }
                else if (grid
                    [currentSandLocation.Item1 + 1]
                    [currentSandLocation.Item2 + 1] == '.'
                )
                {
                    // Sand can fall left
                    currentSandLocation = (
                        currentSandLocation.Item1 + 1,
                        currentSandLocation.Item2 + 1
                    );
                } 
                else
                {
                    // Sand is rested
                    grid
                        [currentSandLocation.Item1]
                        [currentSandLocation.Item2]
                    = 'O';
                    restfulSandCount++;
                    currentSandLocation = sandSource;
                }
            }
        }

        private static int SandsAtRestWhenFull(List<List<char>> grid)
        {
            var sandSource = (500, 0);
            var restfulSandCount = 0;
            var currentSandLocation = sandSource;

            while (true)
            {
                if (grid
                    [currentSandLocation.Item1]
                    [currentSandLocation.Item2 + 1] == '.'
                )
                {
                    // Sand can fall
                    currentSandLocation = (
                        currentSandLocation.Item1,
                        currentSandLocation.Item2 + 1
                    );
                }
                else if (grid
                    [currentSandLocation.Item1 - 1]
                    [currentSandLocation.Item2 + 1] == '.'
                )
                {
                    // Sand can fall left
                    currentSandLocation = (
                        currentSandLocation.Item1 - 1,
                        currentSandLocation.Item2 + 1
                    );
                }
                else if (grid
                    [currentSandLocation.Item1 + 1]
                    [currentSandLocation.Item2 + 1] == '.'
                )
                {
                    // Sand can fall left
                    currentSandLocation = (
                        currentSandLocation.Item1 + 1,
                        currentSandLocation.Item2 + 1
                    );
                }
                else
                {
                    
                    // Sand is rested
                    grid
                        [currentSandLocation.Item1]
                        [currentSandLocation.Item2]
                    = 'O';
                    restfulSandCount++;
                    if (currentSandLocation.Item1 == sandSource.Item1 && currentSandLocation.Item2 == sandSource.Item2)
                        return restfulSandCount;

                    currentSandLocation = sandSource;
                }
            }
        }

        private static bool CanMove((int, int) currentLocation, Move move, List<List<char>> grid, int heightAtGround)
        {
            if (currentLocation.Item2 + 1 >= heightAtGround) return false;

            var diffX = move switch
            {
                Move.Down => 0,
                Move.DownLeft => -1,
                Move.DownRight => 1,
                _ => throw new NotImplementedException()
            };
            return grid
                    [currentLocation.Item1 + diffX]
                    [currentLocation.Item2 + 1] == '.';
        }

        private static List<string> GetSeparatedInputFromFile() => 
            FileInputHelper.GetStringListFromFile("Day14.txt");

        public enum Move
        {
            Down,
            DownLeft,
            DownRight
        }
    }
}
