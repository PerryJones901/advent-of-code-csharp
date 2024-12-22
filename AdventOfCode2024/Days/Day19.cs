using AdventOfCodeCommon;

namespace AdventOfCode2024.Days
{
    internal class Day19(bool isTest) : DayBase(19, isTest)
    {
        private const string NEW_LINE = "\r\n";
        public override string Part1()
        {
            var input = GetInput();
            var towelPatterns = input[0].Split(", ");
            var designs = input[1].Split(NEW_LINE);
            var memoIsDesignValid = new Dictionary<string, bool>();
            foreach (var towelPattern in towelPatterns)
                memoIsDesignValid[towelPattern] = true;

            var countOfValidDesigns = designs.Count(x => IsDesignValid(x, towelPatterns, memoIsDesignValid));

            return countOfValidDesigns.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();
            var towelPatterns = input[0].Split(", ");
            var designs = input[1].Split(NEW_LINE);
            var memoPossibleWaysCount = new Dictionary<string, long>();

            var totalPossibleWaysCount = designs.Sum(x => PossibleWaysCount(x, towelPatterns, memoPossibleWaysCount));

            return totalPossibleWaysCount.ToString();
        }

        private static bool IsDesignValid(string design, string[] towelPatterns, Dictionary<string, bool> memoIsDesignValid)
        {
            if (memoIsDesignValid.TryGetValue(design, out bool memoValue))
                return memoValue;

            var isDesignValid = false;
            foreach (var towelPattern in towelPatterns)
            {
                if (design.StartsWith(towelPattern) && IsDesignValid(design[towelPattern.Length..], towelPatterns, memoIsDesignValid))
                {
                    isDesignValid = true;
                    break;
                }
            }

            if (!memoIsDesignValid.ContainsKey(design))
                memoIsDesignValid[design] = isDesignValid;

            return isDesignValid;
        }

        private static long PossibleWaysCount(string design, string[] towelPatterns, Dictionary<string, long> memoPossibleWaysCount)
        {
            if (design.Length == 0)
                return 1;

            if (memoPossibleWaysCount.TryGetValue(design, out long memoValue))
                return memoValue;

            var possibleWaysCount = 0L;
            foreach (var towelPattern in towelPatterns)
            {
                if (design.StartsWith(towelPattern))
                {
                    possibleWaysCount += PossibleWaysCount(design[towelPattern.Length..], towelPatterns, memoPossibleWaysCount);
                }
            }

            if (!memoPossibleWaysCount.ContainsKey(design))
                memoPossibleWaysCount[design] = possibleWaysCount;

            return possibleWaysCount;
        }

        private string[] GetInput()
        {
            return FileInputAssistant.GetStringArrayFromFile(TextInputFilePath, splitSeparator: $"{NEW_LINE}{NEW_LINE}");
        }
    }
}
