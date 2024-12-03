using AdventOfCodeCommon;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days
{
    internal class Day03(bool isTest) : DayBase(3, isTest)
    {
        public override string Part1()
        {
            var input = GetInput();
            var mulRegex = GetMulRegex();
            var sum = input
                .Select(inputLine =>
                    mulRegex
                        .Matches(inputLine)
                        .Select(y => GetProductFromMatch(y))
                        .Sum()
                ).Sum();

            return sum.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();
            var mulRegex = GetMulRegex();
            var doRegex = new Regex("do\\(\\)");
            var dontRegex = new Regex("don't\\(\\)");

            var sum = 0;
            var isCurrentInstructionDo = true;

            foreach(var line in input)
            {
                var doInstructions = doRegex
                    .Matches(line)
                    .Select(x => new Instruction { IsDo = true, Index = x.Index });
                var dontInstructions = dontRegex
                    .Matches(line)
                    .Select(x => new Instruction { IsDo = false, Index = x.Index });

                var allInstructions = doInstructions
                    .Concat(dontInstructions)
                    .OrderBy(x => x.Index)
                    .ToArray();

                var mulMatches = mulRegex.Matches(line).ToArray() ?? [];

                foreach (var match in mulMatches)
                {
                    var matchIndex = match.Index;
                    var lastInstructionInCurrentLine = allInstructions.LastOrDefault(x => x.Index < matchIndex);

                    if (lastInstructionInCurrentLine != null)
                        isCurrentInstructionDo = lastInstructionInCurrentLine.IsDo;
                    if (!isCurrentInstructionDo)
                        continue;

                    var result = GetProductFromMatch(match);
                    sum += result;
                }
            }

            return sum.ToString();
        }

        private static Regex GetMulRegex() => new("mul\\((\\d{1,3}),(\\d{1,3})\\)");

        private static int GetProductFromMatch(Match match)
            => match.Groups.Values
                .Skip(1) // Skip the 'whole match' group (e.g. "mul(2,3)")
                .Select(x => x.Captures[0].Value)
                .Select(int.Parse)
                .Aggregate(1, (x, y) => x * y);

        private class Instruction
        {
            public bool IsDo { get; set; }
            public int Index { get; set; }
        }

        private string[] GetInput()
        {
            return FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
        }
    }
}
