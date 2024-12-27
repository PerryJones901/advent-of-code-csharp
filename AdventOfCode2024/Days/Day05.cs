using AdventOfCodeCommon;

namespace AdventOfCode2024.Days
{
    internal class Day05(bool isTest) : DayBase(5, isTest)
    {
        public override string Part1()
        {
            var input = GetInput();
            var pageOrderRules = input[0].Split("\r\n").Select(x => x.Split("|").Select(int.Parse).ToArray());
            var updates = input[1].Split("\r\n").Select(x => x.Split(",").Select(int.Parse).ToArray());

            var pageOrderRulesDict = new Dictionary<int, List<int>>();
            foreach (var rule in pageOrderRules)
            {
                var firstPage = rule[0];
                var secondPage = rule[1];

                if (!pageOrderRulesDict.ContainsKey(firstPage))
                    pageOrderRulesDict[firstPage] = [];

                pageOrderRulesDict[firstPage].Add(secondPage);
            }

            int middleValidCount = 0;
            bool updateIsValid;
            foreach (var update in updates)
            {
                updateIsValid = true;

                for (int i = 0; i < update.Length - 1; i++)
                {
                    var currentPage = update[i];
                    
                    for (int j = i + 1; j < update.Length; j++)
                    {
                        var nextPage = update[j];

                        if (!pageOrderRulesDict.TryGetValue(currentPage, out List<int>? value) || !value.Contains(nextPage))
                        {
                            updateIsValid = false;
                            break;
                        }
                    }

                    if (!updateIsValid)
                        break;
                }

                if (updateIsValid)
                {
                    var middlePage = update[update.Length / 2];
                    middleValidCount += middlePage;
                }
            }

            return middleValidCount.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();
            var pageOrderRules = input[0].Split("\r\n").Select(x => x.Split("|").Select(int.Parse).ToArray()).ToArray();
            var updates = input[1].Split("\r\n").Select(x => x.Split(",").Select(int.Parse).ToArray()).ToArray();

            var pageOrderRulesDict = new Dictionary<int, List<int>>();

            foreach (var pageOrderRule in pageOrderRules)
            {
                var firstPage = pageOrderRule[0];
                var secondPage = pageOrderRule[1];

                if (!pageOrderRulesDict.ContainsKey(firstPage))
                    pageOrderRulesDict[firstPage] = [];

                pageOrderRulesDict[firstPage].Add(secondPage);
            }

            var middleSum = 0;

            foreach (var update in updates)
            {
                // Step 1: Reorder update to be in correct ordering
                var correctOrderOfUpdate = update.ToList();

                var hasSwapHappened = true;
                while (hasSwapHappened)
                {
                    hasSwapHappened = false;
                    for (var i = 0; i < correctOrderOfUpdate.Count - 1; i++)
                    {
                        var firstPage = correctOrderOfUpdate[i];
                        var secondPage = correctOrderOfUpdate[i + 1];

                        if (pageOrderRulesDict[secondPage].Contains(firstPage))
                        {
                            hasSwapHappened = true;
                            correctOrderOfUpdate[i + 1] = firstPage;
                            correctOrderOfUpdate[i] = secondPage;
                        }
                    }
                }

                // Step 2: Compare to see if list has changed
                var changedFromOriginal = false;
                for (var i = 0; i < correctOrderOfUpdate.Count - 1; i++)
                {
                    if (update[i] != correctOrderOfUpdate[i])
                    {
                        changedFromOriginal = true;
                        break;
                    }
                }

                if (changedFromOriginal)
                    middleSum += correctOrderOfUpdate[correctOrderOfUpdate.Count / 2];
            }

            return middleSum.ToString();
        }

        private string[] GetInput()
        {
            return FileInputAssistant.GetStringArrayFromFile(TextInputFilePath, "\r\n\r\n");
        }
    }
}
