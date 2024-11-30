using AdventOfCode2023.Helpers;
using AdventOfCodeCommon;

namespace AdventOfCode2023.Days;

public abstract class Day19
{
    public static int Part1Answer() =>
        Part1(GetSeparatedInputFromFile());

    public static long Part2Answer() =>
        Part2(GetSeparatedInputFromFile());

    public static int Part1(List<string> input)
    {
        var workflows = StringHelper.GetRegexCapturesFromInput(
            input[0],
            @"(\w+)\{(\S+)\}"
        ).ToDictionary(x => x[0], x => x[1].Split(','));
        var partRatings = StringHelper.GetRegexCapturesFromInput(
            input[1],
            @"(?:\{x=)(\d+)(?:,m=)(\d+)(?:,a=)(\d+)(?:,s=)(\d+)"
        ).Select(x => new Dictionary<char, int>
        {
            { 'x', int.Parse(x[0]) },
            { 'm', int.Parse(x[1]) },
            { 'a', int.Parse(x[2]) },
            { 's', int.Parse(x[3]) },
        })
        .ToList();

        var totalRatingNumbersFromAcceptedParts = 0;

        foreach (var part in partRatings)
        {
            var currentWorkflow = workflows["in"];
            var currentWorkflowSectionIndex = 0;
            var isAccepted = false;

            while (true)
            {
                var rule = currentWorkflow[currentWorkflowSectionIndex];
                if (rule == "A")
                {
                    isAccepted = true;
                    break;
                } else if (rule == "R")
                {
                    isAccepted = false;
                    break;
                }

                // Next, parse the comparisons,
                var output = GetOutputFromRuleStringOrNull(rule, part);

                // if fail comparison, increment currentWorkflowSectionIndex
                if (output == null)
                {
                    currentWorkflowSectionIndex++;
                    continue;
                }

                // if pass comparison see if its A, R or another workflow
                if (output == "A")
                {
                    isAccepted = true;
                    break;
                }
                else if (output == "R")
                {
                    isAccepted = false;
                    break;
                }

                // if another workflow, set currentWorkflow to that workflow
                currentWorkflow = workflows[output];
                currentWorkflowSectionIndex = 0;
            }

            if (isAccepted)
                totalRatingNumbersFromAcceptedParts += part.Sum(x => x.Value);
        }

        return totalRatingNumbersFromAcceptedParts;
    }

    private static string? GetOutputFromRuleStringOrNull(string rule, Dictionary<char, int> part)
    {
        // Check if there's a colon, if not, return the rule
        if (!rule.Contains(':'))
            return rule;

        var categoryLetter = rule[0];
        var comparitor = rule[1];
        var value = int.Parse(rule.Split(":")[0][2..]);
        var output = rule.Split(":")[1];

        var partValue = part[categoryLetter];
        if (
            comparitor == '>' && partValue > value
            || comparitor == '<' && partValue < value
            || comparitor == '=' && partValue == value
        )
            return output;
        else
            return null;
    }

    public static long Part2(List<string> input)
    {
        var totalSolutionSpace = new Dictionary<char, Interval>() {
            { 'x', new Interval() },
            { 'm', new Interval() },
            { 'a', new Interval() },
            { 's', new Interval() },
        };

        var workflows = StringHelper.GetRegexCapturesFromInput(
            input[0],
            @"(\w+)\{(\S+)\}"
        ).ToDictionary(x => x[0], x => x[1].Split(','));

        var tracker = new Tracker();
        FindSolutions(
            workflows["in"],
            0,
            workflows,
            totalSolutionSpace,
            tracker
        );

        return tracker.TotalAcceptedCases;
    }

    private static void FindSolutions(
        string[] currentWorkflow,
        int ruleIndex,
        Dictionary<string, string[]> workflows,
        Dictionary<char, Interval> solutionSpace, 
        Tracker tracker
    )
    {
        var rule = currentWorkflow[ruleIndex];
        if (rule == "A")
        {
            tracker.TallyAcceptedCases(solutionSpace);
            return;
        }
        else if (rule == "R")
        {
            return;
        }

        // Next, parse the comparisons
        var output = GetRuleResult(rule, solutionSpace);

        // For failure cases, increment currentWorkflowSectionIndex
        if (output.FalseSpace != null)
        {
            FindSolutions(
                currentWorkflow,
                ruleIndex + 1,
                workflows,
                output.FalseSpace,
                tracker
            );
        }

        // If true space is null, return
        if (output.TrueSpace == null)
            return;

        // For success cases, see if its A or R, or go to another workflow
        if (output.TrueOutput == "A")
        {
            tracker.TallyAcceptedCases(output.TrueSpace);
            return;
        }
        else if (output.TrueOutput == "R")
        {
            return;
        }

        // if another workflow, set currentWorkflow to that workflow
        FindSolutions(
            workflows[output.TrueOutput],
            0,
            workflows,
            output.TrueSpace,
            tracker
        );
    }

    private static RuleResult GetRuleResult(string rule, Dictionary<char, Interval> solutionSpace)
    {
        // Check if there's a colon, if not, return the rule
        if (!rule.Contains(':'))
            return new RuleResult
            {
                TrueSpace = new Dictionary<char, Interval>(solutionSpace),
                FalseSpace = null,
                TrueOutput = rule,
            };

        var categoryLetter = rule[0];
        var comparitor = rule[1];
        var value = int.Parse(rule.Split(":")[0][2..]);
        var output = rule.Split(":")[1];

        var spaceIntervalForLetter = solutionSpace[categoryLetter];

        // x > 5 and the solution space is 1-4, oh no
        if (comparitor == '>')
        {
            if (value >= spaceIntervalForLetter.Max)
            {
                // No true solutions
                return new RuleResult
                {
                    TrueSpace = null,
                    FalseSpace = new Dictionary<char, Interval>(solutionSpace),
                    TrueOutput = output,
                };
            }
            else if (spaceIntervalForLetter.Min > value)
            {
                // No false solutions
                return new RuleResult
                {
                    TrueSpace = new Dictionary<char, Interval>(solutionSpace),
                    FalseSpace = null,
                    TrueOutput = output,
                };
            }
            else
            {
                // Split solution in two
                var trueMin = value + 1;
                var trueMax = spaceIntervalForLetter.Max;
                var falseMin = spaceIntervalForLetter.Min;
                var falseMax = value;

                // Check for any zero length intervals

                var trueSpace = trueMin != trueMax
                    ? new Dictionary<char, Interval>(solutionSpace)
                    {
                        [categoryLetter] = new Interval { Min = trueMin, Max = trueMax }
                    }
                    : null;
                var falseSpace = falseMin != falseMax
                    ? new Dictionary<char, Interval>(solutionSpace)
                    {
                        [categoryLetter] = new Interval { Min = falseMin, Max = falseMax }
                    }
                    : null;

                return new RuleResult
                {
                    TrueSpace = trueSpace,
                    FalseSpace = falseSpace,
                    TrueOutput = output,
                };
            }
        } else if (comparitor == '<')
        {
            if (value <= spaceIntervalForLetter.Min)
            {
                // No true solutions
                return new RuleResult
                {
                    TrueSpace = null,
                    FalseSpace = new Dictionary<char, Interval>(solutionSpace),
                    TrueOutput = output,
                };
            }
            else if (spaceIntervalForLetter.Max < value)
            {
                // No false solutions
                return new RuleResult
                {
                    TrueSpace = new Dictionary<char, Interval>(solutionSpace),
                    FalseSpace = null,
                    TrueOutput = output,
                };
            }
            else
            {
                // Split solution in two
                var trueMin = spaceIntervalForLetter.Min;
                var trueMax = value - 1;
                var falseMin = value;
                var falseMax = spaceIntervalForLetter.Max;

                // Check for any zero length intervals

                var trueSpace = trueMin != trueMax
                    ? new Dictionary<char, Interval>(solutionSpace)
                    {
                        [categoryLetter] = new Interval { Min = trueMin, Max = trueMax }
                    }
                    : null;
                var falseSpace = falseMin != falseMax
                    ? new Dictionary<char, Interval>(solutionSpace)
                    {
                        [categoryLetter] = new Interval { Min = falseMin, Max = falseMax }
                    }
                    : null;

                return new RuleResult
                {
                    TrueSpace = trueSpace,
                    FalseSpace = falseSpace,
                    TrueOutput = output,
                };
            }
        } else
        {
            throw new Exception("Invalid comparitor");
        }
    }

    private class Interval
    {
        public int Min = 1;
        public int Max = 4000;
    }
    
    private class RuleResult
    {
        public Dictionary<char, Interval>? TrueSpace { get; set; }
        public Dictionary<char, Interval>? FalseSpace { get; set; }
        public string TrueOutput { get; set; }
    }


    private class Tracker
    {
        public long TotalAcceptedCases { get; private set; } = 0;

        public void TallyAcceptedCases(Dictionary<char, Interval> solutionSpace)
        {
            var totalSolutions = 1L;
            foreach (var interval in solutionSpace)
            {
                totalSolutions *= (interval.Value.Max - interval.Value.Min + 1);
            }
            TotalAcceptedCases += totalSolutions;
        }
    }

    private static List<string> GetSeparatedInputFromFile() =>
        FileInputHelper.GetStringListFromFile("Day19.txt", "\r\n\r\n");
}
