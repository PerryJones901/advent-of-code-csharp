namespace AdventOfCodeCommon.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddOrIncrement<T>(this IDictionary<T, long> dictionary, T key, long amount)
        {
            if (!dictionary.ContainsKey(key))
                dictionary[key] = 0;

            dictionary[key] += amount;
        }
    }
}
