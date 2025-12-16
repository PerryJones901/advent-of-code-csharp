using AdventOfCodeCommon;
using AdventOfCodeCommon.Extensions;

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

                var lightNumber = GetLightNumberFromSquareBracketString(elements.First());

                var buttonNumbers = elements
                    .Skip(1)
                    .SkipLast(1)
                    .Select(GetButtonNumberFromRoundBracketString)
                    .ToArray();

                buttonPressCount += GetButtonPressCount(lightNumber, buttonNumbers, bitCount);
            }

            return buttonPressCount.ToString();
        }

        private static int GetButtonPressCount(int lightNumber, int[] buttonNumbers, int bitCount)
        {
            var buttonPressCount = 0;

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

            return buttonPressCount;
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

        private static int GetLightNumberFromSquareBracketString(string squareBracketString)
        {
            var lightsBinaryNumberString = squareBracketString
                .Replace("[", "")
                .Replace("]", "")
                .Replace(".", "0")
                .Replace("#", "1")
                .Reverse()
                .Aggregate("", (a, b) => $"{a}{b}");

            var lightNumber = Convert.ToInt32(lightsBinaryNumberString, 2);

            return lightNumber;
        }

        private static int GetLightNumberFromJoltages(int[] joltages)
        {
            var lightNumber = 0;
            for (int i = 0; i < joltages.Length; i++)
            {
                var isOdd = joltages[i] % 2 == 1;

                if (isOdd)
                    lightNumber |= 1 << i;
            }

            return lightNumber;
        }

        private static int GetButtonNumberFromRoundBracketString(string roundBracketString)
            => roundBracketString
                .Replace("(", "")
                .Replace(")", "")
                .Split(',')
                .Select(int.Parse)
                .Select(index => 1 << index)
                .Aggregate((a, c) => a | c);

        private static int[] GetJoltagesFromCurlyBracketString(string curlyBracketString)
            => curlyBracketString
                .Replace("{", "")
                .Replace("}", "")
                .Split(',')
                .Select(int.Parse)
                .ToArray();

        public override string Part2()
        {
            // TOO LOW: 10617
            // TOO LOW: 12000
            var input = GetInput();

            var buttonPresses = 0;

            foreach(var line in input)
            {
                var elements = line.Split(' ');
                var joltages = GetJoltagesFromCurlyBracketString(elements.Last());
                var bitCount = joltages.Length;

                var buttonNumbers = elements
                    .Skip(1)
                    .SkipLast(1)
                    .Select(GetButtonNumberFromRoundBracketString)
                    .ToArray();

                var lightNumToInclusionNumMap = GetLightNumberToButtonInclusionNumbersMap(buttonNumbers);
                var emptyJoltagesCacheKey = GetCacheKey([.. Enumerable.Repeat(0, joltages.Length)]);

                var cache = new Dictionary<string, int>
                {
                    { emptyJoltagesCacheKey, 0 }
                };

                var buttonPressCountForLine = GetPressCount(
                    joltages,
                    buttonNumbers,
                    lightNumToInclusionNumMap,
                    cache);

                buttonPresses += buttonPressCountForLine;
            }

            return buttonPresses.ToString();
        }

        private static int GetPressCount(
            int[] joltages,
            int[] buttonNumbers,
            Dictionary<int, IList<int>> lightNumToButtonInclusionNumMap,
            Dictionary<string, int> cache)
        {
            var cacheKey = GetCacheKey(joltages);
            if (cache.TryGetValue(cacheKey, out var cachedPressCount))
                return cachedPressCount;

            var lightNumber = GetLightNumberFromJoltages(joltages);
            if (!lightNumToButtonInclusionNumMap.TryGetValue(lightNumber, out IList<int>? inclusionNumbers))
                return int.MaxValue;

            var minPressCount = int.MaxValue;

            foreach (var inclusionNum in inclusionNumbers)
            {
                var newJoltages = joltages.ToArray();
                var buttonCount = 0;
                var areButtonPressesValid = true;

                for (var buttonIndex = 0; buttonIndex < buttonNumbers.Length; buttonIndex++)
                {
                    if ((inclusionNum & (1 << buttonIndex)) == 0)
                        continue;

                    buttonCount++;
                    var buttonNumber = buttonNumbers[buttonIndex];

                    for (var joltIndex = 0; joltIndex < newJoltages.Length; joltIndex++)
                    {
                        if ((buttonNumber & (1 << joltIndex)) == 0)
                            continue;

                        if (newJoltages[joltIndex] == 0)
                        {
                            // We cannot press this button as it would make a joltage negative
                            // This means the whole button set cannot be done
                            areButtonPressesValid = false;
                            break;
                        }

                        newJoltages[joltIndex] -= 1;
                    }

                    if (!areButtonPressesValid)
                        break;
                }

                if (!areButtonPressesValid)
                    continue;

                newJoltages = [.. newJoltages.Select(j => j / 2)];

                var pressCount = buttonCount + (2 * GetPressCount(
                    newJoltages,
                    buttonNumbers,
                    lightNumToButtonInclusionNumMap,
                    cache));

                if (pressCount < minPressCount)
                    minPressCount = pressCount;
            }

            if (!cache.ContainsKey(cacheKey))
                cache[cacheKey] = minPressCount;

            return minPressCount;
        }

        private static Dictionary<int, IList<int>> GetLightNumberToButtonInclusionNumbersMap(int[] buttonNumbers)
        {
            var map = new Dictionary<int, IList<int>>();
            var bitCount = buttonNumbers.Length;
            var totalCombinations = 1 << bitCount;

            for (int inclusionBinNum = 0; inclusionBinNum < totalCombinations; inclusionBinNum++)
            {
                var number = 0;
                for (int index = 0; index < bitCount; index++)
                {
                    if ((inclusionBinNum & (1 << index)) == 0)
                        continue;

                    var buttonNumber = buttonNumbers[index];
                    number ^= buttonNumber;
                }

                map.AddOrAppend(number, inclusionBinNum);
            }

            return map;
        }

        private static string GetCacheKey(int[] joltages) => string.Join(",", joltages);

        private string[] GetInput()
            => FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
    }
}
