using System.Diagnostics;

namespace AdventOfCodeCommon.Vectors
{
    [DebuggerDisplay("X={X}, Y={Y}, Z={Z}")]
    public class Vector3Double
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
}
