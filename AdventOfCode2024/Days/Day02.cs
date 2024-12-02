using AdventOfCodeCommon;

namespace AdventOfCode2024.Days
{
    internal class Day02(bool isTest) : DayBase(2, isTest)
    {
        public override string Part1()
        {
            var input = GetInput();
            var totalSafe = input.Count(IsRowSafe);

            return totalSafe.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();
            var totalSafe = 0;

            foreach (var report in input)
            {
                var subsetReports = new List<int[]> { report };
                for (int i = 0; i < report.Length; i++)
                {
                    var subReportWithOneElementRemoved = report.Where((_, index) => index != i).ToArray();
                    subsetReports.Add(subReportWithOneElementRemoved);
                }

                if (subsetReports.Any(IsRowSafe))
                    totalSafe++;
            }

            return totalSafe.ToString();
        }

        private int[][] GetInput()
        {
            var stringRows = FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);

            var intRows = stringRows
                .Select(x => x.Split(" "))
                .Select(x => x.Select(int.Parse).ToArray())
                .ToArray();

            return intRows;
        }

        private bool IsRowSafe(int[] row)
        {
            var differences = row.Zip(row.Skip(1), (current, next) => next - current).ToList();
            var isSafe = AreAllPositiveOrAllNegative(differences) && AreAllUnderThreshold(differences);

            return isSafe;
        }

        private static bool AreAllPositiveOrAllNegative(List<int> numbers)
        {
            var distinctSigns = numbers.Select(Math.Sign).Distinct().ToList();
            var areAllSameNonZeroSign = distinctSigns.Count == 1 && distinctSigns.Single() != 0;

            return areAllSameNonZeroSign;
        }

        private static bool AreAllUnderThreshold(List<int> numbers)
        {
            return numbers.All(x => Math.Abs(x) <= 3);
        }
    }
}
