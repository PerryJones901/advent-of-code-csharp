using AdventOfCodeCommon;
using AdventOfCodeCommon.Extensions;

namespace AdventOfCode2025.Days
{
    internal class Day07(bool isTest) : DayBase(7, isTest)
    {
        public override string Part1() => GetCount(CountType.Splits);
        public override string Part2() => GetCount(CountType.Timelines);

        private string GetCount(CountType countType)
        {
            var input = GetInput();
            var startBeamCol = input[0].IndexOf('S');
            var splitCount = 0L;

            var beamColToTimelineCount = new Dictionary<int, long>
            {
                { startBeamCol, 1 }
            };

            foreach (var row in input.Skip(1))
            {
                var newBeamColToTimelineCount = new Dictionary<int, long>();

                foreach (var currentBeam in beamColToTimelineCount.Keys)
                {
                    var amount = beamColToTimelineCount[currentBeam];

                    if (row[currentBeam] == '^')
                    {
                        splitCount++;
                        newBeamColToTimelineCount.AddOrIncrement(key: currentBeam - 1, amount);
                        newBeamColToTimelineCount.AddOrIncrement(key: currentBeam + 1, amount);
                        continue;
                    }

                    newBeamColToTimelineCount.AddOrIncrement(key: currentBeam, amount);
                }

                beamColToTimelineCount = newBeamColToTimelineCount;
            }

            var count = countType switch
            {
                CountType.Splits => splitCount,
                CountType.Timelines => beamColToTimelineCount.Values.Sum(),
                _ => throw new NotImplementedException()
            };

            return count.ToString();
        }

        private enum CountType
        {
            Splits,
            Timelines
        }

        private string[] GetInput()
            => FileInputAssistant.GetStringArrayFromFile(TextInputFilePath);
    }
}
