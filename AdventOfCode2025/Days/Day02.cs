using AdventOfCodeCommon;
using System.Runtime.CompilerServices;

namespace AdventOfCode2025.Days
{
    internal class Day02(bool isTest) : DayBase(2, isTest)
    {
        public override string Part1()
        {
            var input = GetInput();

            var invalidIdSum = 0L;
            foreach (var line in input)
            {
                var lowerPartStr = line.Split("-")[0];
                var upperPartStr = line.Split("-")[1];

                for (var length = lowerPartStr.Length; length <= upperPartStr.Length; length++)
                {
                    if (length % 2 == 1)
                        continue;

                    var chunkLength = length / 2;
                    var keyDivisor = GetKeyDivisor(chunkLength, length);
                    long rangeStart = (long)Math.Pow(10, length - 1);
                    long rangeEnd = (long)Math.Pow(10, length) - 1;

                    if (length == lowerPartStr.Length)
                        rangeStart = long.Parse(lowerPartStr);
                    if (length == upperPartStr.Length)
                        rangeEnd = long.Parse(upperPartStr);

                    var lowerQuotient = (rangeStart - 1) / keyDivisor;
                    var upperQuotient = rangeEnd / keyDivisor;
                    var difference = upperQuotient - lowerQuotient;

                    foreach (var i in Enumerable.Range(1, (int)difference))
                    {
                        var invalidId = keyDivisor * (lowerQuotient + i);
                        invalidIdSum += invalidId;
                    }
                }
            }

            return invalidIdSum.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();

            var invalidIdSum = 0L;
            foreach (var line in input)
            {
                var lowerPartStr = line.Split("-")[0];
                var upperPartStr = line.Split("-")[1];
                var seenInvalidIds = new HashSet<long>();

                for (var length = lowerPartStr.Length; length <= upperPartStr.Length; length++)
                {
                    for (var i = 2; i <= length; i++)
                    {
                        if (length % i != 0)
                            continue;

                        var chunkLength = length / i;
                        var keyDivisor = GetKeyDivisor(chunkLength, length);
                        long rangeStart = (long)Math.Pow(10, length - 1);
                        long rangeEnd = (long)Math.Pow(10, length) - 1;

                        if (length == lowerPartStr.Length)
                            rangeStart = long.Parse(lowerPartStr);
                        if (length == upperPartStr.Length)
                            rangeEnd = long.Parse(upperPartStr);

                        var lowerQuotient = (rangeStart - 1) / keyDivisor;
                        var upperQuotient = rangeEnd / keyDivisor;
                        var difference = upperQuotient - lowerQuotient;

                        foreach (var j in Enumerable.Range(1, (int)difference))
                        {
                            var invalidId = keyDivisor * (lowerQuotient + j);

                            seenInvalidIds.Add(invalidId);
                        }
                    }
                }

                invalidIdSum += seenInvalidIds.Sum();
            }

            return invalidIdSum.ToString();
        }

        private static int GetKeyDivisor(int lengthOfRepeat, int lengthOfSequence)
        {
            var numRepeats = lengthOfSequence / lengthOfRepeat;
            var keyDivisorStr = string.Join("", Enumerable.Repeat("1".PadLeft(lengthOfRepeat, '0'), numRepeats));
            return int.Parse(keyDivisorStr);
        }

        private string[] GetInput()
        {
            return FileInputAssistant.GetStringArrayFromFile(TextInputFilePath, ",");
        }
    }
}
