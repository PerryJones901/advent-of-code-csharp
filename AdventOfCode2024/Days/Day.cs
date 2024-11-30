using AdventOfCodeCommon;

namespace AdventOfCode2024.Days
{
    internal class Day(bool isTest) : DayBase(0, isTest)
    {
        public override string Part1()
        {
            var input = GetInput();

            return "Part 1";
        }

        public override string Part2()
        {
            var input = GetInput();

            return "Part 2";
        }

        private string[] GetInput()
        {
            return FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
        }
    }
}
