using AdventOfCodeCommon;

namespace AdventOfCode2024.Days
{
    internal class Day17(bool isTest) : DayBase(17, isTest)
    {
        public override string Part1()
        {
            var input = ParseInput(GetInputFromFile());

            var outputValues = GetOutput(
                input.RegA,
                input.RegB,
                input.RegC,
                input.Program);

            var outputString = string.Join(",", outputValues);

            return outputString;
        }

        public override string Part2()
        {
            var input = ParseInput(GetInputFromFile());
            var program = input.Program;

            var regA = 0L;
            for (int i = 1; i <= program.Length; i++)
            {
                regA <<= 3;

                var goalNumbers = program.TakeLast(i).ToArray();
                while (!GetOutput(regA, input.RegB, input.RegC, program).SequenceEqual(goalNumbers))
                    regA++;
            }

            return regA.ToString();
        }

        public static List<int> GetOutput(long regA, long regB, long regC, int[] program)
        {
            var instructionPointer = 0;
            var outputValues = new List<int>();

            while (true)
            {
                if (instructionPointer >= program.Length)
                    break;

                var opcode = program[instructionPointer];
                var operand = program[instructionPointer + 1];
                var shouldUpInsPointer = true;

                switch (opcode)
                {
                    case 0:
                        regA >>= (int)GetComboOperand(operand, regA, regB, regC); break;
                    case 1:
                        regB ^= operand; break;
                    case 2:
                        regB = GetComboOperand(operand, regA, regB, regC) % 8; break;
                    case 3:
                        if (regA == 0) break;
                        instructionPointer = operand;
                        shouldUpInsPointer = false;
                        break;
                    case 4:
                        regB ^= regC; break;
                    case 5:
                        var output = GetComboOperand(operand, regA, regB, regC) % 8;
                        outputValues.Add((int)output);
                        break;
                    case 6:
                        regB = regA >> (int)GetComboOperand(operand, regA, regB, regC); break;
                    case 7:
                        regC = regA >> (int)GetComboOperand(operand, regA, regB, regC); break;
                }

                if (shouldUpInsPointer)
                    instructionPointer += 2;
            }

            return outputValues;
        }

        private static long GetComboOperand(int opcode, long regA, long regB, long regC)
        {
            return opcode switch
            {
                0 => 0,
                1 => 1,
                2 => 2,
                3 => 3,
                4 => regA,
                5 => regB,
                6 => regC,
                _ => throw new Exception("Invalid opcode")
            };
        }

        private class InputData
        {
            public long RegA { get; set; }
            public long RegB { get; set; }
            public long RegC { get; set; }
            public int[] Program { get; set; } = [];
        }

        private static InputData ParseInput(string[] input)
        {
            var programNumbers = input[4]
                .Split("Program: ")[1]
                .Split(",")
                .Select(int.Parse)
                .ToArray();

            return new InputData
            {
                RegA = long.Parse(input[0].Split("Register A: ")[1]),
                RegB = long.Parse(input[1].Split("Register B: ")[1]),
                RegC = long.Parse(input[2].Split("Register C: ")[1]),
                Program = programNumbers
            };
        }

        private string[] GetInputFromFile()
            => FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
    }
}
