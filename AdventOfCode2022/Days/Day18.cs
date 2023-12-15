using AdventOfCode2022.Helpers;

namespace AdventOfCode2022.Days
{
    public static class Day18
    {
        public static int Part1Answer() =>
            Part1(GetSeparatedInputFromFile());

        public static int Part2Answer() =>
            Part2(GetSeparatedInputFromFile());

        public static int Part1(List<string> input)
        {
            var cubeCoordsList = input
                .Select(GetCoordsFromString)
                .OrderBy(x => x[0])
                .ThenBy(x => x[1])
                .ThenBy(x => x[2]);

            var facesVisible = 0;

            foreach (var cubeCoords in cubeCoordsList)
            {
                var cubeNeighboursInX = cubeCoordsList.Where(x =>
                    Math.Abs(x[0] - cubeCoords[0]) == 1
                    && x[1] == cubeCoords[1]
                    && x[2] == cubeCoords[2]
                ).ToList();
                var cubeNeighboursInY = cubeCoordsList.Where(x =>
                    x[0] == cubeCoords[0]
                    && Math.Abs(x[1] - cubeCoords[1]) == 1
                    && x[2] == cubeCoords[2]
                ).ToList();
                var cubeNeighboursInZ = cubeCoordsList.Where(x => 
                    x[0] == cubeCoords[0]
                    && x[1] == cubeCoords[1]
                    && Math.Abs(x[2] - cubeCoords[2]) == 1
                ).ToList();

                var numGluedCubes = cubeNeighboursInX.Count + cubeNeighboursInY.Count + cubeNeighboursInZ.Count;
                facesVisible += 6 - numGluedCubes;
            }

            return facesVisible;
        }

        public static int Part2(List<string> input)
        {
            return 0;
        }

        private static int[] GetCoordsFromString(string line)
        {
            return line.Split(',').Select(int.Parse).ToArray();
        }

        private static List<string> GetSeparatedInputFromFile() => 
            FileInputHelper.GetStringListFromFile("Day18.txt");
    }
}
