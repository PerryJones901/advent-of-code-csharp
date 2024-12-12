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
            // 11520 WRONG
            // 9849 WRONG
            // 5043 WRONG
            var input = GetInput();
            var pageOrderRules = input[0].Split("\r\n").Select(x => x.Split("|").Select(int.Parse).ToArray());
            var updates = input[1].Split("\r\n").Select(x => x.Split(",").Select(int.Parse).ToArray());

            // Part 2
            var pages = updates.SelectMany(x => x).Distinct().ToArray();
            var pagesInOrder = new List<int>();

            var pageOrderRulesDict = new Dictionary<int, List<int>>();

            // Part 1 stuff
            foreach (var rule in pageOrderRules)
            {
                var firstPage = rule[0];
                var secondPage = rule[1];

                if (firstPage == 45 && secondPage == 22)
                {
                    Console.WriteLine("Hello");
                }

                if (!pageOrderRulesDict.ContainsKey(firstPage))
                    pageOrderRulesDict[firstPage] = [];

                pageOrderRulesDict[firstPage].Add(secondPage);
            }

            pagesInOrder = pageOrderRulesDict.Keys.OrderByDescending(x => pageOrderRulesDict[x].Count).ToList();

            // Part 2 stuff
            //var changeHasBeenMade = true;
            //while (changeHasBeenMade)
            //{
            //    changeHasBeenMade = false;

            //    foreach (var rule in pageOrderRules)
            //    {
            //        var firstPage = rule[0];
            //        var secondPage = rule[1];

            //        var firstPageIndexInList = pagesInOrder.IndexOf(firstPage);
            //        var secondPageIndexInList = pagesInOrder.IndexOf(secondPage);
            //        if (firstPageIndexInList == -1 && secondPageIndexInList == -1)
            //        {
            //            pagesInOrder.Add(firstPage);
            //            pagesInOrder.Add(secondPage);
            //        }
            //        else if (firstPageIndexInList == -1)
            //        {
            //            pagesInOrder.Insert(secondPageIndexInList, firstPage);
            //        }
            //        else if (secondPageIndexInList == -1)
            //        {
            //            pagesInOrder.Insert(firstPageIndexInList + 1, secondPage);
            //        }
            //        else if (firstPageIndexInList > secondPageIndexInList)
            //        {
            //            pagesInOrder.Remove(firstPage);
            //            pagesInOrder.Insert(secondPageIndexInList, firstPage);
            //        } else
            //        {
            //            continue;
            //        }

            //        changeHasBeenMade = true;
            //    }
            //}

            var pageToOrderValue = pagesInOrder.Select((x, i) => (x, i)).ToDictionary(x => x.x, x => x.i);

            // check
            foreach (var rule in pageOrderRules)
            {
                var firstPage = rule[0];
                var secondPage = rule[1];
                if (pageToOrderValue[firstPage] > pageToOrderValue[secondPage])
                {
                    throw new Exception("Invalid order");
                }
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
                    continue;

                var updateInOrder = update.OrderBy(x => pageToOrderValue[x]).ToArray();

                var middlePage = updateInOrder[update.Length / 2];
                middleValidCount += middlePage;
            }

            return middleValidCount.ToString();
        }

        private string[] GetInput()
        {
            return FileInputAssistant.GetStringArrayFromFile(TextInputFilePath, "\r\n\r\n");
        }
    }
}
