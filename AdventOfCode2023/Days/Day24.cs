using AdventOfCode2023.Helpers;
using System.Diagnostics;

namespace AdventOfCode2023.Days;

public abstract class Day24
{
    const long COORD_MIN = 200000000000000;
    const long COORD_MAX = 400000000000000;

    public static int Part1Answer() =>
        Part1(GetSeparatedInputFromFile());

    public static long Part2Answer() =>
        Part2(GetSeparatedInputFromFile());

    private static List<Hail> GetHailsFromInput(List<List<string>> input)
    {
        return input.Select(
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
    }

    private static List<HailDouble> GetHailsDoubleFromInput(List<List<string>> input)
    {
        return input.Select(
            x => new HailDouble
            {
                Position = new Vector3Double
                {
                    X = long.Parse(x[0]),
                    Y = long.Parse(x[1]),
                    Z = long.Parse(x[2])
                },
                Velocity = new Vector3Double
                {
                    X = long.Parse(x[3]),
                    Y = long.Parse(x[4]),
                    Z = long.Parse(x[5])
                }
            }
        ).ToList();
    }

    public static int Part1(List<List<string>> input)
    {
        var hails = GetHailsFromInput(input);

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

    public static long Part2(List<List<string>> input)
    {
        var hails = GetHailsDoubleFromInput(input);

        var hail0 = hails[0];
        var hail1 = hails[1];
        var hail2 = hails[2];

        var p1 = hail1.Position - hail0.Position;
        var v1 = hail1.Velocity - hail0.Velocity;
        var p2 = hail2.Position - hail0.Position;
        var v2 = hail2.Velocity - hail0.Velocity;

        // Let t1, t2 be the collision times that rock hits hails 1 and 2 resp.
        // Viewed from hail0, location of collisions:
        // p1 + t1 * v1
        // p2 + t2 * v2

        // Hailstone 0 always at origin, so collision is at 0.
        // t1 = -((p1 x p2) * v2) / ((v1 x p2) * v2)
        // t2 = -((p1 x p2) * v1) / ((p1 x v2) * v1)

        var t1 = -p1.Cross(p2).Dot(v2) / v1.Cross(p2).Dot(v2);
        var t2 = -p1.Cross(p2).Dot(v1) / p1.Cross(v2).Dot(v1);

        // Collision coords (actual)
        var c1 = hail1.Position + (t1 * hail1.Velocity);
        var c2 = hail2.Position + (t2 * hail2.Velocity);
        var v = (c2 - c1) / (t2 - t1);
        var initPositionOfRock = c1 - t1 * v;

        return (long)initPositionOfRock.X + (long)initPositionOfRock.Y + (long)initPositionOfRock.Z;
    }

    [DebuggerDisplay("Pos: {Position}, Vel: {Velocity}")]
    private class Hail
    {
        public Vector3 Position { get; set; } = new Vector3();
        public Vector3 Velocity { get; set; } = new Vector3();
    }

    [DebuggerDisplay("Pos: {Position}, Vel: {Velocity}")]
    private class HailDouble
    {
        public Vector3Double Position { get; set; } = new Vector3Double();
        public Vector3Double Velocity { get; set; } = new Vector3Double();
    }

    [DebuggerDisplay("X={X}, Y={Y}, Z={Z}")]
    private class Vector3
    {
        public long X { get; set; }
        public long Y { get; set; }
        public long Z { get; set; }

        public Vector3 Cross(Vector3 vector)
        {
            return new Vector3
            {
                X = Y * vector.Z - Z * vector.Y,
                Y = Z * vector.X - X * vector.Z,
                Z = X * vector.Y - Y * vector.X,
            };
        }

        public long Dot(Vector3 vector)
            => X * vector.X + Y * vector.Y + Z * vector.Z;

        public static Vector3 operator +(Vector3 a, Vector3 b) => new()
        {
            X = a.X + b.X,
            Y = a.Y + b.Y,
            Z = a.Z + b.Z,
        };

        public static Vector3 operator -(Vector3 a, Vector3 b) => new()
        {
            X = a.X - b.X,
            Y = a.Y - b.Y,
            Z = a.Z - b.Z,
        };

        public static Vector3 operator *(long a, Vector3 b) => new()
        {
            X = a * b.X,
            Y = a * b.Y,
            Z = a * b.Z,
        };

        public static Vector3 operator *(Vector3 a, long b) => new()
        {
            X = a.X * b,
            Y = a.Y * b,
            Z = a.Z * b,
        };

        public static Vector3 operator /(Vector3 a, long b) => new()
        {
            X = a.X / b,
            Y = a.Y / b,
            Z = a.Z / b,
        };
    }

    [DebuggerDisplay("X={X}, Y={Y}, Z={Z}")]
    private class Vector3Double
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vector3Double Cross(Vector3Double vector)
        {
            return new Vector3Double
            {
                X = Y * vector.Z - Z * vector.Y,
                Y = Z * vector.X - X * vector.Z,
                Z = X * vector.Y - Y * vector.X,
            };
        }

        public double Dot(Vector3Double vector)
            => X * vector.X + Y * vector.Y + Z * vector.Z;

        public static Vector3Double operator +(Vector3Double a, Vector3Double b) => new()
        {
            X = a.X + b.X,
            Y = a.Y + b.Y,
            Z = a.Z + b.Z,
        };

        public static Vector3Double operator -(Vector3Double a, Vector3Double b) => new()
        {
            X = a.X - b.X,
            Y = a.Y - b.Y,
            Z = a.Z - b.Z,
        };

        public static Vector3Double operator *(double a, Vector3Double b) => new()
        {
            X = a * b.X,
            Y = a * b.Y,
            Z = a * b.Z,
        };

        public static Vector3Double operator *(Vector3Double a, double b) => new()
        {
            X = a.X * b,
            Y = a.Y * b,
            Z = a.Z * b,
        };

        public static Vector3Double operator /(Vector3Double a, double b) => new()
        {
            X = a.X / b,
            Y = a.Y / b,
            Z = a.Z / b,
        };
    }

    private static List<List<string>> GetSeparatedInputFromFile() =>
        FileInputHelper.GetParamListsFromRegexFromFile(
            "Day24.txt",
            @"(\d+), (\d+), (\d+) @ (\-?\d+), (\-?\d+), (\-?\d+)"
        );
}
