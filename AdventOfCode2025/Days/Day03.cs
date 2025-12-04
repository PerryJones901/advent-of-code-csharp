using AdventOfCodeCommon;

namespace AdventOfCode2025.Days
{
    internal class Day03(bool isTest) : DayBase(3, isTest)
    {
        public override string Part1()
        {
            var input = GetInput();

            var sum = 0;

            foreach (var line in input)
            {
                // First, find the largest number, and its earliest index
                var max = line[0];
                var maxIndex = 0;
                for (int i = 1; i < line.Length - 1; i++)
                {
                    if (line[i] > max)
                    {
                        max = line[i];
                        maxIndex = i;
                    }
                }

                // Then, find the largest number after that index
                var secondMax = line[maxIndex + 1];
                for (int i = maxIndex + 2; i < line.Length; i++)
                {
                    if (line[i] > secondMax)
                    {
                        secondMax = line[i];
                    }
                }

                sum += 10 * max + secondMax;
            }

            return sum.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();

            var sum = 0L;

            var NUM_DIGITS_REQUIRED = 12;

            foreach (var line in input)
            {
                var currentDigit = 1;
                var currentFromIndex = 0;
                var digits = new List<int>();

                while (currentDigit <= NUM_DIGITS_REQUIRED)
                {
                    // First, find the largest number, and its earliest index
                    var maxDigit = line[currentFromIndex];
                    var maxIndex = currentFromIndex;

                    for (int i = currentFromIndex + 1; i < line.Length - NUM_DIGITS_REQUIRED + currentDigit; i++)
                    {
                        if (line[i] > maxDigit)
                        {
                            maxDigit = line[i];
                            maxIndex = i;
                        }
                    }

                    digits.Add(maxDigit);
                    currentFromIndex = maxIndex + 1;

                    currentDigit++;
                }

                var joltString = string.Join("", digits);
                var joltValue = long.Parse(joltString);

                sum += joltValue;
            }

            return sum.ToString();
        }

        private int[][] GetInput()
        {
            return FileInputAssistant.GetIntArrayRowsFromFile(TextInputFilePath);
        }
    }
}
