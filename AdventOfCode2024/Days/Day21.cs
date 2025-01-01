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
            // TOO HIGH: 169612
            var input = GetInput();
            var sum = GetComplexitySum(input, 2);

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
                    var buttonsToPress = string.Empty;

                    // If need to move up, do so first
                    if (diffRow < 0)
                    {
                        var count = -diffRow;
                        buttonsToPress += new string([.. Enumerable.Repeat('^', count)]);
                    }

                    // Move horizontal
                    if (diffCol < 0)
                    {
                        var count = -diffCol;
                        buttonsToPress += new string([.. Enumerable.Repeat('<', count)]);
                    }
                    else if (diffCol > 0)
                    {
                        buttonsToPress += new string([.. Enumerable.Repeat('>', diffCol)]);
                    }

                    // If need to move down, do so now
                    if (diffRow > 0)
                    {
                        buttonsToPress += new string([.. Enumerable.Repeat('v', diffRow)]);
                    }

                    // Push button
                    buttonsToPress += 'A';
                    buttonPressCount += GetButtonPressedCount(depth, memoStore, state, buttonsToPress);

                    coordsOfCurrentChar = coordsOfNextButton;
                }

                // Now, to add to complexity score
                Console.WriteLine(state.KeyPresses);
                Console.WriteLine(state.KeyPresses.Length);
                var numericalPartOfCode = int.Parse(code[..3]);
                complexityScore += numericalPartOfCode * buttonPressCount;
            }

            return complexityScore;
        }

        private static long GetButtonPressedCount(
            int depth,
            Dictionary<(char, int), long> memoStore,
            State state,
            string buttonsToPress)
        {
            if (depth == 0)
            {
                state.KeyPresses += buttonsToPress;
                return buttonsToPress.Length;
            }

            // Try getting memo'd value
            //if (memoStore.TryGetValue((buttonToPress, depth), out long memoValue))
            //    return memoValue;

            // We have been fed chars we need to get to, let's do it
            var currentButtonCoords = DirectionPadButtonToCoord['A'];
            var totalButtonPresses = 0L;

            foreach (var buttonToPress in buttonsToPress)
            {
                var nextButtonCoords = DirectionPadButtonToCoord[buttonToPress];
                var diffRow = nextButtonCoords.Item1 - currentButtonCoords.Item1;
                var diffCol = nextButtonCoords.Item2 - currentButtonCoords.Item2;

                var buttonsToPressStr = string.Empty;

                // If need to move down, do so first
                if (diffRow > 0)
                {
                    buttonsToPressStr += new string([.. Enumerable.Repeat('v', diffRow)]);
                }

                // Move horizontal
                if (diffCol < 0)
                {
                    var count = -diffCol;
                    buttonsToPressStr += new string([.. Enumerable.Repeat('<', count)]);
                }
                else if (diffCol > 0)
                {
                    buttonsToPressStr += new string([.. Enumerable.Repeat('>', diffCol)]);
                }

                // If need to move up, do so last
                if (diffRow < 0)
                {
                    var count = -diffRow;
                    buttonsToPressStr += new string([.. Enumerable.Repeat('^', count)]);
                }

                // Press A
                buttonsToPressStr += 'A';

                totalButtonPresses += GetButtonPressedCount(depth - 1, memoStore, state, buttonsToPressStr);

                currentButtonCoords = nextButtonCoords;
            }

            // Add to store, if not done already
            //if (!memoStore.ContainsKey((buttonToPress, depth)))
            //    memoStore[(buttonToPress, depth)] = totalButtonPresses;

            return totalButtonPresses;
        }

        private string[] GetInput()
        {
            return FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
        }
    }
}
