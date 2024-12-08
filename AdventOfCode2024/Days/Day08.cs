using AdventOfCodeCommon;
using AdventOfCodeCommon.Extensions;

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

                foreach (var (firstAntennaLocation, secondAntennaLocation) in antennaLocations.GetAllPairCombinations())
                {
                    var rowDiff = secondAntennaLocation.Item1 - firstAntennaLocation.Item1;
                    var colDiff = secondAntennaLocation.Item2 - firstAntennaLocation.Item2;

                    AddAntiNodes(
                        antiNodeLocations,
                        antennaLocation: secondAntennaLocation,
                        delta: (rowDiff, colDiff),
                        isPart2,
                        input);

                    AddAntiNodes(
                        antiNodeLocations,
                        antennaLocation: firstAntennaLocation,
                        delta: (-rowDiff, -colDiff),
                        isPart2,
                        input);
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

        private static void AddAntiNodes(
            HashSet<(int, int)> antiNodeLocations,
            (int, int) antennaLocation,
            (int, int) delta,
            bool isPart2,
            string[] input)
        {
            var (row, col) = antennaLocation;
            var (rowDelta, colDelta) = delta;

            if (isPart2)
                antiNodeLocations.Add((row, col));

            do
            {
                row += rowDelta;
                col += colDelta;

                if (!input.IsInBounds(row, col))
                    break;

                antiNodeLocations.Add((row, col));
            }
            while (isPart2);
        }

        private string[] GetInput()
        {
            return FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
        }
    }
}
