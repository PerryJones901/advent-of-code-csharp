using AdventOfCode2023.Helpers;

namespace AdventOfCode2023.Days;

public abstract class Day10
{
    public static int Part1Answer() =>
        Part1(GetSeparatedInputFromFile());

    public static int Part2Answer() =>
        Part2(GetSeparatedInputFromFile());

    public static int Part1(List<string> input)
    {
        int currentRow = 0;
        int currentCol = 0;
        for(int i = 0; i < input.Count; i++)
        {
            var indexOfS = input[i].IndexOf('S');
            if (indexOfS >= 0)
            {
                currentCol = indexOfS;
                currentRow = i;
                break;
            }
        }

        var coordsOfS = new List<int> { currentRow, currentCol };

        // Traverse pipes, with special case at S - we must choose which direction to go in

        var numSteps = 0;
        int currentDirectionIndex = 0;
        char currentChar = 'S';

        for (int i = 0; i < 4; i++)
        {
            int newRow = currentRow + CoordsDiffs[i][0];
            int newCol = currentCol + CoordsDiffs[i][1];

            if (IsInBounds(newRow, newCol, input.Count, input[0].Length))
            {
                var pipeChar = input[newRow][newCol];
                if (AcceptedPipesForInput[i].Contains(pipeChar))
                {
                    // Traverse pipe
                    currentRow = newRow;
                    currentCol = newCol;
                    currentDirectionIndex = i;
                    currentChar = pipeChar;
                    numSteps++;
                    break;
                }
            }

            if (i == 3) throw new Exception("No pipe is traverseable from S");
        }

        while (true)
        {
            numSteps++;
            // Based on currentChar and currentDirectionIndex, we can work out where the next location will be
            // We'll then check if the pipe is allowed, if not, throw error
            var binString = PipeCharToAllowedOutputCompassMoves[currentChar];

            // Use the following to not go back to the previous pipe
            var oppositeToOutputDir = ((currentDirectionIndex + 2) % 4);
            currentDirectionIndex = binString
                .Select((x, ind) => x == '1' ? int.Parse(ind.ToString()) : -1)
                .Where(x => x != -1 && x != oppositeToOutputDir)
                .Single();

            currentRow += CoordsDiffs[currentDirectionIndex][0];
            currentCol += CoordsDiffs[currentDirectionIndex][1];

            currentChar = input[currentRow][currentCol];
            if (currentChar == 'S')
                break;
            if (!AcceptedPipesForInput[currentDirectionIndex].Contains(currentChar))
                throw new Exception("Pipe does not allow entry from this direction");
        };

        var halfNumSteps = numSteps / 2;

        return halfNumSteps;
    }

    public static int Part2(List<string> input)
    {
        var paintGrid = GetPaintGrid(input.Count, input[0].Length);

        int currentRow = 0;
        int currentCol = 0;
        for (int i = 0; i < input.Count; i++)
        {
            var indexOfS = input[i].IndexOf('S');
            if (indexOfS >= 0)
            {
                currentCol = indexOfS;
                currentRow = i;
                break;
            }
        }

        var coordsOfS = new List<int> { currentRow, currentCol };

        // Paint first Pipe
        var paintGridRowArray = paintGrid[currentRow].ToCharArray();
        // paintGridRowArray[currentCol] = 'P';
        // Cheating as we know the input:
        paintGridRowArray[currentCol] = '↑';
        paintGrid[currentRow] = new string(paintGridRowArray);

        // Traverse pipes, with special case at S - we must choose which direction to go in

        var numSteps = 0;
        int currentDirectionIndex = 0;
        char currentChar = 'S';

        for (int i = 0; i < 4; i++)
        {
            int newRow = currentRow + CoordsDiffs[i][0];
            int newCol = currentCol + CoordsDiffs[i][1];

            if (IsInBounds(newRow, newCol, input.Count, input[0].Length))
            {
                var pipeChar = input[newRow][newCol];
                if (AcceptedPipesForInput[i].Contains(pipeChar))
                {
                    // Traverse pipe
                    currentRow = newRow;
                    currentCol = newCol;
                    currentDirectionIndex = i;
                    currentChar = pipeChar;
                    numSteps++;

                    // Paint pipes
                    // Paint 1: The Pipe P
                    paintGridRowArray = paintGrid[currentRow].ToCharArray();
                    // paintGridRowArray[currentCol] = 'P';
                    paintGridRowArray[currentCol] = ArrowChars[currentDirectionIndex];
                    paintGrid[currentRow] = new string(paintGridRowArray);

                    // Paint 2: The Left
                    var leftDirIndex = (currentDirectionIndex + 3) % 4;
                    int newLeftRow = currentRow + CoordsDiffs[leftDirIndex][0];
                    int newLeftCol = currentCol + CoordsDiffs[leftDirIndex][1];
                    if (IsInBounds(newLeftRow, newLeftCol, input.Count, input[0].Length))
                    {
                        paintGridRowArray = paintGrid[newLeftRow].ToCharArray();
                        if (ArrowChars.Contains(paintGridRowArray[newLeftCol]))
                        {
                            // Do nothing
                        } 
                        else if (paintGridRowArray[newLeftCol] == 'R')
                        {
                            // Replace with '.'
                            paintGridRowArray[newLeftCol] = '.';
                        }
                        else if (paintGridRowArray[newLeftCol] == 'L')
                        {
                            // Do nothing
                        } else if (paintGridRowArray[newLeftCol] == '.')
                        {
                            paintGridRowArray[newLeftCol] = 'L';
                        } else
                        {
                            throw new Exception("Unexpected char");
                        }
                        
                        paintGrid[newLeftRow] = new string(paintGridRowArray);
                    }

                    // Paint 3: The Right
                    var rightDirIndex = (currentDirectionIndex + 1) % 4;
                    int newRightRow = currentRow + CoordsDiffs[rightDirIndex][0];
                    int newRightCol = currentCol + CoordsDiffs[rightDirIndex][1];
                    if (IsInBounds(newRightRow, newRightCol, input.Count, input[0].Length))
                    {
                        paintGridRowArray = paintGrid[newRightRow].ToCharArray();
                        if (ArrowChars.Contains(paintGridRowArray[newRightCol]))
                        {
                            // Do nothing
                        }
                        else if (paintGridRowArray[newRightCol] == 'R')
                        {
                            // Do nothing
                        }
                        else if (paintGridRowArray[newRightCol] == 'L')
                        {
                            // Replace with '.'
                            paintGridRowArray[newRightCol] = '.';
                        }
                        else if (paintGridRowArray[newRightCol] == '.')
                        {
                            paintGridRowArray[newRightCol] = 'R';
                        }
                        else
                        {
                            throw new Exception("Unexpected char");
                        }

                        paintGrid[newRightRow] = new string(paintGridRowArray);
                    }

                    break;
                }
            }

            if (i == 3) throw new Exception("No pipe is traverseable from S");
        }

        while (true)
        {
            numSteps++;
            // Based on currentChar and currentDirectionIndex, we can work out where the next location will be
            // We'll then check if the pipe is allowed, if not, throw error
            var binString = PipeCharToAllowedOutputCompassMoves[currentChar];

            // Use the following to not go back to the previous pipe
            var oppositeToOutputDir = ((currentDirectionIndex + 2) % 4);
            currentDirectionIndex = binString
                .Select((x, ind) => x == '1' ? int.Parse(ind.ToString()) : -1)
                .Where(x => x != -1 && x != oppositeToOutputDir)
                .Single();

            currentRow += CoordsDiffs[currentDirectionIndex][0];
            currentCol += CoordsDiffs[currentDirectionIndex][1];
            currentChar = input[currentRow][currentCol];

            if (currentChar == 'S')
                break;
            if (!AcceptedPipesForInput[currentDirectionIndex].Contains(currentChar))
                throw new Exception("Pipe does not allow entry from this direction");

            // **** PAINT TIME ****
            // Paint pipes
            // Paint 1: The Pipe P
            paintGridRowArray = paintGrid[currentRow].ToCharArray();
            // paintGridRowArray[currentCol] = 'P';
            paintGridRowArray[currentCol] = ArrowChars[currentDirectionIndex];
            paintGrid[currentRow] = new string(paintGridRowArray);

            // Paint 2: The Left
            var leftDirIndex = (currentDirectionIndex + 3) % 4;
            int newLeftRow = currentRow + CoordsDiffs[leftDirIndex][0];
            int newLeftCol = currentCol + CoordsDiffs[leftDirIndex][1];
            if (IsInBounds(newLeftRow, newLeftCol, input.Count, input[0].Length))
            {
                paintGridRowArray = paintGrid[newLeftRow].ToCharArray();
                if (ArrowChars.Contains(paintGridRowArray[newLeftCol]))
                {
                    // Do nothing
                }
                else if (paintGridRowArray[newLeftCol] == 'R')
                {
                    // Replace with '.'
                    paintGridRowArray[newLeftCol] = '.';
                }
                else if (paintGridRowArray[newLeftCol] == 'L')
                {
                    // Do nothing
                }
                else if (paintGridRowArray[newLeftCol] == '.')
                {
                    paintGridRowArray[newLeftCol] = 'L';
                }
                else
                {
                    throw new Exception("Unexpected char");
                }

                paintGrid[newLeftRow] = new string(paintGridRowArray);
            }

            // Paint 3: The Right
            var rightDirIndex = (currentDirectionIndex + 1) % 4;
            int newRightRow = currentRow + CoordsDiffs[rightDirIndex][0];
            int newRightCol = currentCol + CoordsDiffs[rightDirIndex][1];
            if (IsInBounds(newRightRow, newRightCol, input.Count, input[0].Length))
            {
                paintGridRowArray = paintGrid[newRightRow].ToCharArray();
                if (ArrowChars.Contains(paintGridRowArray[newRightCol]))
                {
                    // Do nothing
                }
                else if (paintGridRowArray[newRightCol] == 'R')
                {
                    // Do nothing
                }
                else if (paintGridRowArray[newRightCol] == 'L')
                {
                    // Replace with '.'
                    paintGridRowArray[newRightCol] = '.';
                }
                else if (paintGridRowArray[newRightCol] == '.')
                {
                    paintGridRowArray[newRightCol] = 'R';
                }
                else
                {
                    throw new Exception("Unexpected char");
                }

                paintGrid[newRightRow] = new string(paintGridRowArray);
            }
        };

        var hello = string.Join('\n', paintGrid);

        var fullPaintGrid = GetFullyPaintedGrid(paintGrid);
        var numOfR = fullPaintGrid.SelectMany(x => x.ToCharArray()).Count(x => x == 'R');

        // 507 -- too low
        // 508 -- too low
        // 509 ??
        // 510 -- not correct
        // 511 ??
        // 512 -- not correct
        // 513 ??
        // 514 -- not correct
        // 515 ??
        // 516 -- too high

        return numOfR;
    }

    private static List<string> GetPaintGrid(int numRows, int numCols)
    {
        return Enumerable.Repeat(GetInitialPaintGridRow(numCols), numRows).ToList();
    }

    private static string GetInitialPaintGridRow(int size)
    {
        return string.Join("", Enumerable.Repeat('.', size));
    }

    private static List<string> GetSeparatedInputFromFile() =>
        FileInputHelper.GetStringListFromFile("Day10.txt");

    private static bool IsInBounds(int rowInd, int colInd, int rowCount, int colCount)
    {
        return (0 <= rowInd && rowInd < rowCount) && (0 <= colInd && colInd < colCount);
    }

    private static List<string> GetFullyPaintedGrid(List<string> initialGrid)
    {
        var changeOccurred = false;
        var grid = initialGrid.Select(x => x).ToList();
        char[] hello;

        do
        {
            changeOccurred = false;
            for (int row = 0; row < grid.Count; row++)
            {
                for (int col = 0; col < grid[0].Length; col++)
                {
                    var currentChar = grid[row][col];
                    if (currentChar != '.') continue;

                    // Have a look around it to decide painting L or R
                    for (int dir = 0; dir < 4; dir++)
                    {
                        var newRow = row + CoordsDiffs[dir][0];
                        var newCol = col + CoordsDiffs[dir][1];
                        if (!IsInBounds(newRow, newCol, grid.Count, grid[0].Length)) continue;

                        var neighbourChar = grid[newRow][newCol];
                        if (neighbourChar == 'L')
                        {
                            hello = grid[row].ToCharArray();
                            hello[col] = 'L';
                            grid[row] = new string(hello);
                            changeOccurred = true;
                        }
                        else if (neighbourChar == 'R')
                        {
                            hello = grid[row].ToCharArray();
                            hello[col] = 'R';
                            grid[row] = new string(hello);
                            changeOccurred = true;
                        }
                    }
                }
            }
        } while (changeOccurred);

        return grid;
    }

    private static readonly List<char> ArrowChars = new()
    {
         '↑', '→', '↓', '←',
    };

    private static readonly Dictionary<char, string> PipeCharToAllowedOutputCompassMoves = new()
    {
        // Char, NESW
        { '|', "1010" },
        { '-', "0101" },
        { 'L', "1100" },
        { 'J', "1001" },
        { '7', "0011" },
        { 'F', "0110" },
    };

    private static readonly Dictionary<char, string> PipeCharToAllowedInputCompassMoves = new()
    {
        // Char, NESW
        { '|', "1010" },
        { '-', "0101" },
        { 'L', "0011" },
        { 'J', "0110" },
        { '7', "1100" },
        { 'F', "1001" },
    };

    private static readonly List<List<int>> CoordsDiffs = new()
    {
        // NESW
        new List<int> { -1,  0 },
        new List<int> {  0,  1 },
        new List<int> {  1,  0 },
        new List<int> {  0, -1 },
    };

    private static readonly List<List<char>> AcceptedPipesForInput = new()
    {
        PipeCharToAllowedInputCompassMoves.Where(x => x.Value[0] == '1').Select(x => x.Key).ToList(),
        PipeCharToAllowedInputCompassMoves.Where(x => x.Value[1] == '1').Select(x => x.Key).ToList(),
        PipeCharToAllowedInputCompassMoves.Where(x => x.Value[2] == '1').Select(x => x.Key).ToList(),
        PipeCharToAllowedInputCompassMoves.Where(x => x.Value[3] == '1').Select(x => x.Key).ToList()
    };
}
