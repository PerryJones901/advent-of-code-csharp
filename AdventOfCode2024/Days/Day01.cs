using AdventOfCodeCommon;

namespace AdventOfCode2024.Days
{
    internal class Day01(bool isTest) : DayBase(1, isTest)
    {
        public override string Part1()
        {
            var input = GetInput();
            var (leftList, rightList) = GetLeftAndRightListsFromInput(input);

            var sum = leftList
                .Zip(rightList, (x, y) => Math.Abs(x - y))
                .Sum();

            return sum.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();
            var (leftList, rightList) = GetLeftAndRightListsFromInput(input);

            var frequencyDict = new Dictionary<int, int>();
            foreach(var rightNumber in rightList)
            {
                if (!frequencyDict.ContainsKey(rightNumber))
                    frequencyDict[rightNumber] = 0;

                frequencyDict[rightNumber]++;
            }

            var sum = leftList
                .Select(x => !frequencyDict.TryGetValue(x, out int value) ? 0 : value * x)
                .Sum();

            return sum.ToString();
        }

        private static (IOrderedEnumerable<int>, IOrderedEnumerable<int>) GetLeftAndRightListsFromInput(string[] input)
        {
            var leftList = input.Select(x => int.Parse(x.Split("   ")[0])).Order();
            var rightList = input.Select(x => int.Parse(x.Split("   ")[1])).Order();

            return (leftList, rightList);
        }

        private string[] GetInput()
        {
            return FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
        }
    }
}
