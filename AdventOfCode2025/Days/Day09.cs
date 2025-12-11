using AdventOfCodeCommon;
using AdventOfCodeCommon.Extensions;

namespace AdventOfCode2025.Days
{
    internal class Day09(bool isTest) : DayBase(9, isTest)
    {
        public override string Part1()
        {
            var input = GetInput();
            var coords = input.Select(line => 
                (int.Parse(line.Split(',')[0]), int.Parse(line.Split(',')[1])
            )).ToList();

            var maxArea = 0L;

            foreach (var (coords0, coords1) in coords.GetAllPairCombinations())
            {
                long xLength = Math.Abs(coords1.Item1 - coords0.Item1) + 1;
                long yLength = Math.Abs(coords1.Item2 - coords0.Item2) + 1;

                long area = xLength * yLength;
                if (area > maxArea)
                    maxArea = area;
            }

            return maxArea.ToString();
        }

        public override string Part2()
        {
            // Step 1: Gotta grab all possible pairs
            // Step 2: Travel along the path between two points, marking pairs as 'not useable' when found
            // Step 3: Calculate area for each pair that is still useable
            // Step 4: Profit

            return "Part 2";
        }

        private string[] GetInput()
            => FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
    }
}
