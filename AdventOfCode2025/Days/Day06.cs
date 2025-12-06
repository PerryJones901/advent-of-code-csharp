using AdventOfCodeCommon;

namespace AdventOfCode2025.Days
{
    internal class Day06(bool isTest) : DayBase(6, isTest)
    {
        public override string Part1()
        {
            var input = GetInput();
            var inputGrid = input.Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries));
            var numberGrid = inputGrid
                .SkipLast(1)
                .Select(x => x.Select(y => int.Parse(y)).ToArray())
                .ToArray();
            var operatorRow = inputGrid.Last().ToArray();

            var grandTotal = 0L;

            for (int col = 0; col < operatorRow.Length; col++)
            {
                var op = operatorRow[col];
                var numbers = numberGrid.Select(x => (long)x[col]);
                long answer = GetAnswer(numbers, op[0]);

                grandTotal += answer;
            }

            return grandTotal.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();

            var operators = input
                .Last()
                .Select((c, i) => new { Char = c, ColIndexInCharGrid = i })
                .Where(x => x.Char == '+' || x.Char == '*')
                .ToList();

            var grandTotal = 0L;

            for (int opId = 0; opId < operators.Count; opId++)
            {
                var op = operators[opId];
                var nextOp = opId + 1 < operators.Count
                    ? operators[opId + 1]
                    : null;

                var nextBlankColIndexInCharGrid = nextOp != null
                    ? nextOp.ColIndexInCharGrid - 1
                    : input[0].Length;

                var numbers = Enumerable.Range(op.ColIndexInCharGrid, nextBlankColIndexInCharGrid - op.ColIndexInCharGrid)
                    .Select(col => GetNumberAtColumn(input, col));

                var answer = GetAnswer(numbers, op.Char);

                grandTotal += answer;
            }

            return grandTotal.ToString();
        }

        private static long GetNumberAtColumn(string[] input, int col)
        {
            var chars = input
                .SkipLast(1)
                .Select(x => x[col])
                .Where(x => x != ' ')
                .ToArray();

            var number = long.Parse(chars);

            return number;
        }

        private static long GetAnswer(IEnumerable<long> numbers, char op)
            =>  op == '*'
                ? numbers.Aggregate((a, b) => a * b)
                : numbers.Aggregate((a, b) => a + b);

        private string[] GetInput()
            => FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
    }
}
