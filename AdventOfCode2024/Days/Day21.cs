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

        private class State
        {
            public string KeyPresses { get; set; } = string.Empty;
        }

        public override string Part1()
        {
            var input = GetInput();
            var sum = GetComplexitySum(input, 1);

            return sum.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();

            return "Part 2";
        }

        private static long GetComplexitySum(string[] codes, int depth)
        {
            var complexityScore = 0L;
            var memoStore = new Dictionary<(char, int), long>();

            foreach (var code in codes)
            {
                var buttonPressCount = 0L;
                var coordsOfCurrentChar = NumPadButtonToCoord['A'];
                var state = new State();

                foreach (var buttonToPress in code)
                {
                    var coordsOfNextButton = NumPadButtonToCoord[buttonToPress];
                    var diffRow = coordsOfNextButton.Item1 - coordsOfCurrentChar.Item1;
                    var diffCol = coordsOfNextButton.Item2 - coordsOfCurrentChar.Item2;

                    // If need to move up, do so first
                    if (diffRow < 0)
                        buttonPressCount += (-1 * diffRow) * GetButtonPressedCount('^', depth, memoStore, state);

                    // Move horizontal
                    if (diffCol < 0)
                        buttonPressCount += (-1 * diffCol) * GetButtonPressedCount('<', depth, memoStore, state);
                    else if (diffCol > 0)
                        buttonPressCount += diffCol * GetButtonPressedCount('>', depth, memoStore, state);

                    // If need to move down, do so now
                    if (diffRow > 0)
                        buttonPressCount += diffRow * GetButtonPressedCount('v', depth, memoStore, state);

                    // Push button
                    buttonPressCount += GetButtonPressedCount('A', depth, memoStore, state);

                    coordsOfCurrentChar = coordsOfNextButton;
                }

                // Now, to add to complexity score
                var numericalPartOfCode = int.Parse(code[..3]);
                complexityScore += numericalPartOfCode * buttonPressCount;
            }

            return complexityScore;
        }

        private static long GetButtonPressedCount(
            char buttonToPress,
            int depth,
            Dictionary<(char, int), long> memoStore,
            State state)
        {
            if (depth == 0)
            {
                state.KeyPresses += buttonToPress;
                return 1L;
            }

            // Try getting memo'd value
            if (memoStore.TryGetValue((buttonToPress, depth), out long memoValue))
                return memoValue;

            // We have been fed a char we need to get to, let's go there
            var buttonCoords = DirectionPadButtonToCoord[buttonToPress];
            var aButtonCoords = DirectionPadButtonToCoord['A'];
            var totalButtonPresses = 0L;

            var diffRow = buttonCoords.Item1 - aButtonCoords.Item1;
            var diffCol = buttonCoords.Item2 - aButtonCoords.Item2;

            // Move down if need be. diffRow is either 0 or 1.
            if (diffRow == 1)
            {
                totalButtonPresses += GetButtonPressedCount('v', depth - 1, memoStore, state);
            }
            // Move left if need be
            if (diffCol < 0)
            {
                totalButtonPresses += (-1 * diffCol)*GetButtonPressedCount('<', depth - 1, memoStore, state);
            }

            // Press A (which presses the movement button in _this_ context)
            totalButtonPresses += GetButtonPressedCount('A', depth - 1, memoStore, state);

            // Move back to the right if need be
            if (diffCol < 0)
            {
                totalButtonPresses += (-1 * diffCol) * GetButtonPressedCount('>', depth - 1, memoStore, state);
            }
            // Move up if need be. diffRow is either 0 or 1.
            if (diffRow == 1)
            {
                totalButtonPresses += GetButtonPressedCount('^', depth - 1, memoStore, state);
            }

            // Press A (which presses the 'A' button in _this_ context)
            totalButtonPresses += GetButtonPressedCount('A', depth - 1, memoStore, state);

            // Add to store, if not done already
            if (!memoStore.ContainsKey((buttonToPress, depth)))
                memoStore[(buttonToPress, depth)] = totalButtonPresses;

            return totalButtonPresses;
        }

        private string[] GetInput()
        {
            return FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
        }
    }
}
