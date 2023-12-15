using AdventOfCode2023.Helpers;

namespace AdventOfCode2023.Days;

public abstract class Day06
{
    public static int Part1Answer() =>
        Part1(GetSeparatedInputFromFile());

    public static long Part2Answer() =>
        Part2(GetSeparatedInputFromFile());

    public static int Part1(List<string> input)
    {
        var times = input[0].Split(' ').Skip(1).Where(x => x != "").Select(int.Parse).ToList();
        var distances = input[1].Split(' ').Skip(1).Where(x => x != "").Select(int.Parse).ToList();

        return BruteForce(times, distances);
    }

    private static long GetResult(List<long> times, List<long> distances)
    {
        var races = times.Zip(distances, (time, distance) => new { time, distance });

        long product = 1;

        foreach (var race in races)
        {
            // Formula for distance = x*(T - x)
            // So we have x*T - x^2 > D
            // So -x^2 + x*T - D > 0
            // So (-b +- sqrt(b^2 - 4*a*c))/(2a)
            // So (-T +- sqrt(T^2 - 4(-1)(-D)))/(-2)
            // So (T +- sqrt(T^2 - 4d))/2

            // Important part is sqrt(T^2 - 4D) - this will be (give it or take) the answer each time
            // If T is even, we have a value at the peak.
            // If (T^2 - 4D) is square, we have two values on x axis

            var discriminant = race.time * race.time - 4 * race.distance;
            long numTimesCanWin;
            if (race.time % 2 == 0)
            {
                numTimesCanWin = (((int)Math.Floor(Math.Sqrt(discriminant) / 2.0)) * 2) - 1;
            }
            else
            {
                numTimesCanWin = (int)Math.Floor((Math.Sqrt(discriminant) + 1.0) / 2.0) * 2;
            }

            product *= numTimesCanWin;
        }

        return product;
    }

    private static int BruteForce(List<int> times, List<int> distances)
    {
        var races = times.Zip(distances, (time, distance) => new { time, distance });

        var product = 1;

        foreach (var race in races)
        {
            var numTimesWin = 0;
            for (int i = 0; i < race.time; i++)
            {
                if (i * (race.time - i) > race.distance) numTimesWin++;
            }
            product *= numTimesWin;
        }
        return product;
    }

    public static long Part2(List<string> input)
    {
        var time = long.Parse(string.Join("", input[0].Split(":")[1].Replace(" ", "")));
        var distance = long.Parse(string.Join("", input[1].Split(":")[1].Replace(" ", "")));

        return GetResult(new List<long>() { time }, new List<long> { distance });
    }

    private static List<string> GetSeparatedInputFromFile() =>
        FileInputHelper.GetStringListFromFile("Day06.txt");
}
