using AdventOfCodeCommon;

namespace AdventOfCode2015.Days
{
    internal class Day06(bool isTest) : DayBase(6, isTest)
    {
        public override string Part1()
        {
            var input = GetInput();
            var lightGrid = Enumerable.Range(0, 1000).Select(_ => GetBoolArray()).ToArray();

            foreach (var inputLine in input)
            {
                var modifierType = GetModifierType(inputLine);
                var regionCorners = GetRegionCorners(inputLine);

                for (int row = regionCorners.Item1.Item1; row <= regionCorners.Item2.Item1; row++)
                {
                    for (int col = regionCorners.Item1.Item2; col <= regionCorners.Item2.Item2; col++)
                    {
                        lightGrid[row][col] = modifierType switch
                        {
                            ModifierType.Off => false,
                            ModifierType.On => true,
                            ModifierType.Toggle => !lightGrid[row][col],
                            _ => throw new Exception("Unknown type"),
                        };
                    }
                }
            }

            var lightOnCount = lightGrid.Sum(x => x.Count(x => x));

            return lightOnCount.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();
            var lightGrid = Enumerable.Range(0, 1000).Select(_ => GetIntArray()).ToArray();

            foreach (var inputLine in input)
            {
                var modifierType = GetModifierType(inputLine);
                var regionCorners = GetRegionCorners(inputLine);

                for (int row = regionCorners.Item1.Item1; row <= regionCorners.Item2.Item1; row++)
                {
                    for (int col = regionCorners.Item1.Item2; col <= regionCorners.Item2.Item2; col++)
                    {
                        lightGrid[row][col] = modifierType switch
                        {
                            ModifierType.Off => Math.Max(0, lightGrid[row][col] - 1),
                            ModifierType.On => lightGrid[row][col] + 1,
                            ModifierType.Toggle => lightGrid[row][col] + 2,
                            _ => throw new Exception("Unknown type"),
                        };
                    }
                }
            }

            var brightnessLevel = lightGrid.Sum(x => x.Sum(x => (long)x));

            return brightnessLevel.ToString();
        }

        private static ModifierType GetModifierType(string inputLine)
        {
            if (inputLine.StartsWith("turn on"))
                return ModifierType.On;

            if (inputLine.StartsWith("turn off"))
                return ModifierType.Off;

            if (inputLine.StartsWith("toggle"))
                return ModifierType.Toggle;

            throw new Exception("Couldn't get modifier type");
        }

        private static ((int, int), (int, int)) GetRegionCorners(string inputLine)
        {
            var splitOnSpaces = inputLine.Split(' ');
            var partCount = splitOnSpaces.Length;

            var firstCornerCoords = splitOnSpaces[partCount - 3].Split(',').Select(int.Parse).ToList();
            var secondCornerCoords = splitOnSpaces[partCount - 1].Split(',').Select(int.Parse).ToList();

            return (
                (firstCornerCoords[0], firstCornerCoords[1]),
                (secondCornerCoords[0], secondCornerCoords[1])
            );
        }

        private enum ModifierType
        {
            Off,
            On,
            Toggle,
        }

        private static bool[] GetBoolArray() => [.. Enumerable.Repeat(false, 1000)];
        private static int[] GetIntArray() => [.. Enumerable.Repeat(0, 1000)];

        private string[] GetInput()
            => FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
    }
}
