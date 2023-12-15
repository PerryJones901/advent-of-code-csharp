using AdventOfCode2022.Helpers;

namespace AdventOfCode2022.Days
{
    public static class Day9
    {
        public static int Part1Answer() =>
            Part1(GetSeparatedInputFromFile());

        public static int Part2Answer() =>
            Part2(GetSeparatedInputFromFile());

        public static int Part1(List<string> input) =>
            UniquePositionsOfTailCount(input, 2);

        public static int Part2(List<string> input) =>
            UniquePositionsOfTailCount(input, 10);

        public static int UniquePositionsOfTailCount(List<string> input, int numOfKnots)
        {
            var moves = GetMoves(input);

            var knotCoords =
                Enumerable.Range(0, numOfKnots)
                    .Select(x => (0, 0))
                    .ToList();

            var setOfTailCoords = new HashSet<(int, int)>() { (0, 0) };

            int i;
            foreach (var move in moves)
            {
                for (i = 0; i < move.Amount; i++)
                {
                    knotCoords[0] = (
                        knotCoords[0].Item1 + move.Vector.Item1,
                        knotCoords[0].Item2 + move.Vector.Item2
                    );

                    for (int j = 1; j < numOfKnots; j++)
                    {
                        knotCoords[j] = GetNewTailCoords(knotCoords[j], knotCoords[j-1]);
                    }
                    
                    setOfTailCoords.Add(knotCoords.Last());
                }
            }

            return setOfTailCoords.Count;
        }

        public static (int, int) GetNewTailCoords(
            (int, int) tailCoords,
            (int, int) headCoords)
        {
            var signDeltaRow = Math.Sign(headCoords.Item1 - tailCoords.Item1);
            var signDeltaCol = Math.Sign(headCoords.Item2 - tailCoords.Item2);

            if (tailCoords.Item1 < headCoords.Item1 - 1)
            {
                return (headCoords.Item1 - 1, tailCoords.Item2 + signDeltaCol);
            }
            if (tailCoords.Item1 > headCoords.Item1 + 1)
            {
                return (headCoords.Item1 + 1, tailCoords.Item2 + signDeltaCol);
            }
            if (tailCoords.Item2 < headCoords.Item2 - 1)
            {
                return (tailCoords.Item1 + signDeltaRow, headCoords.Item2 - 1);
            }
            if (tailCoords.Item2 > headCoords.Item2 + 1)
            {
                return (tailCoords.Item1 + signDeltaRow, headCoords.Item2 + 1);
            }

            return (tailCoords.Item1, tailCoords.Item2);
        }

        private static List<string> GetSeparatedInputFromFile() => 
            FileInputHelper.GetStringListFromFile("Day09.txt");

        private static List<Move> GetMoves(List<string> stringInput) =>
            stringInput
            .Select(x => new Move { 
                Vector = GetVector(x.Split(' ')[0]),
                Amount = int.Parse(x.Split(' ')[1]),
            })
            .ToList();
        
        private static (int, int) GetVector(string input)
        {
            return input switch
            {
                "U" => (-1,  0),
                "R" => ( 0,  1),
                "D" => ( 1,  0),
                "L" => ( 0, -1),
                _ => throw new Exception("Unexpected direction string"),
            };
        }

        private static void PrintGrid(List<(int, int)> knotCoords)
        {
            var grid = Enumerable.Range(0, 11).Select(x => Enumerable.Repeat(".", 11).ToList()).ToList();
            for (int i = 0; i < knotCoords.Count; i++)
            {
                if (knotCoords[i].Item1 + 5 > 10 || knotCoords[i].Item1 + 5 < 0) continue;
                if (knotCoords[i].Item2 + 5 > 10 || knotCoords[i].Item2 + 5 < 0) continue;
                if (grid[knotCoords[i].Item1 + 5][knotCoords[i].Item2 + 5] == ".")
                    grid[knotCoords[i].Item1 + 5][knotCoords[i].Item2 + 5] = i.ToString();
            }
            Console.WriteLine("-----");
            var stringGrid = string.Join("\n", grid.Select(x => string.Join("", x)));
            Console.WriteLine(stringGrid);
        }

        public class Move
        {
            public (int, int) Vector { get; set; }
            public int Amount { get; set; }
        }
    }
}
