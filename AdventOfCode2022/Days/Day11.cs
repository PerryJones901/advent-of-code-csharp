using AdventOfCode2022.Helpers;

namespace AdventOfCode2022.Days
{
    public static class Day11
    {
        private const string LINE_SEPARATOR = "\r\n";

        public static int Part1Answer() =>
            Part1(GetSeparatedInputFromFile());

        public static long Part2Answer() =>
            Part2(GetSeparatedInputFromFile());

        public static int Part1(List<string> input)
        {
            var monkeys = input.Select(GetMonkey).ToList();
            
            // Rounds
            for(int i = 0; i < 20; i++)
                monkeys.ConductRound(withWorryReduction: true);

            return monkeys
                .Select(x => x.InspectionCount)
                .OrderByDescending(x => x)
                .Take(2)
                .Aggregate(1, (acc, count) => acc * count);
        }

        public static long Part2(List<string> input)
        {
            var monkeys = input.Select(GetMonkey).ToList();

            // Rounds
            for (int i = 0; i < 10_000; i++)
                monkeys.ConductRound(withWorryReduction: false);

            return monkeys
                .Select(x => x.InspectionCount)
                .OrderByDescending(x => x)
                .Take(2)
                .Aggregate(1L, (acc, count) => acc * count);
        }

        public static Monkey GetMonkey(string input)
        {
            var inputLines = input.Split(LINE_SEPARATOR);

            var operationString = inputLines[2].Split("Operation: new = ")[1].Split(" ");
            // Assume operationString[0] is "old"
            Func<long, long> operation = (x => x);

            if (operationString[1] == "+")
            {
                var amount = int.Parse(operationString[2]);
                operation = (x => x + amount);
            }
            else
            {
                // Assume *
                if (operationString[2] == "old")
                    operation = (x => x * x);
                else
                {
                    var amount = int.Parse(operationString[2]);
                    operation = (x => x * amount);
                }
            }

            return new Monkey
            {
                ItemsAsWorryScores = inputLines[1]
                    .Split("Starting items: ")[1]
                    .Split(", ")
                    .Select(x => long.Parse(x))
                    .ToList(),
                Operation = operation,
                DivisorForTest = int.Parse(inputLines[3].Split(" ").Last()),
                TrueMonkey = int.Parse(inputLines[4].Split(" ").Last()),
                FalseMonkey = int.Parse(inputLines[5].Split(" ").Last()),
            };
        }

        private static List<string> GetSeparatedInputFromFile() => 
            FileInputHelper.GetStringListFromFile("Day11.txt", $"{LINE_SEPARATOR}{LINE_SEPARATOR}");
    }

    public class Monkey
    {
        public List<long> ItemsAsWorryScores { get; set; } = new();
        public Func<long, long> Operation { get; set; } = (x => x);
        public int DivisorForTest { get; set; }
        public int TrueMonkey { get; set; }
        public int FalseMonkey { get; set; }
        public int InspectionCount { get; set; } = 0;
    }

    public static class MonkeyListExtensions
    {
        public static void ConductRound(this List<Monkey> monkeys, bool withWorryReduction)
        {
            var lowestCommonMultiple = monkeys
                .Select(x => x.DivisorForTest)
                .Aggregate(1, (acc, d) => acc * d);

            foreach(var monkey in monkeys)
            {
                for(int i = 0; i < monkey.ItemsAsWorryScores.Count; i++)
                {
                    monkey.InspectionCount++;
                    var score = monkey.ItemsAsWorryScores[i];
                    var newScore =
                        (monkey.Operation(score) / (withWorryReduction ? 3 : 1)) 
                        % lowestCommonMultiple;
                    // monkey.ItemsAsWorryScores[i] = newScore;

                    // Assume no monkey throws to themself
                    monkeys[
                        newScore % monkey.DivisorForTest == 0 
                            ? monkey.TrueMonkey 
                            : monkey.FalseMonkey
                    ].ItemsAsWorryScores.Add(newScore);
                }
                monkey.ItemsAsWorryScores.Clear();
            }
        }
    }
}
