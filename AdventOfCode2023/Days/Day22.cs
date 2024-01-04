using AdventOfCode2023.Helpers;
using System.Diagnostics;

namespace AdventOfCode2023.Days;

public abstract class Day22
{
    public static int Part1Answer() =>
        Part1(GetSeparatedInputFromFile());

    public static int Part2Answer() =>
        Part2(GetSeparatedInputFromFile());

    public static int Part1(List<string> input)
    {
        var allBricks = GetBricks(input);
        var bricksDict = allBricks
            .GroupBy(x => x.Z)
            .OrderBy(x => x.Key)
            .ToDictionary(
                x => x.Key, 
                x => x.ToList()
            );

        bool hasBrickMoved;
        var newBricksDict = bricksDict.ToDictionary(
            x => x.Key,
            x => x.Value.Select(x => x).ToList()
        );

        do
        {
            hasBrickMoved = false;
            var dict = new Dictionary<int, List<Brick>>();

            foreach (var (zCoord, bricks) in newBricksDict)
            {
                // If Z = 1, blocks are at rest
                if (zCoord == 1)
                {
                    foreach (var brick in bricks)
                        brick.IsAtRest = true;

                    if (!dict.ContainsKey(zCoord))
                        dict.Add(zCoord, new List<Brick>());

                    dict[zCoord].AddRange(bricks);
                    continue;
                }

                // For Z-bricks, if their edge is at level 1, they are at rest
                foreach (var brick in bricks.Where(x => x.Direction == Direction.Z && x.Z + x.Displacement == 1))
                    brick.IsAtRest = true;

                // add bricks at rest now
                var bricksAtRest = bricks.Where(x => x.IsAtRest).ToList();
                if (bricksAtRest.Any())
                {
                    if (!dict.ContainsKey(zCoord))
                        dict.Add(zCoord, new List<Brick>());
                    dict[zCoord].AddRange(bricksAtRest);
                }

                // now deal with unrested bricks, that could potentially be at rest now
                var bricksNotAtRest = bricks.Where(x => !x.IsAtRest).ToList();

                foreach (var brick in bricksNotAtRest)
                {
                    // Find all the bricks at rest 1 level beneath this brick
                    var zValueOneBelowBrick = brick.Direction == Direction.Z
                        ? zCoord + brick.Displacement - 1
                        : zCoord - 1;

                    var bricksAtOneBelow = (newBricksDict.ContainsKey(zValueOneBelowBrick)
                        ? newBricksDict[zValueOneBelowBrick]
                        : new List<Brick>()).ToList();
                    var bricksAtRestOneBelow = bricksAtOneBelow
                        .Where(x => x.IsAtRest)
                        .ToList();

                    // Now, go through each brick at rest and see if it's in the way
                    Brick? lastBrickUnderneath = null;
                    int bricksUnderneathCount = 0;
                    foreach (var restedBrick in bricksAtRestOneBelow)
                    {
                        if (BricksAreTouching(brick, restedBrick))
                        {
                            bricksUnderneathCount++;
                            brick.IsAtRest = true;
                            lastBrickUnderneath = restedBrick;

                            // Now, do not break, as we want all the bricks at rest to be marked
                            // break;
                        }
                    }

                    if (lastBrickUnderneath != null && bricksUnderneathCount == 1)
                        // We cannot remove this brick underneath
                        lastBrickUnderneath.IsUnremovable = true;

                    if (!brick.IsAtRest)
                        hasBrickMoved = true;
                }

                // Now, the bricks not originally at rest, but now at rest, need to be added to the dictionary
                var bricksNowAtRest = bricksNotAtRest.Where(x => x.IsAtRest).ToList();
                var bricksStillNotAtRest = bricksNotAtRest.Where(x => !x.IsAtRest).ToList();

                if (bricksNowAtRest.Any())
                {
                    if (!dict.ContainsKey(zCoord))
                        dict.Add(zCoord, new List<Brick>());
                    dict[zCoord].AddRange(bricksNowAtRest);
                }
                if (bricksStillNotAtRest.Any())
                {
                    var newZCoord = zCoord - 1;
                    if (!dict.ContainsKey(newZCoord))
                        dict.Add(newZCoord, new List<Brick>());
                    dict[newZCoord].AddRange(bricksStillNotAtRest);
                }
            }

            newBricksDict = dict.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        } while(hasBrickMoved);

        return newBricksDict.SelectMany(x => x.Value).Count(x => !x.IsUnremovable);
    }

    private static List<Brick> GetBricks(List<string> input)
    {
        return input.Select(x =>
        {
            var split = x.Split('~');
            var firstCoords = split[0].Split(',').Select(int.Parse).ToList();
            var secondsCoords = split[1].Split(',').Select(int.Parse).ToList();

            var xCoord = firstCoords[0];
            var yCoord = firstCoords[1];
            var zCoord = firstCoords[2];

            var direction = Direction.X;
            var displacement = 0;

            if (firstCoords[0] != secondsCoords[0])
            {
                direction = Direction.X;

                // Now, choose the smallest X edge
                if (firstCoords[0] > secondsCoords[0])
                {
                    xCoord = secondsCoords[0];
                    yCoord = secondsCoords[1];
                    zCoord = secondsCoords[2];

                    displacement = firstCoords[0] - secondsCoords[0];
                }
                else
                    displacement = secondsCoords[0] - firstCoords[0];

            }
            else if (firstCoords[1] != secondsCoords[1])
            {
                direction = Direction.Y;

                // Now, choose the smallest X edge
                if (firstCoords[1] > secondsCoords[1])
                {
                    xCoord = secondsCoords[0];
                    yCoord = secondsCoords[1];
                    zCoord = secondsCoords[2];

                    displacement = firstCoords[1] - secondsCoords[1];
                }
                else
                    displacement = secondsCoords[1] - firstCoords[1];
            }
            else if (firstCoords[2] != secondsCoords[2])
            {
                direction = Direction.Z;

                // Now choose largest Z edge
                if (firstCoords[2] > secondsCoords[2])
                    displacement = secondsCoords[2] - firstCoords[2];
                else
                {
                    xCoord = secondsCoords[0];
                    yCoord = secondsCoords[1];
                    zCoord = secondsCoords[2];

                    displacement = firstCoords[2] - secondsCoords[2];
                }
            }

            return new Brick
            {
                X = xCoord,
                Y = yCoord,
                Z = zCoord,
                Direction = direction,
                Displacement = displacement
            };
        }).ToList();
    }

    // We assume that they differ by 1 in the Z direction
    private static bool BricksAreTouching(Brick firstBrick, Brick secondBrick)
    {
        var firstMinX = firstBrick.X;
        var firstMaxX = firstBrick.X + (firstBrick.Direction == Direction.X ? firstBrick.Displacement : 0);
        var firstMinY = firstBrick.Y;
        var firstMaxY = firstBrick.Y + (firstBrick.Direction == Direction.Y ? firstBrick.Displacement : 0);

        var secondMinX = secondBrick.X;
        var secondMaxX = secondBrick.X + (secondBrick.Direction == Direction.X ? secondBrick.Displacement : 0);
        var secondMinY = secondBrick.Y;
        var secondMaxY = secondBrick.Y + (secondBrick.Direction == Direction.Y ? secondBrick.Displacement : 0);

        // Now, got to check it fits
        var isCrossingX = (firstMinX <= secondMinX && firstMaxX >= secondMinX) ||
                          (firstMinX <= secondMaxX && firstMaxX >= secondMaxX) ||
                          (secondMinX <= firstMinX && secondMaxX >= firstMinX) ||
                          (secondMinX <= firstMaxX && secondMaxX >= firstMaxX);
        var isCrossingY = (firstMinY <= secondMinY && firstMaxY >= secondMinY) ||
                          (firstMinY <= secondMaxY && firstMaxY >= secondMaxY) ||
                          (secondMinY <= firstMinY && secondMaxY >= firstMinY) ||
                          (secondMinY <= firstMaxY && secondMaxY >= firstMaxY);

        return isCrossingX && isCrossingY;
    }

    public static int Part2(List<string> input)
    {
        return 0;
    }

    [DebuggerDisplay("X={X},Y={Y},Z={Z},Direction={Direction},Dis={Displacement},Rest={IsAtRest},Removable={IsUnremovable}")]
    private class Brick
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public Direction Direction { get; set; }
        public int Displacement { get; set; }
        public bool IsAtRest { get; set; } = false;
        public bool IsUnremovable { get; set; } = false;
    }

    private enum Direction
    {
        X, Y, Z
    }

    private static List<string> GetSeparatedInputFromFile() =>
        FileInputHelper.GetStringListFromFile("Day22.txt");
}
