using AdventOfCodeCommon;

namespace AdventOfCode2024.Days
{
    internal class Day09(bool isTest) : DayBase(9, isTest)
    {
        public override string Part1()
        {
            // Too high: 24249769251776
            var input = GetInput();
            var diskInfos = input.ToCharArray().Select((c, index) => new DiskInfo
            {
                Id = index % 2 == 0 ? index / 2 : null,
                Size = int.Parse(c.ToString())
            }).ToList();

            var files = diskInfos.Where(d => d.Id.HasValue).ToList();
            var diskSize = diskInfos.Sum(x => x.Size);

            // End File
            var currentEndFileIndex = files.Count - 1;
            var currentEndFileByteIndex = 0;

            var ascendingDiskIndex = 0;
            var descendingDiskIndex = 0;
            var checksum = 0L;

            foreach(var diskInfo in diskInfos)
            {
                // TODO: Add some logic about ascending and desc = length of disk

                for (int i = 0; i < diskInfo.Size; i++)
                {
                    // If file, need to add to checksum
                    if (diskInfo.Id.HasValue)
                    {
                        checksum += diskInfo.Id.Value * ascendingDiskIndex;
                    }
                    // If free space, take from other side of disk
                    else
                    {
                        // Get end file
                        var endFile = files[currentEndFileIndex];

                        // Now, check if we must move to next end file
                        if (currentEndFileByteIndex == endFile.Size)
                        {
                            if (endFile.Size == 0) throw new Exception("End file size is 0");

                            currentEndFileIndex--;
                            currentEndFileByteIndex = 0;
                            endFile = files[currentEndFileIndex];
                        }

                        // Now, we add to checksum with this end file
                        checksum += files[currentEndFileIndex].Id.Value * ascendingDiskIndex;

                        currentEndFileByteIndex++;
                        descendingDiskIndex++;
                    }

                    ascendingDiskIndex++;

                    // We are moving up by ascendingDiskIndex, and down by descendingDiskIndex
                    // Once these meet up, we should stop
                    if (ascendingDiskIndex == diskSize - descendingDiskIndex) break;
                }
            }

            return checksum.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();

            return "Part 2";
        }

        private class DiskInfo
        {
            // If null, it's free space
            public int? Id { get; set; }
            public int Size { get; set; }
        }

        private string GetInput()
        {
            return FileInputAssistant.GetStringFromFile(TextInputFilePath);
        }
    }
}
