using AdventOfCodeCommon;

namespace AdventOfCode2024.Days
{
    internal class Day22(bool isTest) : DayBase(22, isTest)
    {
        private const long MOD_CONST = 16_777_216;

        public override string Part1()
        {
            var input = GetInput();
            var secretNumSum = 0L;

            foreach (var initSecretNum in input)
            {
                var newNum = initSecretNum;
                for (int iter = 0; iter < 2_000; iter++)
                {
                    newNum = GetNextSecretNumber(newNum);
                }
                secretNumSum += newNum;
            }

            return secretNumSum.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();

            return "Part 2";
        }

        private static long GetNextSecretNumber(long number)
        {
            var mult64Result = number * 64;
            var newNumber = number ^ mult64Result;
            newNumber %= MOD_CONST;

            var divResult = newNumber / 32;
            newNumber ^= divResult;
            newNumber %= MOD_CONST;

            var mult2048Result = newNumber * 2048;
            newNumber ^= mult2048Result;
            newNumber %= MOD_CONST;

            return newNumber;
        }

        private long[] GetInput()
        {
            return FileInputAssistant.GetLongRowsFromFile(TextInputFilePath);
        }
    }
}
