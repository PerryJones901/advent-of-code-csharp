using AdventOfCode2023.Helpers;

namespace AdventOfCode2023.Days;

public abstract class Day20
{
    public static long Part1Answer() =>
        Part1(GetSeparatedInputFromFile());

    public static long Part2Answer() =>
        Part2(GetSeparatedInputFromFile());

    public static long Part1(List<string> input)
    {
        var moduleInputList = input.Select(x => x.Split(" -> ")).ToList();
        var broadcasterModuleInput = moduleInputList.Where(x => x[0] == "broadcaster").First();

        var flipFlopModulesInput = moduleInputList.Where(x => x[0][0] == '%').ToList();
        flipFlopModulesInput.Add(broadcasterModuleInput);
        // flipFlopModulesInput.Add(new[] { "output", "" });

        var flipFlopModules = flipFlopModulesInput.Select(x => x[0].Replace("%", "")).ToList();

        var conjunctionModulesInput = moduleInputList.Where(x => x[0][0] == '&').ToList();
        var conjunctionModules = conjunctionModulesInput.Select(x => x[0].Replace("&", "")).ToList();

        var outputsDict = moduleInputList.ToDictionary(
            x => x[0].Replace("%", "").Replace("&", ""),
            x => x[1].Split(", ").ToList()
        );

        var conjunctionInputsDict = conjunctionModulesInput.ToDictionary(
            x => x[0].Replace("&", ""),
            x => outputsDict
                .Where(y => y.Value.Contains(x[0].Replace("&", "")))
                .Select(x => x.Key)
                .ToList()
        );

        var states = flipFlopModulesInput.ToDictionary(
            x => x[0].Replace("%", "").Replace("&", ""),
            x => false
        );

        var numHighPulses = 0;
        var numLowPulses = 0;

        for (int i = 0; i < 1_000; i++)
        {
            // First, broadcaster
            var moduleQueue = new Queue<string>(new[] { "broadcaster" });
            numLowPulses++;

            while(moduleQueue.Count > 0)
            {
                var currentModule = moduleQueue.Dequeue();
                // Figure out broadcast value

                if (!states.ContainsKey(currentModule) && !conjunctionInputsDict.ContainsKey(currentModule))
                {
                    // Module doesn't output anything, so just continue
                    continue;
                }

                var broadcastValueIsHighPulse = !conjunctionInputsDict.ContainsKey(currentModule)
                    ? states[currentModule]
                    : !states
                        .Where(x => conjunctionInputsDict[currentModule].Contains(x.Key))
                        .All(x => x.Value);

                // Now, update the state of the current module
                states[currentModule] = broadcastValueIsHighPulse;

                // Now broadcast
                if (!outputsDict.ContainsKey(currentModule)) continue;

                foreach (var output in (outputsDict[currentModule]))
                {
                    // If a flip flop, and low pulse, flip the state
                    if (flipFlopModules.Contains(output))
                    {
                        if (broadcastValueIsHighPulse) continue; // Ignored

                        if (states.ContainsKey(output))
                            states[output] = !states[output];

                        moduleQueue.Enqueue(output);
                    }
                    else
                    {
                        // Just put on queue
                        moduleQueue.Enqueue(output);
                    }
                }

                // We've sent pulses, so increment counter
                if (broadcastValueIsHighPulse)
                    numHighPulses += outputsDict[currentModule].Count;
                else
                    numLowPulses += outputsDict[currentModule].Count;
            }
        }

        return numLowPulses * numHighPulses;
    }

    public static long Part2(List<string> input)
    {
        var moduleInputList = input.Select(x => x.Split(" -> ")).ToList();
        var broadcasterModuleInput = moduleInputList.Where(x => x[0] == "broadcaster").First();

        var flipFlopModulesInput = moduleInputList.Where(x => x[0][0] == '%').ToList();
        flipFlopModulesInput.Add(broadcasterModuleInput);
        // flipFlopModulesInput.Add(new[] { "output", "" });

        var flipFlopModules = flipFlopModulesInput.Select(x => x[0].Replace("%", "")).ToList();

        var conjunctionModulesInput = moduleInputList.Where(x => x[0][0] == '&').ToList();
        var conjunctionModules = conjunctionModulesInput.Select(x => x[0].Replace("&", "")).ToList();

        var outputsDict = moduleInputList.ToDictionary(
            x => x[0].Replace("%", "").Replace("&", ""),
            x => x[1].Split(", ").ToList()
        );

        var conjunctionInputsDict = conjunctionModulesInput.ToDictionary(
            x => x[0].Replace("&", ""),
            x => outputsDict
                .Where(y => y.Value.Contains(x[0].Replace("&", "")))
                .Select(x => x.Key)
                .ToList()
        );

        var states = flipFlopModulesInput.ToDictionary(
            x => x[0].Replace("%", "").Replace("&", ""),
            x => false
        );

        var numButtonPresses = 0;
        var firstOn = new Dictionary<string, int>();
        // cheating a little
        var inputsOfLg = conjunctionInputsDict["lg"];

        while (true)
        {
            numButtonPresses++;
            // First, broadcaster
            var moduleQueue = new Queue<string>(new[] { "broadcaster" });

            while (moduleQueue.Count > 0)
            {
                var currentModule = moduleQueue.Dequeue();
                // Figure out broadcast value

                if (!states.ContainsKey(currentModule) && !conjunctionInputsDict.ContainsKey(currentModule))
                {
                    // Module doesn't output anything, so just continue
                    continue;
                }

                var broadcastValueIsHighPulse = !conjunctionInputsDict.ContainsKey(currentModule)
                    ? states[currentModule]
                    : !states
                        .Where(x => conjunctionInputsDict[currentModule].Contains(x.Key))
                        .All(x => x.Value);

                if (
                    broadcastValueIsHighPulse
                    && inputsOfLg.Contains(currentModule)
                )
                {
                    if (!firstOn.ContainsKey(currentModule))
                        firstOn[currentModule] = numButtonPresses;

                    if (firstOn.Count == inputsOfLg.Count)
                        // LCM of all firstOn values
                        // DO NOT Return numButtonPresses, must be LCM
                        return numButtonPresses;
                }

                // Now, update the state of the current module
                states[currentModule] = broadcastValueIsHighPulse;

                // Now broadcast
                if (!outputsDict.ContainsKey(currentModule)) continue;

                foreach (var output in (outputsDict[currentModule]))
                {
                    // If output == 'rx', then we're done
                    if (output == "rx" && !broadcastValueIsHighPulse)
                        return numButtonPresses;

                    // If a flip flop, and low pulse, flip the state
                    if (flipFlopModules.Contains(output))
                    {
                        if (broadcastValueIsHighPulse) continue; // Ignored

                        if (states.ContainsKey(output))
                            states[output] = !states[output];

                        moduleQueue.Enqueue(output);
                    }
                    else
                    {
                        // Just put on queue
                        moduleQueue.Enqueue(output);
                    }
                }      
            }
        }
    }

    private static List<string> GetSeparatedInputFromFile() =>
        FileInputHelper.GetStringListFromFile("Day20.txt");
}
