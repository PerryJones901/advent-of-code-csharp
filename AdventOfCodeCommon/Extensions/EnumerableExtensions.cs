namespace AdventOfCodeCommon.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<(T, T)> GetAllPairCombinations<T>(this IEnumerable<T> enumerable)
            => enumerable.SelectMany((item, index) => enumerable.Skip(index + 1), (x, y) => (x, y));
    }
}
