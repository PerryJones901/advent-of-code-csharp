using AdventOfCodeCommon;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days
{
    internal class Day13(bool isTest) : DayBase(13, isTest)
    {
        private const int MAX_BUTTON_PRESSES = 100;
        private const long PART_TWO_OFFSET = 10000000000000;

        public override string Part1()
        {
            // 11349 TOO LOW
            var input = GetInput();
            var totalTokenCount = 0L;

            // For each case, we can increment A button press by 1, and calculate the presses needed on B (if possible).
            // - If token price is smaller than those prior, remember it
            foreach (var prizeCase in input)
            {
                var x = 0L;
                var y = 0L;
                var minTokens = long.MaxValue;

                for (int aButtonPressCount = 0; aButtonPressCount <= MAX_BUTTON_PRESSES; aButtonPressCount++)
                {
                    // Press A button correct number of times
                    x = prizeCase.ButtonA.X * aButtonPressCount;
                    y = prizeCase.ButtonA.Y * aButtonPressCount;

                    // Now, calculate difference to Prize
                    var xDiff = prizeCase.Prize.X - x;
                    var yDiff = prizeCase.Prize.Y - y;

                    // Now, need to check how many times we must press B button to get correct X pos and Y pos separately
                    // We need to check these amounts are equal
                    // Now, to check equality, we must have bButtonPressesForX - bButtonPressesForY = 0
                    // This is xDiff / prizeCase.ButtonB.X - yDiff / prizeCase.ButtonB.Y = 0
                    // Then, to avoid dividing, multiply up by prizeCase.ButtonB.X * prizeCase.ButtonB.Y
                    var buttonPressesDiff = xDiff * prizeCase.ButtonB.Y - yDiff * prizeCase.ButtonB.X;

                    // Check if the amount of B Button presses do NOT line up for both X & Y
                    if (buttonPressesDiff != 0)
                        continue;

                    // Now, check if xDiff is NOT divisible by prizeCase.ButtonB.X (needs to be a whole number of presses)
                    if (xDiff % prizeCase.ButtonB.X != 0)
                        continue;

                    var bButtonPressAmount = xDiff / prizeCase.ButtonB.X;

                    if (bButtonPressAmount > MAX_BUTTON_PRESSES)
                        continue;

                    // Woo! We're good to go
                    var totalTokens = 3L * aButtonPressCount + bButtonPressAmount;

                    // If number of tokens is less than current min, reassign
                    if (minTokens > totalTokens)
                        minTokens = totalTokens;
                }

                if (minTokens < int.MaxValue)
                    totalTokenCount += minTokens;
            }

            return totalTokenCount.ToString();
        }

        public override string Part2()
        {
            var input = GetInput(isPart2: true);
            var totalTokenCount = 0L;

            foreach (var prizeCase in input)
            {
                var det = prizeCase.ButtonA.X * prizeCase.ButtonB.Y - prizeCase.ButtonB.X * prizeCase.ButtonA.Y;

                // if det == 0, throw exception?
                if (det == 0) throw new Exception("Not expected!");

                // Now, if a solution exists, it's unique

                var aButtonPressCountUnnormed = prizeCase.ButtonB.Y * prizeCase.Prize.X - prizeCase.ButtonB.X * prizeCase.Prize.Y;
                var bButtonPressCountUnnormed = -prizeCase.ButtonA.Y * prizeCase.Prize.X + prizeCase.ButtonA.X * prizeCase.Prize.Y;

                // check unnormed values are NOT divisible by det
                if (aButtonPressCountUnnormed % det != 0 || bButtonPressCountUnnormed % det != 0)
                    continue;

                var aButtonPressCount = aButtonPressCountUnnormed / det;
                var bButtonPressCount = bButtonPressCountUnnormed / det;

                // check if either are negative
                if (aButtonPressCount < 0 || bButtonPressCount < 0)
                    continue;

                // Should be good!
                var tokenCount = 3L * aButtonPressCount + bButtonPressCount;
                totalTokenCount += tokenCount;
            }

            return totalTokenCount.ToString();
        }

        private class PrizeCase
        {
            public Coords ButtonA { get; set; }
            public Coords ButtonB { get; set; }
            public Coords Prize { get; set; }
        }

        private class Coords
        {
            public long X { get; set; }
            public long Y { get; set; }
        }

        private PrizeCase[] GetInput(bool isPart2 = false)
        {
            var input = FileInputAssistant.GetStringFromFile(TextInputFilePath);

            var parts = StringHelper.GetRegexCapturesFromInput(
                input,
                @"Button A: X\+(\d+), Y\+(\d+)\r\nButton B: X\+(\d+), Y\+(\d+)\r\nPrize: X=(\d+), Y=(\d+)",
                "\r\n\r\n"
            );

            var offset = isPart2 ? PART_TWO_OFFSET : 0L;

            var prizeCases = parts.Select(x => new PrizeCase
            {
                ButtonA = new Coords { X = int.Parse(x[0]), Y = int.Parse(x[1]) },
                ButtonB = new Coords { X = int.Parse(x[2]), Y = int.Parse(x[3]) },
                Prize = new Coords { X = int.Parse(x[4]) + offset, Y = int.Parse(x[5]) + offset },
            }).ToArray();

            return prizeCases;
        }
    }
}
