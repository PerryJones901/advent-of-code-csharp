using AdventOfCodeCommon;
using AdventOfCodeCommon.Extensions;

namespace AdventOfCode2025.Days
{
    internal class Day07(bool isTest) : DayBase(7, isTest)
    {
        public override string Part1()
        {
            var input = GetInput();
            var tachyonBeamCols = new HashSet<int>();
            var startBeam = input[0].IndexOf('S');
            tachyonBeamCols.Add(startBeam);

            var splitCount = 0;

            foreach (var row in input.Skip(1))
            {
                var newTachyonBeamCols = new HashSet<int>();

                foreach (var currentBeam in tachyonBeamCols)
                {
                    if (row[currentBeam] == '^')
                    {
                        splitCount++;
                        newTachyonBeamCols.Add(currentBeam + 1);
                        newTachyonBeamCols.Add(currentBeam - 1);
                    }
                    else
                    {
                        newTachyonBeamCols.Add(currentBeam);
                    }
                }

                tachyonBeamCols = newTachyonBeamCols;
            }

            return splitCount.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();
            var tachyonBeamsColToTimelineCount = new Dictionary<int, long>();
            var startBeam = input[0].IndexOf('S');
            tachyonBeamsColToTimelineCount.Add(startBeam, 1);

            foreach (var row in input.Skip(1))
            {
                var newTachyonBeamsColToTimelineCount = new Dictionary<int, long>();

                foreach (var currentBeam in tachyonBeamsColToTimelineCount.Keys)
                {
                    var amount = tachyonBeamsColToTimelineCount[currentBeam];

                    if (row[currentBeam] == '^')
                    {
                        newTachyonBeamsColToTimelineCount.AddOrIncrement(key: currentBeam - 1, amount);
                        newTachyonBeamsColToTimelineCount.AddOrIncrement(key: currentBeam + 1, amount);
                        continue;
                    }

                    newTachyonBeamsColToTimelineCount.AddOrIncrement(key: currentBeam, amount);
                }

                tachyonBeamsColToTimelineCount = newTachyonBeamsColToTimelineCount;
            }

            var timelineCount = tachyonBeamsColToTimelineCount.Values.Sum();

            return timelineCount.ToString();
        }

        private string[] GetInput()
            => FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
    }
}
