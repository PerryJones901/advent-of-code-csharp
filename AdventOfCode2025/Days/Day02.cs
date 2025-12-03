using AdventOfCodeCommon;

namespace AdventOfCode2025.Days
{
    internal class Day02(bool isTest) : DayBase(2, isTest)
    {
        public override string Part1() => GetInvalidIdSum(repeatCount: 2);
        public override string Part2() => GetInvalidIdSum();

        private string GetInvalidIdSum(int? repeatCount = null)
        {
            var input = GetInput();

            var invalidIdSum = 0L;
            foreach (var line in input)
            {
                var lowerPartStr = line.Split("-")[0];
                var upperPartStr = line.Split("-")[1];
                var seenInvalidIds = new HashSet<long>();

                for (var sequenceLength = lowerPartStr.Length; sequenceLength <= upperPartStr.Length; sequenceLength++)
                {
                    for (var i = 2; i <= sequenceLength; i++)
                    {
                        if (sequenceLength % i != 0 || (repeatCount.HasValue && i != repeatCount.Value))
                            continue;

                        var chunkLength = sequenceLength / i;
                        var divisor = GetDivisor(chunkLength, sequenceLength);

                        var rangeStart = sequenceLength == lowerPartStr.Length
                            ? long.Parse(lowerPartStr)
                            : (long)Math.Pow(10, sequenceLength - 1);

                        var rangeEnd = sequenceLength == upperPartStr.Length
                            ? long.Parse(upperPartStr)
                            : (long)Math.Pow(10, sequenceLength) - 1;

                        // By taking 'rangeStart - 1' instead of 'rangeStart', we're able to include 'rangeStart' itself if it's divisible
                        var lowerQuotient = (rangeStart - 1) / divisor;
                        var upperQuotient = rangeEnd / divisor;
                        var difference = upperQuotient - lowerQuotient;

                        foreach (var j in Enumerable.Range(1, (int)difference))
                        {
                            var invalidId = divisor * (lowerQuotient + j);

                            seenInvalidIds.Add(invalidId);
                        }
                    }
                }

                invalidIdSum += seenInvalidIds.Sum();
            }

            return invalidIdSum.ToString();
        }

        private static int GetDivisor(int chunkLength, int sequenceLength)
        {
            var chunk = "1".PadLeft(chunkLength, '0');
            var repeatCount = sequenceLength / chunkLength;
            var repeatingSequence = Enumerable.Repeat(chunk, repeatCount);

            var divisorStr = string.Join("", repeatingSequence);
            var divisor = int.Parse(divisorStr);

            return divisor;
        }

        private string[] GetInput()
            => FileInputAssistant.GetStringArrayFromFile(TextInputFilePath, ",");
    }
}
