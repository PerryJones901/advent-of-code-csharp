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

                var successExists = DoesSuccessExist(testList, testValue, isConcatIncluded: false);

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

                // First, try without concat operation (saves a bit of time)
                var successExistsWithoutConcat = DoesSuccessExist(testList, testValue, isConcatIncluded: false);
                if (successExistsWithoutConcat)
                {
                    totalCalibrationResult += testValue;
                    continue;
                }

                // Now, include concat operation
                // TODO: filter out permutations without concat op, as we've already tried them above
                var successExists = DoesSuccessExist(testList, testValue, isConcatIncluded: true);
                if (successExists)
                {
                    totalCalibrationResult += testValue;
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

        private static bool DoesSuccessExist(List<long> testList, long testValue, bool isConcatIncluded = false)
        {
            foreach (var permutation in GetOperationPermutations(testList.Count - 1, isConcatIncluded))
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
                    return true;
            }

            return false;
        }

        private static IEnumerable<Operation[]> GetOperationPermutations(int length, bool isConcatIncluded = false)
        {
            var opCount = isConcatIncluded ? 3 : 2;

            for (var i = 0; i < Math.Pow(opCount, length); i++)
            {
                var listOfOps = new List<Operation>();

                for (var j = 0; j < length; j++)
                {
                    var op = (Operation)(i / Math.Pow(opCount, j) % opCount);
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
