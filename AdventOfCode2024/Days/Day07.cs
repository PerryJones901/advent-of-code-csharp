using AdventOfCodeCommon;

namespace AdventOfCode2024.Days
{
    internal class Day07(bool isTest) : DayBase(7, isTest)
    {
        public override string Part1()
        {
            var input = GetInput();

            var totalCalibrationResult = 0L;

            foreach (var inputItem in input)
            {
                var testValue = inputItem.TestValue;
                var testList = inputItem.TestList;

                var successExists = SuccessExistsWithAddAndMultiply(testList, testValue);

                if (successExists)
                    totalCalibrationResult += testValue;
            }

            return totalCalibrationResult.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();

            var totalCalibrationResult = 0L;

            foreach (var inputItem in input)
            {
                var testValue = inputItem.TestValue;
                var testList = inputItem.TestList;

                // Focus on success without concat first
                var successExistsWithoutConcat = SuccessExistsWithAddAndMultiply(testList, testValue);

                if (successExistsWithoutConcat)
                {
                    totalCalibrationResult += testValue;
                    continue;
                }

                // Now, try with concat in the mix.
                // TODO: filter out permutations without concat op
                foreach (var permutation in GetOperationPermutations(testList.Count - 1))
                {
                    var currentValue = testList[0];
                    for (var i = 1; i < testList.Count; i++)
                    {
                        var op = permutation[i - 1];
                        var nextValue = testList[i];

                        switch (op)
                        {
                            case Operation.Add:
                                currentValue += nextValue;
                                break;
                            case Operation.Multiply:
                                currentValue *= nextValue;
                                break;
                            case Operation.Concatenate:
                                currentValue = long.Parse(currentValue.ToString() + nextValue.ToString());
                                break;
                        }
                    }

                    if (currentValue == testValue)
                    {
                        totalCalibrationResult += testValue;
                        break;
                    }
                }
            }

            return totalCalibrationResult.ToString();
        }

        private List<EquationInput> GetInput()
        {
            var input = FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
            var parsedInput = new List<EquationInput>();
            foreach (var inputItem in input)
            {
                var splitInput = inputItem.Split(": ");
                var testValue = long.Parse(splitInput[0]);
                var testList = splitInput[1].Split(" ").Select(long.Parse).ToList();

                var equationInput = new EquationInput
                {
                    TestValue = testValue,
                    TestList = testList
                };

                parsedInput.Add(equationInput);
            }

            return parsedInput;
        }

        private static bool SuccessExistsWithAddAndMultiply(List<long> testList, long testValue)
        {
            foreach (var permutation in GetBoolPermutations(testList.Count - 1))
            {
                var currentValue = testList[0];
                for (var i = 1; i < testList.Count; i++)
                {
                    if (permutation[i - 1])
                    {
                        currentValue *= testList[i];
                    }
                    else
                    {
                        currentValue += testList[i];
                    }
                }

                if (currentValue == testValue)
                {
                    return true;
                }
            }
            return false;
        }

        private static IEnumerable<bool[]> GetBoolPermutations(int length)
        {
            for (var i = 0; i < Math.Pow(2, length); i++)
            {
                var binaryString = Convert.ToString(i, 2).PadLeft(length, '0');
                yield return binaryString.Select(c => c == '1').ToArray();
            }
        }

        private static IEnumerable<Operation[]> GetOperationPermutations(int length)
        {
            for (var i = 0; i < Math.Pow(3, length); i++)
            {
                var listOfOps = new List<Operation>();

                for (var j = 0; j < length; j++)
                {
                    var op = (Operation)(i / Math.Pow(3, j) % 3);
                    listOfOps.Add(op);
                }

                yield return listOfOps.ToArray();
            }
        }

        private class EquationInput
        {
            public long TestValue { get; set; }
            public required List<long> TestList { get; set; }
        }

        private enum Operation
        {
            Add = 0,
            Multiply = 1,
            Concatenate = 2,
        }
    }
}
