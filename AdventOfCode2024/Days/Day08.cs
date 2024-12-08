using AdventOfCodeCommon;

namespace AdventOfCode2024.Days
{
    internal class Day08(bool isTest) : DayBase(8, isTest)
    {
        public override string Part1()
            => GetAntiNodeLocationCount(isPart2: false).ToString();

        public override string Part2()
            => GetAntiNodeLocationCount(isPart2: true).ToString();

        public int GetAntiNodeLocationCount(bool isPart2)
        {
            var input = GetInput();
            var frequenciesToLocations = GetFrequenciesToLocationsDictionary(input);
            var antiNodeLocations = new HashSet<(int, int)>();

            foreach (var frequencyToLocations in frequenciesToLocations)
            {
                var antennaLocations = frequencyToLocations.Value;

                foreach (var (first, second) in GetAllPairCombinations(antennaLocations))
                {
                    var rowDiff = second.Item1 - first.Item1;
                    var colDiff = second.Item2 - first.Item2;

                    // Add antinodes from 2nd location
                    // If Part2, include 2nd location itself
                    var antinodeRow = isPart2 ? second.Item1 : second.Item1 + rowDiff;
                    var antinodeCol = isPart2 ? second.Item2 : second.Item2 + colDiff;

                    do
                    {
                        if (!IsInBounds(antinodeRow, antinodeCol, input))
                            break;

                        antiNodeLocations.Add((antinodeRow, antinodeCol));

                        antinodeRow += rowDiff;
                        antinodeCol += colDiff;
                    }
                    while (isPart2);

                    // Add antinodes before first location
                    antinodeRow = isPart2 ? first.Item1 : first.Item1 - rowDiff;
                    antinodeCol = isPart2 ? first.Item2 : first.Item2 - colDiff;

                    do
                    {
                        if (!IsInBounds(antinodeRow, antinodeCol, input))
                            break;

                        antiNodeLocations.Add((antinodeRow, antinodeCol));

                        antinodeRow -= rowDiff;
                        antinodeCol -= colDiff;
                    }
                    while (isPart2);
                }
            }

            return antiNodeLocations.Count;
        }

        private static Dictionary<char, List<(int, int)>> GetFrequenciesToLocationsDictionary(string[] input)
        {
            var frequenciesToLocations = new Dictionary<char, List<(int, int)>>();

            for (int row = 0; row < input.Length; row++)
            {
                for (int col = 0; col < input[row].Length; col++)
                {
                    var frequency = input[row][col];
                    if (frequency == '.') continue;

                    if (!frequenciesToLocations.ContainsKey(frequency))
                        frequenciesToLocations[frequency] = [];

                    frequenciesToLocations[frequency].Add((row, col));
                }
            }

            return frequenciesToLocations;
        }

        private static IEnumerable<(T, T)> GetAllPairCombinations<T>(IList<T> enumerable)
            => enumerable.SelectMany((item, index) => enumerable.Skip(index + 1), (x, y) => (x, y));

        private static bool IsInBounds(int row, int col, string[] input)
            => row >= 0 && row < input.Length && col >= 0 && col < input[0].Length;

        private string[] GetInput()
        {
            return FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
        }
    }
}
