using AdventOfCodeCommon;
using AdventOfCodeCommon.Directions;
using AdventOfCodeCommon.Extensions;
using AdventOfCodeCommon.Models;
using System.Collections.ObjectModel;

namespace AdventOfCode2024.Days
{
    internal class Day06(bool isTest) : DayBase(6, isTest)
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
            var visitedLocations = GetVisitedLocations(input);

            return visitedLocations.Count.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();
            var visitedLocations = GetVisitedLocations(input);
            var startLocation = FindStartLocation(input);

            var obstacleLocations = visitedLocations.Where(x => x != startLocation).ToHashSet();

            var loopCount = 0;

            foreach (var obstacleLocation in obstacleLocations)
            {
                var inputWithObstacle = GetInputWithObstacle(input, obstacleLocation);
                var direction = Direction.Up;
                var location = startLocation;

                var visitedSpacesWithDirection = new HashSet<((int, int), Direction)>()
                {
                    (location, direction)
                };

                // Guard patrol again with new obstacle added
                while (true)
                {
                    var (rowDiff, colDiff) = DirectionToDiffValue[direction];
                    var spaceInFront = (location.Item1 + rowDiff, location.Item2 + colDiff);

                    if (!inputWithObstacle.IsInBounds(spaceInFront.Item1, spaceInFront.Item2))
                    {
                        break;
                    }
                    else if (inputWithObstacle[spaceInFront.Item1][spaceInFront.Item2] == '#')
                    {
                        direction = direction.ToClockwiseDirection();
                    }
                    else
                    {
                        location = spaceInFront;

                        if (visitedSpacesWithDirection.Contains((location, direction)))
                        {
                            loopCount++;
                            break;
                        }

                        visitedSpacesWithDirection.Add((location, direction));
                    }
                }
            }

            return loopCount.ToString();
        }

        private static (int, int) FindStartLocation(string[] input)
        {
            for (var row = 0; row < input.Length; row++)
            {
                for (var col = 0; col < input[row].Length; col++)
                {
                    if (input[row][col] == '^')
                    {
                        return (row, col);
                    }
                }
            }

            throw new Exception("Could not find the starting point");
        }

        private static HashSet<(int, int)> GetVisitedLocations(string[] input)
        {
            var direction = Direction.Up;
            var location = FindStartLocation(input);
            var spacesVisited = new HashSet<(int, int)>([location]);

            while (true)
            {
                var (rowDiff, colDiff) = DirectionToDiffValue[direction];
                var spaceInFront = (location.Item1 + rowDiff, location.Item2 + colDiff);

                if (!input.IsInBounds(spaceInFront.Item1, spaceInFront.Item2))
                {
                    break;
                }
                else if (input[spaceInFront.Item1][spaceInFront.Item2] == '#')
                {
                    direction = direction.ToClockwiseDirection();
                }
                else
                {
                    location = spaceInFront;

                    spacesVisited.Add(location);
                }
            }

            return spacesVisited;
        }

        private static string[] GetInputWithObstacle(string[] input, (int, int) location)
        {
            var newInput = input.Select(x => $"{x}").ToArray();
            var rowWithWall = newInput[location.Item1];
            newInput[location.Item1] = $"{rowWithWall[..location.Item2]}#{rowWithWall[(location.Item2 + 1)..]}";

            return newInput;
        }

        private string[] GetInput()
        {
            return FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
        }
    }
}
