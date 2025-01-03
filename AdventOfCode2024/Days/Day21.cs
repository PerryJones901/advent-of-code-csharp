﻿using AdventOfCodeCommon;

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

        public override string Part1()
        {
            var input = GetInput();
            var sum = GetComplexitySum(input, depth: 2);

            return sum.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();
            var sum = GetComplexitySum(input, depth: 25);

            return sum.ToString();
        }

        private static long GetComplexitySum(string[] codes, int depth)
        {
            var complexityScore = 0L;
            var memoStore = new Dictionary<(string, int), long>();

            foreach (var code in codes)
            {
                var buttonPressCount = 0L;
                var coordsOfCurrentChar = NumPadButtonToCoord['A'];

                foreach (var buttonToPress in code)
                {
                    var coordsOfNextButton = NumPadButtonToCoord[buttonToPress];
                    var diffRow = coordsOfNextButton.Item1 - coordsOfCurrentChar.Item1;
                    var diffCol = coordsOfNextButton.Item2 - coordsOfCurrentChar.Item2;
                    var buttonsToPressOnNextDirPad = GetButtonsToPress(diffRow, diffCol, priorityDirection: VerticalDirection.Up);
                    var buttonsToPressOnHumanDirPadCount = GetMostEfficientButtonPressesCount(depth, memoStore, buttonsToPressOnNextDirPad);

                    // Now, need to see if possible through alt route. (i.e. down prio)
                    // if (3, 0) will be passed in alt route, we cannot take it. Otherwise, take it.

                    var altPassesThrough30 = (coordsOfCurrentChar.Item1 == 3 && coordsOfNextButton.Item2 == 0)
                        || (coordsOfCurrentChar.Item2 == 0 && coordsOfNextButton.Item1 == 3);

                    if (!altPassesThrough30)
                    {
                        // Attempt alt path
                        var buttonsToPressOnNextDirPadAlt = GetButtonsToPress(
                            diffRow, diffCol, priorityDirection: VerticalDirection.Down);
                        var buttonsToPressOnHumanDirPadCountAlt = GetMostEfficientButtonPressesCount(depth, memoStore, buttonsToPressOnNextDirPadAlt);

                        buttonsToPressOnHumanDirPadCount = new List<long>([
                            buttonsToPressOnHumanDirPadCount,
                            buttonsToPressOnHumanDirPadCountAlt
                        ]).Min();
                    }

                    buttonPressCount += buttonsToPressOnHumanDirPadCount;

                    coordsOfCurrentChar = coordsOfNextButton;
                }

                // Now, to add to complexity score
                Console.WriteLine(buttonPressCount);
                var numericalPartOfCode = int.Parse(code[..3]);
                complexityScore += numericalPartOfCode * buttonPressCount;
            }

            return complexityScore;
        }

        private static long GetMostEfficientButtonPressesCount(
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

                var buttonsToPressStr = GetButtonsToPress(
                    diffRow, diffCol, priorityDirection: VerticalDirection.Down);

                // Need to decide on if we want the alt included.
                // If the alt path goes through the forbidden (0, 0) space, we cannot take it.
                // This occurs if '<' is one of the buttons.
                // First, run the normal one.
                var buttonPressesCount = GetMostEfficientButtonPressesCount(depth - 1, memoStore, buttonsToPressStr);
                if (currentButtonCoords != (1, 0) && nextButtonCoords != (1, 0))
                {
                    var altButtonsToPressStr = GetButtonsToPress(diffRow, diffCol, priorityDirection: VerticalDirection.Up);
                    var altButtonPressesCount = GetMostEfficientButtonPressesCount(depth - 1, memoStore, altButtonsToPressStr);
                    buttonPressesCount = new List<long>([buttonPressesCount, altButtonPressesCount]).Min();
                }

                totalButtonPressesCount += buttonPressesCount;

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
