using AdventOfCode2023.Helpers;
using System.Diagnostics;

namespace AdventOfCode2023.Days;

public abstract class Day24
{
    const long COORD_MIN = 200000000000000;
    const long COORD_MAX = 400000000000000;

    public static int Part1Answer() =>
        Part1(GetSeparatedInputFromFile());

    public static int Part2Answer() =>
        Part2(GetSeparatedInputFromFile());

    public static int Part1(List<List<string>> input)
    {
        // TOO HIGH: 22951
        // TOO HIGH: 18783

        var hails = input.Select(
            x => new Hail
            {
                Position = new Vector3
                {
                    X = long.Parse(x[0]),
                    Y = long.Parse(x[1]),
                    Z = long.Parse(x[2])
                },
                Velocity = new Vector3
                {
                    X = long.Parse(x[3]),
                    Y = long.Parse(x[4]),
                    Z = long.Parse(x[5])
                }
            }
        ).ToList();

        var goodLineIntersections = 0;

        for (int i = 0; i < hails.Count - 1; i++)
        {
            var firstHail = hails[i];
            for (int j = i + 1; j < hails.Count; j++)
            {
                var secondHail = hails[j];

                // calculate det
                var det = -secondHail.Velocity.X * firstHail.Velocity.Y
                    + firstHail.Velocity.X * secondHail.Velocity.Y;

                if (det == 0)
                {
                    // Lines are parallel
                    continue;
                }

                // Calculate t_0
                var xDis = firstHail.Position.X - secondHail.Position.X; // a
                var yDis = firstHail.Position.Y - secondHail.Position.Y; // d
                var t_0 = (1 / (double)det) * (secondHail.Velocity.X * yDis - secondHail.Velocity.Y * xDis);
                var t_1 = (1 / (double)det) * (firstHail.Velocity.X * yDis - firstHail.Velocity.Y * xDis);

                if (t_0 < 0 || t_1 < 0) continue;

                // Calculate w_x and w_y
                var w_x = firstHail.Position.X + t_0 * firstHail.Velocity.X;
                var w_y = firstHail.Position.Y + t_0 * firstHail.Velocity.Y;

                if (COORD_MIN <= w_x && w_x <= COORD_MAX
                    && COORD_MIN <= w_y && w_y <= COORD_MAX)
                    goodLineIntersections++;
            }
        }

        return goodLineIntersections;
    }

    public static int Part2(List<List<string>> input)
    {
        return 0;
    }

    [DebuggerDisplay("Pos: {Position}, Vel: {Velocity}")]
    private class Hail
    {
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }
    }

    [DebuggerDisplay("X={X}, Y={Y}, Z={Z}")]
    private class Vector3
    {
        public long X { get; set; }
        public long Y { get; set; }
        public long Z { get; set; }
    }

    private static List<List<string>> GetSeparatedInputFromFile() =>
        FileInputHelper.GetParamListsFromRegexFromFile(
            "Day24.txt",
            @"(\d+), (\d+), (\d+) @ (\-?\d+), (\-?\d+), (\-?\d+)"
        );
}
