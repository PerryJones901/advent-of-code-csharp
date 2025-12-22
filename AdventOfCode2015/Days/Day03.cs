using AdventOfCodeCommon;

namespace AdventOfCode2015.Days
{
    internal class Day03(bool isTest) : DayBase(3, isTest)
    {
        public override string Part1()
        {
            var input = GetInput();
            var charToDiffMap = GetCharToDiffMap();

            var currentCoords = (0, 0);
            var visitedCoords = new HashSet<(int, int)> { currentCoords };

            foreach (var move in input)
            {
                var (rowDiff, colDiff) = charToDiffMap[move];
                currentCoords = (currentCoords.Item1 + rowDiff, currentCoords.Item2 + colDiff);
                visitedCoords.Add(currentCoords);
            }

            return visitedCoords.Count.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();
            var charToDiffMap = GetCharToDiffMap();

            var santaCoords = (0, 0);
            var roboSantaCoords = (0, 0);

            var visitedCoords = new HashSet<(int, int)> { (0, 0) };

            for (int i = 0; i < input.Length; i++)
            {
                var move = input[i];
                var (rowDiff, colDiff) = charToDiffMap[move];

                if (i % 2 == 0)
                {
                    santaCoords = (santaCoords.Item1 + rowDiff, santaCoords.Item2 + colDiff);
                    visitedCoords.Add(santaCoords);
                }
                else
                {
                    roboSantaCoords = (roboSantaCoords.Item1 + rowDiff, roboSantaCoords.Item2 + colDiff);
                    visitedCoords.Add(roboSantaCoords);
                }
            }

            return visitedCoords.Count.ToString();
        }

        private IDictionary<char, (int, int)> GetCharToDiffMap()
            => new Dictionary<char, (int, int)>
            {
                { '^', (-1, 0) },
                { 'v', (1, 0) },
                { '<', (0, -1) },
                { '>', (0, 1) },
            };

        private string GetInput()
            => FileInputAssistant.GetStringFromFile(TextInputFilePath);
    }
}
