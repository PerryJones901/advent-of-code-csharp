using AdventOfCode2022.Helpers;

namespace AdventOfCode2022.Days
{
    public static class Day10
    {
        public static int Part1Answer() =>
            Part1(GetSeparatedInputFromFile());

        public static string Part2Answer() =>
            Part2(GetSeparatedInputFromFile());

        public static int Part1(List<string> input)
        {
            var operations = input.Select(GetOpFromString);
            var cycleNum = 0;
            var xRegValue = 1;
            var sumOfSignalStrengths = 0;

            foreach(var operation in operations)
            {
                if(operation.OpType == OpType.Noop)
                {
                    cycleNum++;
                    if (cycleNum % 40 == 20)
                    sumOfSignalStrengths += (xRegValue * cycleNum);
                }
                else if (operation.OpType == OpType.Addx)
                {
                    cycleNum++;
                    if (cycleNum % 40 == 20)
                        sumOfSignalStrengths += (xRegValue * cycleNum);

                    cycleNum++;
                    if (cycleNum % 40 == 20)
                        sumOfSignalStrengths += (xRegValue * cycleNum);

                    if (operation.Amount == null)
                        throw new Exception("Amount should not be null for Addx Op");
                    xRegValue += operation.Amount ?? 0;
                }
            }

            return sumOfSignalStrengths;
        }

        private static string GetPixel(int xRegValue, int cycleNum)
        {
            var pixelPosition = (cycleNum - 1) % 40;
            return (xRegValue - 1 <= pixelPosition && pixelPosition <= xRegValue + 1)
                ? "#"
                : ".";
        }

        public static string Part2(List<string> input)
        {
            var operations = input.Select(GetOpFromString);
            var cycleNum = 0;
            var xRegValue = 1;
            var sumOfSignalStrengths = 0;
            var pixelsString = "";

            foreach (var operation in operations)
            {
                if (operation.OpType == OpType.Noop)
                {
                    cycleNum++;
                    if (cycleNum % 40 == 20)
                        sumOfSignalStrengths += (xRegValue * cycleNum);
                    pixelsString += GetPixel(xRegValue, cycleNum);
                }
                else if (operation.OpType == OpType.Addx)
                {
                    cycleNum++;
                    if (cycleNum % 40 == 20)
                        sumOfSignalStrengths += (xRegValue * cycleNum);
                    pixelsString += GetPixel(xRegValue, cycleNum);

                    cycleNum++;
                    if (cycleNum % 40 == 20)
                        sumOfSignalStrengths += (xRegValue * cycleNum);
                    pixelsString += GetPixel(xRegValue, cycleNum);

                    if (operation.Amount == null)
                        throw new Exception("Amount should not be null for Addx Op");
                    xRegValue += operation.Amount ?? 0;
                }
            }

            var output = string.Join("\r\n", Enumerable
                .Range(0, 6)
                .Select(x => pixelsString[(x * 40)..((x + 1) * 40)]));

            return output;
        }

        public static Operation GetOpFromString(string input) =>
            new()
            { 
                OpType = GetOpTypeFromString(input.Split(' ')[0]), 
                Amount = input.Split(' ').Length > 1 ? int.Parse(input.Split(' ')[1]) : null 
            };

        public static List<string> GetSeparatedInputFromFile() => 
            FileInputHelper.GetStringListFromFile("Day10.txt");

        public class Operation
        {
            public OpType OpType { get; set; } = OpType.Noop;
            public int? Amount { get; set; } = null;
        }

        public static OpType GetOpTypeFromString(string input)
        {
            return input switch
            {
                "noop" => OpType.Noop,
                "addx" => OpType.Addx,
                _ => throw new Exception("Unexpected OpType string"),
            };
        }

        public enum OpType
        {
            Noop, Addx
        }
    }
}
