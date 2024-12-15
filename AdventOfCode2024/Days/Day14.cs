using AdventOfCodeCommon;
using System.Diagnostics;

namespace AdventOfCode2024.Days
{
    internal class Day14(bool isTest) : DayBase(14, isTest)
    {
        private const int BATHROOM_WIDTH = 101;
        private const int BATHROOM_HEIGHT = 103;
        private const int PART_1_SECONDS = 100;

        /*
         * In hindsight, we're able to find the tree based on the border that surrounds it.
         * For example, there's a border of 33 hashtags. We'll make the assumption that if we ever see this many hashtags in a row, we've found the tree
         * We'll also set a sensible upper bound of seconds before we give up.
         */
        private const string BORDER_OF_TREE = "#################################";
        private const int PART_2_MAX_SECONDS = 10_000;

        public override string Part1()
        {
            var robots = GetInput();
            var robotToLocation = GetRobotToLocationDictionary(robots, PART_1_SECONDS);

            var midLineX = BATHROOM_WIDTH / 2;
            var midLineY = BATHROOM_HEIGHT / 2;

            var topLeftCount = robotToLocation.Values.Count(x => x.Item1 < midLineX && x.Item2 < midLineY);
            var topRightCount = robotToLocation.Values.Count(x => x.Item1 > midLineX && x.Item2 < midLineY);
            var bottomLeftCount = robotToLocation.Values.Count(x => x.Item1 < midLineX && x.Item2 > midLineY);
            var bottomRightCount = robotToLocation.Values.Count(x => x.Item1 > midLineX && x.Item2 > midLineY);

            var safetyFactor = topLeftCount * topRightCount * bottomLeftCount * bottomRightCount;

            return safetyFactor.ToString();
        }

        public override string Part2()
        {
            var robots = GetInput();

            var secondsPassed = 0;
            while (secondsPassed <= PART_2_MAX_SECONDS)
            {
                secondsPassed++;
                var charArrays = Get2DCharArrayOfDots();
                var robotToLocation = GetRobotToLocationDictionary(robots, secondsPassed);

                foreach (var (newPosX, newPosY) in robotToLocation.Values)
                    charArrays[newPosX][newPosY] = '#';

                var grid = string.Join("\r\n", charArrays.Select(x => string.Join("", x)));
                
                if (grid.Contains(BORDER_OF_TREE))
                {
                    Console.WriteLine($"..... Does the following image contain the tree? .....");
                    Console.WriteLine(grid);
                    return $"..... If so, the answer is {secondsPassed}! .....";
                }
            }

            throw new Exception($"Could not find tree within {PART_2_MAX_SECONDS} seconds.");
        }

        [DebuggerDisplay("X={X}, Y={Y}")]
        private class Coords
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        [DebuggerDisplay("Pos=({Position}), Vel=({Velocity})")]
        private class Robot
        {
            public required Coords Position { get; set; }
            public required Coords Velocity { get; set; }
        }

        private static Dictionary<Robot, (int, int)> GetRobotToLocationDictionary(Robot[] input, int secondsPassed)
        {
            var robotToLocation = new Dictionary<Robot, (int, int)>();

            foreach (var robot in input)
            {
                var newPosX = (robot.Position.X + ((robot.Velocity.X + BATHROOM_WIDTH) % BATHROOM_WIDTH) * secondsPassed) % BATHROOM_WIDTH;
                var newPosY = (robot.Position.Y + ((robot.Velocity.Y + BATHROOM_HEIGHT) % BATHROOM_HEIGHT) * secondsPassed) % BATHROOM_HEIGHT;

                robotToLocation.Add(robot, (newPosX, newPosY));
            }

            return robotToLocation;
        }

        private static char[][] Get2DCharArrayOfDots()
        {
            var charArrays = new char[BATHROOM_WIDTH][];
            for (int x = 0; x < BATHROOM_WIDTH; x++)
            {
                charArrays[x] = new char[BATHROOM_HEIGHT];

                for (int y = 0; y < BATHROOM_HEIGHT; y++)
                {
                    charArrays[x][y] = '.';
                }
            }

            return charArrays;
        }

        private Robot[] GetInput()
        {
            var input = FileInputAssistant.GetStringFromFile(TextInputFilePath);

            var parts = StringHelper.GetRegexCapturesFromInput(
                input,
                @"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)",
                "\r\n"
            );

            var prizeCases = parts.Select(x => new Robot
            {
                Position = new Coords { X = int.Parse(x[0]), Y = int.Parse(x[1]) },
                Velocity = new Coords { X = int.Parse(x[2]), Y = int.Parse(x[3]) },
            }).ToArray();

            return prizeCases;
        }
    }
}
