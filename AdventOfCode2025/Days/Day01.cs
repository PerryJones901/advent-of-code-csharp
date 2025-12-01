using AdventOfCodeCommon;

namespace AdventOfCode2025.Days
{
    internal class Day01(bool isTest) : DayBase(1, isTest)
    {
        private const int CODE_LENGTH = 100;
        private const int CODE_START = 50;
        public override string Part1()
        {
            var input = GetInput();
            var currentPos = CODE_START;
            var count = 0;

            foreach(var turn in input)
            {
                currentPos += GetTurnAmount(turn);

                currentPos = GetNormalisedPosition(currentPos);
                if (currentPos == 0)
                    count++;
            }

            return count.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();
            var currentPos = CODE_START;
            var count = 0;
            var startsAtZero = false;

            foreach (var turn in input)
            {
                currentPos += GetTurnAmount(turn);

                count += Math.Abs(currentPos) / CODE_LENGTH;

                if (currentPos <= 0 && !startsAtZero)
                    count++;

                currentPos = GetNormalisedPosition(currentPos);

                startsAtZero = currentPos == 0;
            }

            return count.ToString();
        }

        private static int GetTurnAmount(string turn)
        {
            var turnAmount = int.Parse(turn[1..]);

            var turnLetter = turn[0];
            var multiplier = turnLetter == 'R' ? 1 : -1;

            return multiplier * turnAmount;
        }

        private static int GetNormalisedPosition(int position)
            => ((position % CODE_LENGTH) + CODE_LENGTH) % CODE_LENGTH;

        private string[] GetInput()
            => FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
    }
}
