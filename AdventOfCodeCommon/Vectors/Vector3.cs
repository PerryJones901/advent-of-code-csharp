using System.Diagnostics;

namespace AdventOfCodeCommon.Vectors
{
    [DebuggerDisplay("X={X}, Y={Y}, Z={Z}")]
    public class Vector3
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

        public double DistanceTo(Vector3 vector)
        {
            var dx = X - vector.X;
            var dy = Y - vector.Y;
            var dz = Z - vector.Z;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

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
}
