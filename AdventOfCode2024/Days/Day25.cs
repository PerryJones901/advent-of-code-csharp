using AdventOfCodeCommon;

namespace AdventOfCode2024.Days
{
    internal class Day25(bool isTest) : DayBase(25, isTest)
    {
        private const string NEW_LINE = "\r\n";
        public override string Part1()
        {
            var input = GetInput();
            var keys = new List<(int, int, int, int, int)>();
            var locks = new List<(int, int, int, int, int)>();

            foreach (var inputLine in input)
            {
                var inputLineAsStrings = inputLine.Split(NEW_LINE);
                var isLock = inputLine.StartsWith("#####");

                if (isLock)
                {
                    var heights = new List<int>();
                    for (int index = 0; index < 5; index++)
                    {
                        var height = 0;
                        while (true)
                        {
                            height++;
                            if (inputLineAsStrings[height][index] == '.')
                                break;
                        }
                        heights.Add(height - 1);
                    }
                    locks.Add((heights[0], heights[1], heights[2], heights[3], heights[4]));
                }
                else
                {
                    var heights = new List<int>();
                    for (int index = 0; index < 5; index++)
                    {
                        var height = 0;
                        while (true)
                        {
                            height++;
                            if (inputLineAsStrings[6-height][index] == '.')
                                break;
                        }
                        heights.Add(height - 1);
                    }
                    keys.Add((heights[0], heights[1], heights[2], heights[3], heights[4]));
                }
            }

            var goodMatchCount = 0;

            foreach (var @lock in locks)
            {
                foreach (var key in keys)
                {
                    var add1 = @lock.Item1 + key.Item1;
                    var add2 = @lock.Item2 + key.Item2;
                    var add3 = @lock.Item3 + key.Item3;
                    var add4 = @lock.Item4 + key.Item4;
                    var add5 = @lock.Item5 + key.Item5;

                    if (add1 <= 5 && add2 <= 5 && add3 <= 5 && add4 <= 5 && add5 <= 5)
                        goodMatchCount++;
                }
            }

            return goodMatchCount.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();

            return "Part 2";
        }

        private string[] GetInput()
        {
            return FileInputAssistant.GetStringArrayFromFile(TextInputFilePath, $"{NEW_LINE}{NEW_LINE}");
        }
    }
}
