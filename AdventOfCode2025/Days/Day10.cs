using AdventOfCodeCommon;

namespace AdventOfCode2025.Days
{
    internal class Day10(bool isTest) : DayBase(10, isTest)
    {
        public override string Part1()
        {
            var input = GetInput();
            var buttonPressCount = 0;

            foreach (var line in input)
            {
                var elements = line.Split(' ');
                var bitCount = elements.Length - 2;

                var lightsBinaryNumberString = elements
                    .First()
                    .Replace("[", "")
                    .Replace("]", "")
                    .Replace(".", "0")
                    .Replace("#", "1")
                    .Reverse()
                    .Aggregate("", (a, b) => $"{a}{b}");

                var lightNumber = Convert.ToInt32(lightsBinaryNumberString, 2);

                var buttonNumbers = elements
                    .Skip(1)
                    .SkipLast(1)
                    .Select(b =>
                        b
                        .Replace("(", "")
                        .Replace(")", "")
                        .Split(',')
                        .Select(int.Parse)
                        .Select(index => 1 << index)
                        .Aggregate((a, c) => a | c)
                    ).ToArray();

                for (int i = 1; i <= bitCount; i++)
                {
                    var comboFound = false;
                    var binariesWithN1Bits = GetListOfBinaryNumbersWithN1bits(bitCount, i);
                    foreach (var binary in binariesWithN1Bits)
                    {
                        var number = 0;
                        for (int index = 0; index < bitCount; index++)
                        {
                            if (binary[index] == '0')
                                continue;

                            var buttonNumber = buttonNumbers[index];
                            number ^= buttonNumber;
                        }

                        if (number == lightNumber)
                        {
                            buttonPressCount += i;
                            comboFound = true;
                            break;
                        }
                    }

                    if (comboFound)
                        break;
                }
            }

            return buttonPressCount.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();

            return "Part 2";
        }

        private static List<string> GetListOfBinaryNumbersWithN1bits(int bitCount, int n1Bits)
        {
            var results = new List<string>();
            var totalCombinations = 1 << bitCount;
            for (int i = 0; i < totalCombinations; i++)
            {
                var binaryString = Convert.ToString(i, 2).PadLeft(bitCount, '0');
                if (binaryString.Count(c => c == '1') == n1Bits)
                {
                    results.Add(binaryString);
                }
            }
            return results;
        }

        private string[] GetInput()
            => FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
    }
}
