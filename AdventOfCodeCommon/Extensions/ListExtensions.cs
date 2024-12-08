namespace AdventOfCodeCommon.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// Determines whether the specified row and column are within the bounds of the grid.
        /// </summary>
        public static bool IsInBounds<S, T>(this IList<T> grid, int row, int col) where T : IList<S>
            => row >= 0 && row < grid.Count && col >= 0 && col < grid[0].Count;

        /// <summary>
        /// Determines whether the specified row and column are within the bounds of the grid.
        /// </summary>
        public static bool IsInBounds(this IList<string> grid, int row, int col)
            => row >= 0 && row < grid.Count && col >= 0 && col < grid[0].Length;
    }
}
