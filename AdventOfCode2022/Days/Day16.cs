using AdventOfCode2022.Helpers;

namespace AdventOfCode2022.Days
{
    public static class Day16
    {
        public static int Part1Answer() =>
            Part1(GetInputParamListsFromFile());

        //public static int Part2Answer() =>
        //    Part2(GetInputParamListsFromFile());

        public static int Part1(List<List<string>> input)
        {
            var valves = GetValves(input);

            // from indexOfU to indexOfV
            var distList = Enumerable.Range(0, valves.Count)
                .Select(x => Enumerable.Repeat(int.MaxValue, valves.Count));

            //foreach(var valve in valves)
            //{
            //    distList[]
            //}

            return 0;
        }

        public static int Part2(List<string> input)
        {
            return 0;
        }

        private static List<Valve> GetValves(List<List<string>> input)
        {
            var valves = input.ToDictionary(
                x => x[0], 
                x => new Valve
                    {
                        Name = x[0],
                        FlowRate = int.Parse(x[1]),
                    }
                );
            foreach(var inputLine in input)
            {
                valves[inputLine[0]].Valves = inputLine[2]
                    .Split(", ")
                    .Select(x => valves[x])
                    .ToList();
            }
            return valves.Select(x => x.Value).ToList();
        }

        private static List<string> GetSeparatedInputFromFile() => 
            FileInputHelper.GetStringListFromFile("Day16.txt");

        private static List<List<string>> GetInputParamListsFromFile() =>
            FileInputHelper.GetParamListsFromRegexFromFile(
                "Day16.txt",
                @"(?:Valve )(\w+)(?: has flow rate=)(\d+)(?:; tunnels? leads? to valves? )(.*)"
            );
    }

    public class Valve
    {
        public string Name { get; set; } = "";
        public int FlowRate { get; set; } = 0;
        public bool Open { get; set; } = false;
        public List<Valve> Valves { get; set; } = new List<Valve>();
    }
}
