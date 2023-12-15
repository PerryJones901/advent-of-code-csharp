using AdventOfCode2022.Helpers;

namespace AdventOfCode2022.Days
{
    public static class Day2
    {
        public static int Part1Answer() =>
            GetSeparatedInputFromFile().Select(x => ScoreForRoundPart1(x)).Sum();

        public static int Part2Answer() =>
            GetSeparatedInputFromFile().Select(x => ScoreForRoundPart2(x)).Sum();

        public static List<string> GetSeparatedInputFromFile() => FileInputHelper
                .GetStringListFromFile("Day2.txt");

        public static int ScoreForRoundPart1(string roundString)
        {
            var symbols = roundString.Split(' ');
            var opponentSymbol = symbols[0];
            var playerSymbol = symbols[1];

            var score = 0;
            if (playerSymbol == "X")
            {
                score += 1;
                if (opponentSymbol == "A") score += 3;
                if (opponentSymbol == "C") score += 6;
            }
            else if (playerSymbol == "Y")
            {
                score += 2;
                if (opponentSymbol == "B") score += 3;
                if (opponentSymbol == "A") score += 6;
            }
            else if (playerSymbol == "Z") 
            { 
                score += 3;
                if (opponentSymbol == "C") score += 3;
                if (opponentSymbol == "B") score += 6;
            }

            return score;
        }

        public static int ScoreForRoundPart2(string roundString)
        {
            var symbols = roundString.Split(' ');
            var opponentSymbol = symbols[0];
            var outcome = symbols[1];

            var score = 0;
            if (outcome == "X")
            {
                // Must lose
                if (opponentSymbol == "A") score += 3;
                if (opponentSymbol == "B") score += 1;
                if (opponentSymbol == "C") score += 2;
            }
            else if (outcome == "Y")
            {
                // Must draw
                score += 3;
                if (opponentSymbol == "A") score += 1;
                if (opponentSymbol == "B") score += 2;
                if (opponentSymbol == "C") score += 3;
            }
            else if (outcome == "Z")
            {
                // Must win
                score += 6;
                if (opponentSymbol == "A") score += 2;
                if (opponentSymbol == "B") score += 3;
                if (opponentSymbol == "C") score += 1;
            }

            return score;
        }
    }
}
