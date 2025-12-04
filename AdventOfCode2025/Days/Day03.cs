using AdventOfCodeCommon;

namespace AdventOfCode2025.Days
{
    internal class Day03(bool isTest) : DayBase(3, isTest)
    {
        public override string Part1() => GetJoltage(batteryCount: 2);
        public override string Part2() => GetJoltage(batteryCount: 12);

        private string GetJoltage(int batteryCount)
        {
            var input = GetInput();

            var sum = 0L;

            foreach (var line in input)
            {
                var currentDigitIndex = 0;
                var currentFromIndex = 0;
                var joltString = string.Empty;

                while (currentDigitIndex < batteryCount)
                {
                    var maxDigit = line[currentFromIndex];
                    var maxIndex = currentFromIndex;

                    for (int i = currentFromIndex + 1; i <= line.Length - batteryCount + currentDigitIndex; i++)
                    {
                        if (line[i] <= maxDigit)
                            continue;

                        maxDigit = line[i];
                        maxIndex = i;
                    }

                    joltString += maxDigit.ToString();
                    currentFromIndex = maxIndex + 1;

                    currentDigitIndex++;
                }

                var joltValue = long.Parse(joltString);

                sum += joltValue;
            }

            return sum.ToString();
        }

        private int[][] GetInput()
            => FileInputAssistant.GetIntArrayRowsFromFile(TextInputFilePath);
    }
}
