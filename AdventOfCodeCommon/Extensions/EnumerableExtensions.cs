namespace AdventOfCodeCommon.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Returns all possible pair combinations of the elements in the enumerable.
        /// The order of the two elements within a pair will match the ordering of those elements in the enumerable.
        /// </summary>
        public static IEnumerable<(T, T)> GetAllPairCombinations<T>(this IEnumerable<T> enumerable)
            => enumerable.SelectMany((item, index) => enumerable.Skip(index + 1), (x, y) => (x, y));
    }
}
