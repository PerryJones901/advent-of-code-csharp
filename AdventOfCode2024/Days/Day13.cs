using AdventOfCodeCommon;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days
{
    internal class Day13(bool isTest) : DayBase(13, isTest)
    {
        private const long PART_TWO_OFFSET = 10000000000000;

        public override string Part1() => GetTotalTokenCount();
        public override string Part2() => GetTotalTokenCount(prizeOffset: PART_TWO_OFFSET);

        private string GetTotalTokenCount(long prizeOffset = 0)
        {
            var input = GetInput(prizeOffset);
            var totalTokenCount = 0L;

            foreach (var prizeCase in input)
            {
                /*
                 * Some cheeky Linear Algebra here
                 * Solutions are in form:
                 * ┌ ┐                         ┌            ┐ ┌   ┐
                 *  a              1              B.Y  -B.X    P.X
                 *  b  =  -------------------    -A.Y   A.X    P.Y
                 * └ ┘    (A.X*B.Y - B.X*A.Y)  └            ┘ └   ┘
                 * 
                 * where:
                 * a = number of A presses
                 * b = number of B presses
                 * A, B are the vectors of a single button press of A and B respectively
                 * P is the vector of where the prize lives
                 * 
                 * now, the dividing of the det will mean we leave the nice world of longs and into floats/doubles: not nice.
                 * we can avoid that by checking divisibility of the coords of the "unnormed" vector to the 1/det's right hand side.
                 */
                var det = prizeCase.ButtonA.X * prizeCase.ButtonB.Y - prizeCase.ButtonB.X * prizeCase.ButtonA.Y;

                // if det == 0, assume we don't handle
                if (det == 0) throw new Exception("det is zero");

                var aButtonPressCountUnnormed = prizeCase.ButtonB.Y * prizeCase.Prize.X - prizeCase.ButtonB.X * prizeCase.Prize.Y;
                var bButtonPressCountUnnormed = -prizeCase.ButtonA.Y * prizeCase.Prize.X + prizeCase.ButtonA.X * prizeCase.Prize.Y;

                // continue if unnormed values are NOT divisible by det
                if (aButtonPressCountUnnormed % det != 0 || bButtonPressCountUnnormed % det != 0)
                    continue;

                var aButtonPressCount = aButtonPressCountUnnormed / det;
                var bButtonPressCount = bButtonPressCountUnnormed / det;

                // continue if either are negative
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

        private PrizeCase[] GetInput(long offset)
        {
            var input = FileInputAssistant.GetStringFromFile(TextInputFilePath);

            var parts = StringHelper.GetRegexCapturesFromInput(
                input,
                @"Button A: X\+(\d+), Y\+(\d+)\r\nButton B: X\+(\d+), Y\+(\d+)\r\nPrize: X=(\d+), Y=(\d+)",
                "\r\n\r\n"
            );

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
