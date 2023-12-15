using AdventOfCode2022.Helpers;

namespace AdventOfCode2022.Days
{
    public static class Day8
    {
        public static int Part1Answer() =>
            Part1(GetSeparatedInputFromFile());

        public static int Part2Answer() =>
            Part2(GetSeparatedInputFromFile());

        public static int Part1(List<string> input)
        {
            var heightOfGrid = input.Count;
            var widthOfGrid = input.First().Length;
            var visibilityGrid = new bool[heightOfGrid, widthOfGrid];

            // Left to right:
            for(int i = 0; i < heightOfGrid; i++)
            {
                var tallestTreeNum = -1;
                for (int j = 0; j < widthOfGrid; j++)
                {
                    var treeNum = int.Parse(input[i][j].ToString());
                    if (treeNum > tallestTreeNum)
                    {
                        visibilityGrid[i,j] = true;
                        tallestTreeNum = treeNum;
                    }
                }
            }

            // Right to left:
            for (int i = 0; i < heightOfGrid; i++)
            {
                var tallestTreeNum = -1;
                for (int j = widthOfGrid - 1; j >= 0; j--)
                {
                    var treeNum = int.Parse(input[i][j].ToString());
                    if (treeNum > tallestTreeNum)
                    {
                        visibilityGrid[i, j] = true;
                        tallestTreeNum = treeNum;
                    }
                }
            }

            // Top to bottom:
            for (int j = 0; j < widthOfGrid; j++)
            {
                var tallestTreeNum = -1;
                for (int i = 0; i < heightOfGrid; i++)
                {
                    var treeNum = int.Parse(input[i][j].ToString());
                    if (treeNum > tallestTreeNum)
                    {
                        visibilityGrid[i, j] = true;
                        tallestTreeNum = treeNum;
                    }
                }
            }

            // Bottom to top:
            for (int j = 0; j < widthOfGrid; j++)
            {
                var tallestTreeNum = -1;
                for (int i = heightOfGrid - 1; i >= 0; i--)
                {
                    var treeNum = int.Parse(input[i][j].ToString());
                    if (treeNum > tallestTreeNum)
                    {
                        visibilityGrid[i, j] = true;
                        tallestTreeNum = treeNum;
                    }
                }
            }

            var count = 0;
            foreach(var cell in visibilityGrid)
            {
                if(cell) count++;
            }
            return count;
        }

        public static int Part2(List<string> input)
        {

            var heightOfGrid = input.Count;
            var widthOfGrid = input.First().Length;
            var scoreGrid = 
                Enumerable
                    .Range(0, heightOfGrid)
                    .Select(x => Enumerable.Repeat(1, widthOfGrid).ToArray())
                    .ToArray();

            
            // Left to right:
            for (int i = 0; i < heightOfGrid; i++)
            {
                var dict = TreeHeightToTreesSeen();
                for (int j = 0; j < widthOfGrid; j++)
                {
                    var treeNum = int.Parse(input[i][j].ToString());
                    scoreGrid[i][j] *= dict[treeNum];

                    foreach (int smallOrEqTreeNum in Enumerable.Range(0, 10))
                    {
                        if(smallOrEqTreeNum <= treeNum)
                            dict[smallOrEqTreeNum] = 1;
                        else
                            dict[smallOrEqTreeNum] += 1;
                    }
                }
            }

            // Right to left:
            for (int i = 0; i < heightOfGrid; i++)
            {
                var dict = TreeHeightToTreesSeen();
                for (int j = widthOfGrid - 1; j >= 0; j--)
                {
                    var treeNum = int.Parse(input[i][j].ToString());
                    scoreGrid[i][j] *= dict[treeNum];

                    foreach (int smallOrEqTreeNum in Enumerable.Range(0, 10))
                    {
                        if (smallOrEqTreeNum <= treeNum)
                            dict[smallOrEqTreeNum] = 1;
                        else
                            dict[smallOrEqTreeNum] += 1;
                    }
                }
            }

            // Top to bottom:
            for (int j = 0; j < widthOfGrid; j++)
            {
                var dict = TreeHeightToTreesSeen();
                for (int i = 0; i < heightOfGrid; i++)
                {
                    var treeNum = int.Parse(input[i][j].ToString());
                    scoreGrid[i][j] *= dict[treeNum];

                    foreach (int smallOrEqTreeNum in Enumerable.Range(0, 10))
                    {
                        if (smallOrEqTreeNum <= treeNum)
                            dict[smallOrEqTreeNum] = 1;
                        else
                            dict[smallOrEqTreeNum] += 1;
                    }
                }
            }

            // Bottom to top:
            for (int j = 0; j < widthOfGrid; j++)
            {
                var dict = TreeHeightToTreesSeen();
                for (int i = heightOfGrid - 1; i >= 0; i--)
                {
                    var treeNum = int.Parse(input[i][j].ToString());
                    scoreGrid[i][j] *= dict[treeNum];

                    foreach (int smallOrEqTreeNum in Enumerable.Range(0, 10))
                    {
                        if (smallOrEqTreeNum <= treeNum)
                            dict[smallOrEqTreeNum] = 1;
                        else
                            dict[smallOrEqTreeNum] += 1;
                    }
                }
            }

            return scoreGrid.Max(x => x.Max());
        }

        public static Dictionary<int, int> TreeHeightToTreesSeen() =>
            Enumerable
                .Range(0, 10)
                .ToDictionary(x => x, x => 0);

        public static List<string> GetSeparatedInputFromFile() =>
            FileInputHelper.GetStringListFromFile("Day08.txt");
    }
}
