using AdventOfCodeCommon;

namespace AdventOfCode2015.Days
{
    internal class Day02(bool isTest) : DayBase(2, isTest)
    {
        public override string Part1()
        {
            var input = GetInput();

            var totalWrappingPaperArea = input
                .Select(box => box.OrderBy(dimension => dimension).ToArray())
                .Sum(box => 3 * box[0] * box[1] + 2 * box[1] * box[2] + 2 * box[0] * box[2]);

            return totalWrappingPaperArea.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();

            var totalRibbonLength = input
                .Select(box => box.OrderBy(dimension => dimension).ToArray())
                .Sum(box => 2*box[0] + 2*box[1] + box[0]*box[1]*box[2]);

            return totalRibbonLength.ToString();
        }

        private int[][] GetInput()
            => FileInputAssistant.GetParamListsFromRegexFromFile(
                TextInputFilePath,
                @"(\d+)x(\d+)x(\d+)"
            ).Select(x => x.Select(int.Parse).ToArray()).ToArray();
    }
}
