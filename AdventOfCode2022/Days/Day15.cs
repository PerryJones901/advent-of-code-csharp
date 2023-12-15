using AdventOfCode2022.Helpers;

namespace AdventOfCode2022.Days
{
    public static class Day15
    {
        public static int Part1Answer() =>
            Part1(GetSeparatedInputFromFile());

        public static long Part2Answer() =>
            Part2(GetSeparatedInputFromFile());

        public static int Part1(List<string> input)
        {
            var dataLines = GetDataLinesFromInput(input);

            var hello = dataLines
                .Where(x => x.AllGood())
                .Select(x => (x.LeftXCoord(), x.RightXCoord())).ToList();

            var listOfEnds = new List<(int, int)>();
            foreach (var line in hello)
                listOfEnds.AddLine(line);

            return listOfEnds.Sum(x => x.Item2 - x.Item1 + 1);
        }

        public static long Part2(List<string> input)
        {
            var dataLines = GetDataLinesFromInput(input);

            var line1s = dataLines.Select(x => x.Line1()).OrderBy(x => x.YIntersept);
            var line2s = dataLines.Select(x => x.Line2()).OrderBy(x => x.YIntersept);
            var line3s = dataLines.Select(x => x.Line3()).OrderBy(x => x.YIntersept);
            var line4s = dataLines.Select(x => x.Line4()).OrderBy(x => x.YIntersept);

            Console.WriteLine("Line1s");
            foreach(var line in line1s)
            {
                Console.WriteLine(line.YIntersept);
            }
            Console.WriteLine("Line2s");

            foreach (var line in line2s)
            {
                Console.WriteLine(line.YIntersept);
            }
            Console.WriteLine("Line3s");

            foreach (var line in line3s)
            {
                Console.WriteLine(line.YIntersept);
            }
            Console.WriteLine("Line4s");

            foreach (var line in line4s)
            {
                Console.WriteLine(line.YIntersept);
            }

            var gradOnePairedLines = line1s
                .SelectMany(l1 =>
                    line3s.Where(l3 => l3.YIntersept - l1.YIntersept == 2).Select(l3 => (l1, l3))
                );

            var gradMinusOnePairedLines = line2s
                .SelectMany(l2 =>
                    line4s.Where(l4 => l4.YIntersept - l2.YIntersept == 2).Select(l4 => (l2, l4))
                );
            // Assuming 1 pair for each

            var upPair = gradOnePairedLines.Single();
            var downPair = gradMinusOnePairedLines.Single();

            var upLineYIntersept = upPair.l1.YIntersept + 1;
            var downLineYIntersept = downPair.l2.YIntersept + 1;

            var x = (downLineYIntersept - upLineYIntersept) / 2;
            var y = x + upLineYIntersept;

            return 4000000L * x + y;
        }

        private static List<DataLine> GetDataLinesFromInput(List<string> input)
            => input.Select(x => new DataLine
            {
                SensorX = int.Parse(x.Split("x=")[1].Split(',')[0]),
                SensorY = int.Parse(x.Split("y=")[1].Split(':')[0]),
                ClosestBeaconX = int.Parse(x.Split("x=")[2].Split(',')[0]),
                ClosestBeaconY = int.Parse(x.Split("y=")[2]),
            }).ToList();

        private static List<string> GetSeparatedInputFromFile() =>
            FileInputHelper.GetStringListFromFile("Day15.txt");
    }

    public class DataLine
    {
        private static int MAGIC_HEIGHT = 2_000_000;

        public int SensorX { get; set; }
        public int SensorY { get; set; }
        public int ClosestBeaconX { get; set; }
        public int ClosestBeaconY { get; set; }

        public int Radius()
            => Math.Abs(SensorX - ClosestBeaconX) + Math.Abs(SensorY - ClosestBeaconY);

        public int LeftXCoord()
            => SensorX - (Radius() - Math.Abs(MAGIC_HEIGHT - SensorY));

        public int RightXCoord()
            => SensorX + (Radius() - Math.Abs(MAGIC_HEIGHT - SensorY));

        public bool AllGood()
            => LeftXCoord() <= RightXCoord();

        // y = x - SensorX + SensorY + Radius
        public Line Line1()
        {
            return new Line
            {
                Gradient = 1,
                YIntersept = -SensorX + SensorY + Radius()
            };
        }

        // y = SensorX + Sensor Y + Radius - x
        public Line Line2()
        {
            return new Line
            {
                Gradient = -1,
                YIntersept = SensorX + SensorY + Radius()
            };
        }

        public Line Line3()
        {
            return new Line
            {
                Gradient = 1,
                YIntersept = -SensorX + SensorY - Radius()
            };
        }

        public Line Line4()
        {
            return new Line
            {
                Gradient = -1,
                YIntersept = SensorX + SensorY - Radius()
            };
        }
    }

    public class Line
    {
        public int Gradient { get; set; }
        public int YIntersept { get; set; }
    }

    public static class ListCoordsExtensions {
        public static void AddLine(this List<(int,int)> lines, (int, int) lineToAdd)
        {
            var linesToRemove = new List<(int, int)>();
            foreach(var line in lines)
            {
                // lineToAdd is completely contained in line
                if (line.Item1 <= lineToAdd.Item1 && lineToAdd.Item2 <= line.Item2)
                    return;
            }

            // Now do reverse
            foreach(var line in lines)
            {
                if (lineToAdd.Item1 <= line.Item1 && line.Item2 <= lineToAdd.Item2)
                    linesToRemove.Add(line);
            }

            foreach (var line in linesToRemove)
                lines.Remove(line);

            lines.Add(lineToAdd);
        }
    }
}
