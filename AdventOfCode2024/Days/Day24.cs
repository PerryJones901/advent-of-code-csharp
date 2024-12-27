using AdventOfCodeCommon;

namespace AdventOfCode2024.Days
{
    internal class Day24(bool isTest) : DayBase(24, isTest)
    {
        private const string NEW_LINE = "\r\n";
        public override string Part1()
        {
            var input = GetInput();
            var wireStates = input[0].Split(NEW_LINE).ToDictionary(
                x => x.Split(": ")[0],
                x => x.Split(": ")[1] == "1"
            );

            var gates = input[1].Split(NEW_LINE).Select(x => new Gate
            {
                Input0 = x.Split(" ")[0],
                Operation = GetOperationFromString(x.Split(" ")[1]),
                Input1 = x.Split(" ")[2],
                Output = x.Split(" ")[4],
            });

            foreach (var gate in gates)
            {
                CalculateGateResult(gate, wireStates, gates);
            }

            var zWireValues = wireStates
                .Where(x => x.Key.StartsWith('z'))
                .OrderBy(x => x.Key);

            var binaryString = zWireValues.Aggregate("", (current, next) => (next.Value ? "1" : "0") + current);
            var intResult = Convert.ToInt64(binaryString, 2);

            return intResult.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();

            return "Part 2";
        }

        private static bool CalculateGateResult(Gate gate, Dictionary<string, bool> wireStates, IEnumerable<Gate> gates)
        {
            if (wireStates.TryGetValue(gate.Output, out bool outputFromStates))
                return outputFromStates;

            if (!wireStates.TryGetValue(gate.Input0, out bool input0Value))
                input0Value = CalculateGateResult(gates.First(x => x.Output == gate.Input0), wireStates, gates);

            if (!wireStates.TryGetValue(gate.Input1, out bool input1Value))
                input1Value = CalculateGateResult(gates.First(x => x.Output == gate.Input1), wireStates, gates);

            var result = gate.Operation switch
            {
                Operation.And => input0Value && input1Value,
                Operation.Or => input0Value || input1Value,
                Operation.Xor => input0Value ^ input1Value,

                _ => throw new Exception("Operation not supported."),
            };

            wireStates.Add(gate.Output, result);

            return result;
        }

        private class Gate
        {
            public string Input0 { get; set; }
            public Operation Operation { get; set; }
            public string Input1 { get; set; }
            public string Output { get; set; }
        }

        private enum Operation
        {
            And,
            Or,
            Xor,
        }

        private Operation GetOperationFromString(string op) => op switch
        {
            "AND" => Operation.And,
            "OR" => Operation.Or,
            "XOR" => Operation.Xor,

            _ => throw new Exception("Operation string not supported."),
        };

        private string[] GetInput()
        {
            return FileInputAssistant.GetStringArrayFromFile(TextInputFilePath, $"{NEW_LINE}{NEW_LINE}");
        }
    }
}
