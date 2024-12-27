using AdventOfCodeCommon;

namespace AdventOfCode2024.Days
{
    internal class Day25(bool isTest) : DayBase(25, isTest)
    {
        private const string NEW_LINE = "\r\n";
        public override string Part1()
        {
            var input = GetInput();
            var keys = new List<List<int>>();
            var locks = new List<List<int>>();

            foreach (var inputLine in input)
            {
                var inputLineAsStrings = inputLine.Split(NEW_LINE);
                var isLock = inputLine.StartsWith("#####");

                if (isLock)
                {
                    var heights = Enumerable.Range(0, inputLineAsStrings[0].Length).ToList().Select(index =>
                    {
                        var height = 0;
                        while (true)
                        {
                            height++;
                            if (inputLineAsStrings[height][index] == '.')
                                break;
                        }
                        return height - 1;
                    }).ToList();

                    locks.Add(heights);
                }
                else
                {
                    var heights = Enumerable.Range(0, inputLineAsStrings[0].Length).ToList().Select(index =>
                    {
                        var height = 0;
                        while (true)
                        {
                            height++;
                            if (inputLineAsStrings[6 - height][index] == '.')
                                break;
                        }
                        return height - 1;
                    }).ToList();

                    keys.Add(heights);
                }
            }

            var goodMatchCount = 0;

            foreach (var @lock in locks)
            {
                foreach (var key in keys)
                {
                    var zipped = @lock.Zip(key, (lockHeight, keyHeight) => lockHeight + keyHeight);
                    var isMatch = zipped.All(x => x <= 5);

                    if (isMatch)
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
