using AdventOfCodeCommon;

namespace AdventOfCode2024.Days
{
    internal class Day08(bool isTest) : DayBase(8, isTest)
    {
        public override string Part1()
        {
            var input = GetInput();
            var frequenciesToLocations = GetFrequenciesToLocationsDictionary(input);

            var antiNodeLocations = new HashSet<(int, int)>();

            foreach (var frequencyToLocations in frequenciesToLocations)
            {
                var locations = frequencyToLocations.Value;

                foreach (var (firstLocation, secondLocation) in GetAllPairCombinations(locations))
                {
                    var rowDiff = secondLocation.Item1 - firstLocation.Item1;
                    var colDiff = secondLocation.Item2 - firstLocation.Item2;

                    var antinode1Row = secondLocation.Item1 + rowDiff;
                    var antinode1Col = secondLocation.Item2 + colDiff;
                    if (IsInBounds(antinode1Row, antinode1Col, input))
                        antiNodeLocations.Add((antinode1Row, antinode1Col));

                    var antinode2Row = firstLocation.Item1 - rowDiff;
                    var antinode2Col = firstLocation.Item2 - colDiff;
                    if (IsInBounds(antinode2Row, antinode2Col, input))
                        antiNodeLocations.Add((antinode2Row, antinode2Col));
                }
            }

            var uniqueLocationCount = antiNodeLocations.Count;

            return uniqueLocationCount.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();
            var frequenciesToLocations = GetFrequenciesToLocationsDictionary(input);

            var antiNodeLocations = new HashSet<(int, int)>();

            foreach (var frequencyToLocations in frequenciesToLocations)
            {
                var locations = frequencyToLocations.Value;

                foreach (var (first, second) in GetAllPairCombinations(locations))
                {
                    var rowDiff = second.Item1 - first.Item1;
                    var colDiff = second.Item2 - first.Item2;

                    // Add nodes after second location
                    var antinodeRow = second.Item1;
                    var antinodeCol = second.Item2;

                    while (true)
                    {
                        antiNodeLocations.Add((antinodeRow, antinodeCol));

                        antinodeRow += rowDiff;
                        antinodeCol += colDiff;

                        if (!IsInBounds(antinodeRow, antinodeCol, input))
                            break;
                    }

                    // Add nodes before first location
                    antinodeRow = first.Item1;
                    antinodeCol = first.Item2;

                    while (true)
                    {
                        antiNodeLocations.Add((antinodeRow, antinodeCol));

                        antinodeRow -= rowDiff;
                        antinodeCol -= colDiff;

                        if (!IsInBounds(antinodeRow, antinodeCol, input))
                            break;
                    }
                }
            }

            var uniqueLocationCount = antiNodeLocations.Count;

            return uniqueLocationCount.ToString();
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
        {
            return enumerable.SelectMany((item, index) => enumerable.Skip(index + 1), (x, y) => (x, y));
        }

        private static bool IsInBounds(int row, int col, string[] input)
        {
            return row >= 0 && row < input.Length && col >= 0 && col < input[0].Length;
        }

        private string[] GetInput()
        {
            return FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
        }
    }
}
