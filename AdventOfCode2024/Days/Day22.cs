using AdventOfCodeCommon;

namespace AdventOfCode2024.Days
{
    internal class Day22(bool isTest) : DayBase(22, isTest)
    {
        private const long MOD_CONST = 16_777_216;

        public override string Part1()
        {
            var input = GetInput();

            var secretNumSum = input.Sum(secretNum =>
                Enumerable.Range(0, 2000).Aggregate(
                    secretNum,
                    (num, _) => GetNextSecretNumber(num)
                ));

            return secretNumSum.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();
            var seqToTotalBananas = new Dictionary<(int, int, int, int), int>();

            foreach (var initSecretNum in input)
            {
                var priceSeqAlreadyDone = new HashSet<(int, int, int, int)>();
                var priceSeq = new List<int>();
                var currentNum = initSecretNum;

                for (int iter = 0; iter < 2_000; iter++)
                {
                    var nextNum = GetNextSecretNumber(currentNum);
                    var currentPrice = (int)(currentNum % 10);
                    var nextPrice = (int)(nextNum % 10);
                    var diffPrice = nextPrice - currentPrice;

                    priceSeq.Add(diffPrice);

                    while (priceSeq.Count > 4)
                        priceSeq.RemoveAt(0);

                    if (priceSeq.Count == 4)
                    {
                        var seqKey = (priceSeq[0], priceSeq[1], priceSeq[2], priceSeq[3]);

                        // Check if this is the first time you've seen the seq
                        if (!priceSeqAlreadyDone.Contains(seqKey))
                        {
                            // Now, add to main one if key not already there
                            if (!seqToTotalBananas.ContainsKey(seqKey))
                                seqToTotalBananas[seqKey] = 0;

                            // Increase the total bananas for the key
                            seqToTotalBananas[seqKey] += nextPrice;

                            // Finally, add to seq already done
                            priceSeqAlreadyDone.Add(seqKey);
                        }
                    }

                    currentNum = nextNum;
                }
            }

            var maxBananaCount = seqToTotalBananas.MaxBy(x => x.Value).Value;

            return maxBananaCount.ToString();
        }

        private static long GetNextSecretNumber(long number)
        {
            var multiply64Result = number * 64;
            var newNumber = number ^ multiply64Result;
            newNumber %= MOD_CONST;

            var divResult = newNumber / 32;
            newNumber ^= divResult;
            newNumber %= MOD_CONST;

            var multiply2048Result = newNumber * 2048;
            newNumber ^= multiply2048Result;
            newNumber %= MOD_CONST;

            return newNumber;
        }

        private long[] GetInput()
        {
            return FileInputAssistant.GetLongRowsFromFile(TextInputFilePath);
        }
    }
}
