using AdventOfCode2015.Days;
using System.Diagnostics;

var isTest = false;
var day = new Day01(isTest);

Stopwatch stopwatch = new();

stopwatch.Start();
Console.WriteLine(day.Part1());
stopwatch.Stop();
Console.WriteLine($"Part 1 took {stopwatch.ElapsedMilliseconds}ms");

stopwatch.Restart();
Console.WriteLine(day.Part2());
stopwatch.Stop();
Console.WriteLine($"Part 2 took {stopwatch.ElapsedMilliseconds}ms");
