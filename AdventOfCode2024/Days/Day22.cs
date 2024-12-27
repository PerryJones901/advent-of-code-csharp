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

            // Step 1: track diff's over time, and put in dict of keys of 4-tuple int.
            //  Remember: we cannot replace because the monkey will sell on first time it sees it
            // Step 2: iterate on all keys til we find the one with best amount of bananas, and return that max

            // Use a master account
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

                            if (seqKey == (-2, 1, -1, 3))
                                Console.WriteLine(nextPrice);
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
