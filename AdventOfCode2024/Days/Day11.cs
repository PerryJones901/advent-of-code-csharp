using AdventOfCodeCommon;

namespace AdventOfCode2024.Days
{
    internal class Day11(bool isTest) : DayBase(11, isTest)
    {
        public override string Part1() => GetTotalStoneCount(isPart2: false).ToString();
        public override string Part2() => GetTotalStoneCount(isPart2: true).ToString();

        private string GetTotalStoneCount(bool isPart2)
        {
            var totalBlinkCount = isPart2 ? 75 : 25;

            // This memo store has:
            // - Key: (long: stoneValue, int: blinksToGo)
            // - Value: the stone count
            // Will be handy to save this as these calc's will be repeated a lot
            var memoStore = new Dictionary<(long, int), long>();
            var input = GetInput();

            var stoneCount = input.Sum(stone =>
                GetStoneCount(stone, blinksToGo: totalBlinkCount, memoStore));

            return stoneCount.ToString();
        }

        private static long GetStoneCount(long stone, int blinksToGo, Dictionary<(long, int), long> memoStore)
        {
            if (blinksToGo == 0)
                return 1;

            if (memoStore.ContainsKey((stone, blinksToGo)))
                return memoStore[(stone, blinksToGo)];

            long stoneCount;
            if (stone == 0)
            {
                stoneCount = GetStoneCount(1, blinksToGo - 1, memoStore);
            }
            else if (stone.ToString().Length % 2 == 0)
            {
                var lengthOfString = stone.ToString().Length;
                var leftStone = long.Parse(stone.ToString()[..(lengthOfString / 2)]);
                var rightStone = long.Parse(stone.ToString()[(lengthOfString / 2)..]);

                var leftStoneCount = GetStoneCount(leftStone, blinksToGo - 1, memoStore);
                var rightStoneCount = GetStoneCount(rightStone, blinksToGo - 1, memoStore);
                stoneCount = leftStoneCount + rightStoneCount;
            }
            else
            {
                stoneCount = GetStoneCount(stone * 2024, blinksToGo - 1, memoStore);
            }

            if (!memoStore.ContainsKey((stone, blinksToGo)))
                memoStore[(stone, blinksToGo)] = stoneCount;

            return stoneCount;
        }

        private long[] GetInput()
            => FileInputAssistant.GetLongRowsFromFile(TextInputFilePath, splitSeparator: " ");
    }
}
