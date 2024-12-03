using AdventOfCodeCommon;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days
{
    internal class Day03(bool isTest) : DayBase(3, isTest)
    {
        public override string Part1()
        {
            var input = GetInput();
            var mulRegex = new Regex("(mul\\((\\d{1,3}),(\\d{1,3})\\))");
            var sum = 0;
            foreach (var line in input)
            {
                var hello1 = mulRegex.Matches(line);
                var hello = hello1.Select(x => x.Groups.Values.Skip(2).Select(x => x.Captures[0].Value).Select(int.Parse).Aggregate(1, (x, y) => x*y));
                sum += hello.Sum();
            }

            return sum.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();
            var mulRegex = new Regex("(mul\\((\\d{1,3}),(\\d{1,3})\\))");
            var doRegex = new Regex("(do\\(\\))");
            var dontRegex = new Regex("(don't\\(\\))");

            var sum = 0;
            var currentDo = true;

            foreach(var line in input)
            {
                var doMatches = doRegex.Matches(line);
                var dontMatches = dontRegex.Matches(line);

                var doDatas = doMatches.Select(x => new Data { Do = true, Index = x.Index });
                var dontDatas = dontMatches.Select(x => new Data { Do = false, Index = x.Index });
                var allDatas = doDatas.Concat(dontDatas).OrderBy(x => x.Index).ToArray();

                var mulMatches = mulRegex.Matches(line)?.ToList();
                if (mulMatches == null || mulMatches.Count == 0)
                    continue;

                foreach (var match in mulMatches)
                {
                    var matchIndex = match.Index;
                    var doData = allDatas.LastOrDefault(x => x.Index < matchIndex);

                    if (doData != null)
                        currentDo = doData.Do;
                    if (!currentDo)
                        continue;

                    var result = match.Groups.Values.Skip(2).Select(x => x.Captures[0].Value).Select(int.Parse).Aggregate(1, (x, y) => x * y);
                    sum += result;
                }
            }

            return sum.ToString();
        }

        private class Data
        {
            public bool Do { get; set; }
            public int Index { get; set; }
        }

        private string[] GetInput()
        {
            return FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
        }
    }
}
