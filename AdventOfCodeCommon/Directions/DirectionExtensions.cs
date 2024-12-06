using AdventOfCodeCommon.Models;

namespace AdventOfCodeCommon.Directions
{
    public static class DirectionExtensions
    {
        public static Direction ToClockwiseDirection(this Direction direction)
            => (Direction)(((int)direction + 1) % 4);

        public static Direction ToAntiClockwiseDirection(this Direction direction)
            => (Direction)(((int)direction + 3) % 4);
    }
}
