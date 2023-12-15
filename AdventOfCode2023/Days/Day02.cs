using AdventOfCode2023.Helpers;

namespace AdventOfCode2023.Days;

public abstract class Day02
{
    private const string RED = "red";
    private const string GREEN = "green";
    private const string BLUE = "blue";

    // Part 1:
    private const int MAX_RED = 12;
    private const int MAX_GREEN = 13;
    private const int MAX_BLUE = 14;

    public static int Part1Answer() =>
        Part1(GetSeparatedInputFromFile());

    public static int Part2Answer() =>
        Part2(GetSeparatedInputFromFile());

    public static int Part1(List<string> input)
    {
        var idSum = 0;

        foreach (var gameInputString in input)
        {
            var gameStringAtoms = gameInputString.Split(' ');

            var gameId = GetGameId(gameStringAtoms);
            var isGamePossible = GetIsGamePossible(gameStringAtoms.Skip(2));

            if (isGamePossible) idSum += gameId;
        }

        return idSum;
    }

    private static int GetGameId(string[] gameStringAtoms)
    {
        // Of format "Game 1: ...", "Game 2: ..." etc
        return int.Parse(gameStringAtoms[1].Replace(":", ""));
    }

    private static bool GetIsGamePossible(IEnumerable<string> gameStringAtoms)
    {
        var isGamePossible = true;
        var atomPairs = gameStringAtoms.Zip(
            gameStringAtoms.Skip(1),
            (prev, current) => new { prev, current }
        );

        foreach (var atomPair in atomPairs)
        {
            if (atomPair.current.StartsWith(RED))
            {
                var amount = int.Parse(atomPair.prev);
                if (amount > MAX_RED)
                {
                    isGamePossible = false;
                    break;
                }
            }
            else if (atomPair.current.StartsWith(GREEN))
            {
                var amount = int.Parse(atomPair.prev);
                if (amount > MAX_GREEN)
                {
                    isGamePossible = false;
                    break;
                }
            }
            else if (atomPair.current.StartsWith(BLUE))
            {
                var amount = int.Parse(atomPair.prev);
                if (amount > MAX_BLUE)
                {
                    isGamePossible = false;
                    break;
                }
            }
        }

        return isGamePossible;
    }

    public static int Part2(List<string> input)
    {
        var powerSum = 0;

        foreach (var gameInputString in input)
        {
            var gameStringAtoms = gameInputString.Split(' ');
            powerSum += GetGamePower(gameStringAtoms);
        }

        return powerSum;
    }

    private static int GetGamePower(IEnumerable<string> gameStringAtoms)
    {
        var atomPairs = gameStringAtoms.Zip(
            gameStringAtoms.Skip(1),
            (prev, current) => new { prev, current }
        );

        int minRed = 0, minGreen = 0, minBlue = 0;

        foreach (var atomPair in atomPairs)
        {
            if (atomPair.current.StartsWith(RED))
            {
                var amount = int.Parse(atomPair.prev);
                if (amount > minRed)
                    minRed = amount;
            }
            else if (atomPair.current.StartsWith(GREEN))
            {
                var amount = int.Parse(atomPair.prev);
                if (amount > minGreen)
                    minGreen = amount;
            }
            else if (atomPair.current.StartsWith(BLUE))
            {
                var amount = int.Parse(atomPair.prev);
                if (amount > minBlue)
                    minBlue = amount;
            }
        }

        return minRed * minGreen * minBlue;
    }

    private static List<string> GetSeparatedInputFromFile() => 
        FileInputHelper.GetStringListFromFile("Day02.txt");
}
