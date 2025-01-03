using AdventOfCodeCommon;

namespace AdventOfCode2024.Days
{
    internal class Day21(bool isTest) : DayBase(21, isTest)
    {
        private static Dictionary<char, (int, int)> NumPadButtonToCoord = new() {
            { '7', (0, 0) },    { '8', (0, 1) }, { '9', (0, 2) },
            { '4', (1, 0) },    { '5', (1, 1) }, { '6', (1, 2) },
            { '1', (2, 0) },    { '2', (2, 1) }, { '3', (2, 2) },
                                { '0', (3, 1) }, { 'A', (3, 2) },
        };

        private static Dictionary<char, (int, int)> DirectionPadButtonToCoord = new() {
                                { '^', (0, 1) }, { 'A', (0, 2) },
            { '<', (1, 0) },    { 'v', (1, 1) }, { '>', (1, 2) },
        };

        public override string Part1() => GetResult(depth: 2);
        public override string Part2() => GetResult(depth: 25);

        private string GetResult(int depth)
        {
            var input = GetInput();
            var sum = GetComplexitySum(input, depth);

            return sum.ToString();
        }

        private static long GetComplexitySum(string[] codes, int depth)
        {
            var complexityScore = 0L;
            var memoStore = new Dictionary<(string, int), long>();

            foreach (var code in codes)
            {
                var totalHumanPressesCount = 0L;
                var coordsOfCurrentChar = NumPadButtonToCoord['A'];

                foreach (var buttonToPress in code)
                {
                    var coordsOfNextButton = NumPadButtonToCoord[buttonToPress];
                    var diffRow = coordsOfNextButton.Item1 - coordsOfCurrentChar.Item1;
                    var diffCol = coordsOfNextButton.Item2 - coordsOfCurrentChar.Item2;

                    // Get buttons to press on first direction pad
                    // We prioritise the "Up" direction here, as it will always provide safe inputs (i.e. we won't cross
                    //  the forbidden empty square in the bottom left)
                    var buttonsPressedOnNextPad = GetButtonsToPress(
                        diffRow, diffCol, priorityDirection: VerticalDirection.Up);

                    var humanPressesCount = GetHumanButtonPressCount(depth, memoStore, buttonsPressedOnNextPad);

                    // Now, if it's safe to so, we can swap the vertical and horizontal ordering.
                    // This is only safe if the empty space, at (3, 0), will NOT be passed by swapping the order.
                    // We check if it IS passed by checking if one button coords has row = 3, and the other col = 0.

                    var altPassesThroughEmptySquare = (coordsOfCurrentChar.Item1 == 3 && coordsOfNextButton.Item2 == 0)
                        || (coordsOfCurrentChar.Item2 == 0 && coordsOfNextButton.Item1 == 3);

                    if (!altPassesThroughEmptySquare)
                    {
                        var buttonsPressedOnNextPadAlt = GetButtonsToPress(
                            diffRow, diffCol, priorityDirection: VerticalDirection.Down);
                        var humanPressesCountAlt = GetHumanButtonPressCount(depth, memoStore, buttonsPressedOnNextPadAlt);

                        if (humanPressesCountAlt < humanPressesCount)
                            humanPressesCount = humanPressesCountAlt;
                    }

                    totalHumanPressesCount += humanPressesCount;

                    coordsOfCurrentChar = coordsOfNextButton;
                }

                var numericalPartOfCode = int.Parse(code[..3]);
                complexityScore += numericalPartOfCode * totalHumanPressesCount;
            }

            return complexityScore;
        }

        private static long GetHumanButtonPressCount(
            int depth,
            Dictionary<(string, int), long> memoStore,
            string buttonsToPress)
        {
            if (depth == 0)
                return buttonsToPress.Length;

            if (memoStore.TryGetValue((buttonsToPress, depth), out var memoCount))
                return memoCount;

            var currentButtonCoords = DirectionPadButtonToCoord['A'];
            var totalButtonPressesCount = 0L;

            foreach (var buttonToPress in buttonsToPress)
            {
                var nextButtonCoords = DirectionPadButtonToCoord[buttonToPress];
                var diffRow = nextButtonCoords.Item1 - currentButtonCoords.Item1;
                var diffCol = nextButtonCoords.Item2 - currentButtonCoords.Item2;

                // Get buttons to press on next direction pad
                // We prioritise the "Down" direction here, as it will always provide safe inputs (i.e. we won't cross
                //  the forbidden empty square in the top left)
                var buttonsPressedOnNextPad = GetButtonsToPress(
                    diffRow, diffCol, priorityDirection: VerticalDirection.Down);
                var humanPressesCount = GetHumanButtonPressCount(depth - 1, memoStore, buttonsPressedOnNextPad);

                // Now, if it's safe to so, we can swap the vertical and horizontal ordering.
                // We check if the alt path would pass through the empty square. It is sufficient to check if
                //  either the current or next button is '<'. In other words, has coords (1, 0).
                var altPassesThroughEmptySquare = currentButtonCoords == (1, 0) || nextButtonCoords == (1, 0);

                if (!altPassesThroughEmptySquare)
                {
                    var buttonsPressedOnNextPadAlt = GetButtonsToPress(
                        diffRow, diffCol, priorityDirection: VerticalDirection.Up);
                    var humanPressesCountAlt = GetHumanButtonPressCount(depth - 1, memoStore, buttonsPressedOnNextPadAlt);
                    if (humanPressesCountAlt < humanPressesCount)
                        humanPressesCount = humanPressesCountAlt;
                }

                totalButtonPressesCount += humanPressesCount;

                currentButtonCoords = nextButtonCoords;
            }

            // Add to store, if not done already
            if (!memoStore.ContainsKey((buttonsToPress, depth)))
                memoStore[(buttonsToPress, depth)] = totalButtonPressesCount;

            return totalButtonPressesCount;
        }

        private static string GetButtonsToPress(int diffRow, int diffCol, VerticalDirection priorityDirection)
        {
            // Get buttons to press in each direction
            var verticalString = GetRepeatedChar(diffRow > 0 ? 'v' : '^', diffRow);
            var horizontalString = GetRepeatedChar(diffCol > 0 ? '>' : '<', diffCol);

            // Decide if we will press the vertical buttons first
            var isVerticalFirst = priorityDirection == VerticalDirection.Down
                ? diffRow > 0
                : diffRow < 0;

            // Assemble string, with 'A' pressed at the end
            var buttonsToPressStr = isVerticalFirst
                ? $"{verticalString}{horizontalString}A"
                : $"{horizontalString}{verticalString}A";

            return buttonsToPressStr;
        }

        private static string GetRepeatedChar(char c, int signedDiff)
            => new([.. Enumerable.Repeat(c, Math.Abs(signedDiff))]);

        private enum VerticalDirection
        {
            Up,
            Down
        }

        private string[] GetInput()
        {
            return FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
        }
    }
}
