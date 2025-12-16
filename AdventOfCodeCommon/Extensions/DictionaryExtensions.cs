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

        public static void AddOrAppend<T, U>(this IDictionary<T, IList<U>> dictionary, T key, U value)
        {
            if (!dictionary.ContainsKey(key))
                dictionary[key] = new List<U>();

            dictionary[key].Add(value);
        }
    }
}
