using AdventOfCode2022.Helpers;

namespace AdventOfCode2022.Days
{
    public static class Day5
    {
        private const string LINE_SEPARATOR = "\r\n";

        public static string Part1Answer() => Part1(GetSeparatedInputFromFile());

        public static string Part1(List<string> input)
        {
            return GetEndConfig(
                GetCrateConfigFromInputString(input[0]),
                GetMoves(input[1])
            );
        }

        public static string Part2Answer() => Part2(GetSeparatedInputFromFile());

        public static string Part2(List<string> input)
        {
            return GetEndConfigPart2(
                GetCrateConfigFromInputStringPart2(input[0]), 
                GetMoves(input[1])
            );
        }

        public static List<Stack<char>> GetCrateConfigFromInputString(string inputString)
        {
            var inputRows = inputString.Split(LINE_SEPARATOR).ToList();
            var heightOfInput = inputRows.Count;

            // Shouldn't hardcode 9, ah well
            var crateStacks = Enumerable.Range(1, 9).Select(x => new Stack<char>()).ToList();

            // Skip row indexed at (Length - 1)

            for(int i = heightOfInput - 2; i >= 0; i--)
            {
                var currentRow = inputRows[i];
                for (int j = 1; j < currentRow.Length; j += 4)
                {
                    var indexInStack = (j - 1)/4;
                    var currentChar = currentRow[j];

                    if (currentChar != ' ') {
                        crateStacks[indexInStack].Push(currentChar);
                    }
                }
            }

            return crateStacks;
        }

        public static List<List<char>> GetCrateConfigFromInputStringPart2(string inputString)
        {
            var inputRows = inputString.Split(LINE_SEPARATOR).ToList();
            var heightOfInput = inputRows.Count;

            // Shouldn't hardcode 9, ah well
            var crateStacks = Enumerable.Range(1, 9).Select(x => new List<char>()).ToList();

            // Skip row indexed at (Length - 1)

            for (int i = heightOfInput - 2; i >= 0; i--)
            {
                var currentRow = inputRows[i];
                for (int j = 1; j < currentRow.Length; j += 4)
                {
                    var indexInStack = (j - 1) / 4;
                    var currentChar = currentRow[j];

                    if (currentChar != ' ')
                    {
                        crateStacks[indexInStack].Insert(0, currentChar);
                    }
                }
            }

            return crateStacks;
        }

        public static List<Move> GetMoves(string inputString)
        {
            return inputString
                .Split(LINE_SEPARATOR)
                .Select(x => x.Split(' '))
                .Select(x => new Move
                {
                    Quantity = int.Parse(x[1]),
                    FromStackIndex = int.Parse(x[3]) - 1,
                    ToStackIndex = int.Parse(x[5]) - 1,
                })
                .ToList();
        }

        public static string GetEndConfig(List<Stack<char>> crates, List<Move> moves)
        {
            foreach (var move in moves)
            {
                for (int i = 0; i < move.Quantity; i++)
                {
                    var crate = crates[move.FromStackIndex].Pop();
                    crates[move.ToStackIndex].Push(crate);
                }
            }

            var output = "";

            foreach (var stack in crates)
            {
                output += stack.Pop();
            }

            return output;
        }

        public static string GetEndConfigPart2(List<List<char>> crates, List<Move> moves)
        {
            foreach (var move in moves)
            {
                var topNChars = crates[move.FromStackIndex].GetRange(0, move.Quantity);
                crates[move.ToStackIndex].InsertRange(0, topNChars);
                crates[move.FromStackIndex].RemoveRange(0, move.Quantity);
            }

            var output = "";

            foreach (var stack in crates)
            {
                if(stack.Count > 0) output += stack[0];
            }

            return output;
        }

        private static List<string> GetSeparatedInputFromFile() => FileInputHelper
                .GetStringListFromFile("Day5.txt", $"{LINE_SEPARATOR}{LINE_SEPARATOR}");

    }

    public class Move
    {
        public int Quantity { get; set; }
        public int FromStackIndex { get; set; }
        public int ToStackIndex { get; set; }
    }
}
