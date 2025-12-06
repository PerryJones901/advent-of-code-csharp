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
                long answer = numberGrid[0][col];

                for (int row = 1; row < numberGrid.Length; row++)
                {
                    answer = op == "*"
                        ? answer * numberGrid[row][col]
                        : answer + numberGrid[row][col];
                }

                grandTotal += answer;
            }

            return grandTotal.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();

            var operators = input.Last().Select((c, i) => new { Char = c, ColIndexInCharGrid = i })
                .Where(x => x.Char == '+' || x.Char == '*')
                .ToList();

            var grandTotal = 0L;

            for (int opId = 0; opId < operators.Count(); opId++)
            {
                var op = operators[opId];
                var nextOp = opId + 1 < operators.Count()
                    ? operators[opId + 1]
                    : null;

                var nextBlankColIndexInCharGrid = nextOp != null
                    ? nextOp.ColIndexInCharGrid - 1
                    : input[0].Length;

                var answer = GetNumberAtColumn(input, op.ColIndexInCharGrid);

                for (int col = op.ColIndexInCharGrid + 1; col < nextBlankColIndexInCharGrid; col++)
                {
                    var number = GetNumberAtColumn(input, col);
                    answer = op.Char == '*'
                        ? answer * number
                        : answer + number;
                }

                grandTotal += answer;
            }

            return grandTotal.ToString();
        }

        private static long GetNumberAtColumn(string[] input, int col)
        {
            var numberCharList = new List<char>();

            for (int row = 0; row < input.Length - 1; row++)
            {
                var @char = input[row][col];

                if (@char == ' ')
                    continue;

                numberCharList.Add(@char);
            }

            var numberString = new string(numberCharList.ToArray());
            var number = long.Parse(numberString);

            return number;
        }

        private string[] GetInput()
            => FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
    }
}
