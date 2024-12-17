using AdventOfCodeCommon;

namespace AdventOfCode2024.Days
{
    internal class Day17(bool isTest) : DayBase(17, isTest)
    {
        public override string Part1()
        {
            var input = GetInput();
            var regA = long.Parse(input[0].Split("Register A: ")[1]);
            var regB = long.Parse(input[1].Split("Register B: ")[1]);
            var regC = long.Parse(input[2].Split("Register C: ")[1]);
            var numbers = input[4].Split("Program: ")[1].Split(",").Select(int.Parse).ToArray();

            var outputValues = GetOutput(regA, regB, regC, numbers);
            var outputString = string.Join(",", outputValues);

            return outputString;
        }

        public override string Part2()
        {
            // TOO LOW: 64559155
            // TOO LOW: 64559548
            // (not offical but) TOO LOW: 2015500000

            var input = GetInput();
            var regAFromFile = long.Parse(input[0].Split("Register A: ")[1]);
            var regBFromFile = long.Parse(input[1].Split("Register B: ")[1]);
            var regCFromFile = long.Parse(input[2].Split("Register C: ")[1]);
            var numbers = input[4].Split("Program: ")[1].Split(",").Select(int.Parse).ToArray();

            // regA reset
            var initialRegA = -1;
            
            while (true)
            {
                initialRegA++;
                if (initialRegA % 100_000 == 0)
                    Console.WriteLine(initialRegA);

                // Ignore the one from file
                if (initialRegA == regAFromFile)
                    continue;

                var regA = initialRegA;
                var regB = regBFromFile;
                var regC = regCFromFile;
                var outputLooksGoodForNow = true;
                var currentOutputValuesIndex = 0;
                var instructionPointer = 0;
                var deleteMeList = new List<long>();

                while (outputLooksGoodForNow)
                {
                    if (instructionPointer >= numbers.Length)
                    {
                        // We're outside
                        break;
                    }

                    var opcode = numbers[instructionPointer];
                    var operand = numbers[instructionPointer + 1];
                    var shouldUpInsPointer = true;

                    switch (opcode)
                    {
                        case 0: // Division
                            regA /= (int)Math.Pow(2, GetComboOperand(operand, regA, regB, regC)); break;
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
                            // check if index is out of bounds
                            if (currentOutputValuesIndex >= numbers.Length)
                            {
                                outputLooksGoodForNow = false;
                                break;
                            }

                            var output = GetComboOperand(operand, regA, regB, regC) % 8;
                            // check if not same in original list
                            if (output != numbers[currentOutputValuesIndex])
                            {
                                outputLooksGoodForNow = false;
                                break;
                            }

                            // otherwise, all good, increase index
                            currentOutputValuesIndex++;
                            break;
                        case 6:
                            regB = regA / (int)Math.Pow(2, GetComboOperand(operand, regA, regB, regC)); break;
                        case 7:
                            regC = regA / (int)Math.Pow(2, GetComboOperand(operand, regA, regB, regC)); break;
                    }

                    if (shouldUpInsPointer)
                        instructionPointer += 2;
                }

                if (outputLooksGoodForNow && currentOutputValuesIndex == numbers.Length)
                {
                    // We are done!
                    Console.WriteLine(string.Join(",", deleteMeList));
                    return initialRegA.ToString();
                }
            }


            return "";
        }

        public static List<long> GetOutput(long regA, long regB, long regC, int[] numbers)
        {
            var instructionPointer = 0;
            var outputValues = new List<long>();

            while (true)
            {
                if (instructionPointer >= numbers.Length)
                {
                    // We're outside
                    break;
                }

                var opcode = numbers[instructionPointer];
                var operand = numbers[instructionPointer + 1];
                var shouldUpInsPointer = true;

                switch (opcode)
                {
                    case 0: // Division
                        regA /= (int)Math.Pow(2, GetComboOperand(operand, regA, regB, regC)); break;
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
                        outputValues.Add(output);
                        break;
                    case 6:
                        regB = regA / (int)Math.Pow(2, GetComboOperand(operand, regA, regB, regC)); break;
                    case 7:
                        regC = regA / (int)Math.Pow(2, GetComboOperand(operand, regA, regB, regC)); break;
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

        private string[] GetInput()
        {
            return FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
        }
    }
}
