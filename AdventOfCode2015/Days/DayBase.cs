namespace AdventOfCode2015.Days;

public abstract class DayBase(int dayNumber, bool isTest = false)
{
    // TODO: Don't hardcode this path
    private const string INPUT_FILES_PATH = "C:\\Work\\advent-of-code-csharp\\AdventOfCode2015\\InputFiles";

    protected string FileNameSuffix => IsTest ? "_Test" : "";
    protected string TextInputFileName => $"Day{DayNumber.ToString().PadLeft(2, '0')}{FileNameSuffix}";
    protected string TextInputFilePath => $"{INPUT_FILES_PATH}\\{TextInputFileName}.txt";

    protected readonly int DayNumber = dayNumber;
    protected readonly bool IsTest = isTest;

    public abstract string Part1();
    public abstract string Part2();
}
