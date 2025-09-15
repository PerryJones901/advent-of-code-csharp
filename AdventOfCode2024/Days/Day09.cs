using AdventOfCodeCommon;
using System.Diagnostics;

namespace AdventOfCode2024.Days
{
    internal class Day09(bool isTest) : DayBase(9, isTest)
    {
        public override string Part1()
        {
            var input = GetInput();
            var diskInfos = GetDiskInfosFromInput(input);
            var diskSize = diskInfos.Sum(x => x.Size);

            var files = diskInfos.Where(d => d.Id.HasValue).ToList();
            var filesSize = files.Sum(x => x.Size);

            // End File
            var currentEndFileIndex = files.Count - 1;
            var currentEndFileByteIndex = 0;

            var ascendingDiskIndex = 0;
            var checksum = 0L;

            foreach(var diskInfo in diskInfos)
            {
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
                            // Find next file that has non-zero size
                            currentEndFileByteIndex = 0;

                            do
                            {
                                currentEndFileIndex--;
                                endFile = files[currentEndFileIndex];
                            }
                            while (endFile.Size == 0);
                        }

                        // Now, we add to checksum with this end file
                        checksum += files[currentEndFileIndex].Id.Value * ascendingDiskIndex;

                        currentEndFileByteIndex++;
                    }

                    ascendingDiskIndex++;

                    if (ascendingDiskIndex == filesSize) break;
                }

                if (ascendingDiskIndex == filesSize) break;
            }

            return checksum.ToString();
        }

        public override string Part2()
        {
            var input = GetInput();
            var diskInfos = GetDiskInfosFromInput(input);

            RemoveEmptySpaces(diskInfos);
            CombineFreeSpaces(diskInfos);

            while (true)
            {
                var lastFileIndex = diskInfos.FindLastIndex(x => x.Id.HasValue && !x.IsProcessed);
                if (lastFileIndex == -1)
                    break;

                var lastFile = diskInfos[lastFileIndex];

                var firstSpaceBigEnoughIndex = diskInfos.FindIndex(x => !x.Id.HasValue && x.Size >= lastFile.Size);
                if (firstSpaceBigEnoughIndex != -1 && firstSpaceBigEnoughIndex < lastFileIndex)
                {
                    // Move the file
                    var freeSpace = diskInfos[firstSpaceBigEnoughIndex];

                    // Old file position is now a free space
                    diskInfos[lastFileIndex] = new DiskInfo
                    {
                        Id = null,
                        Size = lastFile.Size,
                    };

                    diskInfos[firstSpaceBigEnoughIndex] = lastFile;

                    // Is there space left? If so, add it
                    var remainingSpace = freeSpace.Size - lastFile.Size;
                    if (remainingSpace > 0)
                    {
                        diskInfos.Insert(firstSpaceBigEnoughIndex + 1, new DiskInfo
                        {
                            Id = null,
                            Size = freeSpace.Size - lastFile.Size,
                        });

                        CombineFreeSpaces(diskInfos);
                    }
                }

                lastFile.IsProcessed = true;
            }

            var checksum = 0L;
            var driveIndex = 0;
            foreach (var diskInfo in diskInfos)
            {
                if (diskInfo.Id is not null)
                    for (int i = 0; i < diskInfo.Size; i++)
                        checksum += (long)diskInfo.Id * (driveIndex + i);

                driveIndex += diskInfo.Size;
            }

            return checksum.ToString();
        }

        private static List<DiskInfo> GetDiskInfosFromInput(string input)
        {
            return input.ToCharArray().Select((c, index) => new DiskInfo
            {
                Id = index % 2 == 0 ? index / 2 : null,
                Size = int.Parse(c.ToString())
            }).ToList();
        }

        private static void RemoveEmptySpaces(List<DiskInfo> diskInfos)
        {
            for (var index = 0; index < diskInfos.Count; index++)
            {
                var item = diskInfos[index];

                if (item.Size > 0)
                    continue;

                diskInfos.RemoveAt(index);
                index--;
            }
        }

        private static void CombineFreeSpaces(List<DiskInfo> diskInfos)
        {
            for (var index = 0; index < diskInfos.Count - 1; index++)
            {
                var item = diskInfos[index];
                var nextItem = diskInfos[index + 1];
                if (!item.Id.HasValue && !nextItem.Id.HasValue)
                {
                    item.Size += nextItem.Size;
                    diskInfos.RemoveAt(index + 1);
                    index--;
                }
            }
        }

        [DebuggerDisplay("Id:{Id},Size:{Size},IsProcessed:{IsProcessed}")]
        private class DiskInfo
        {
            // If null, it's free space
            public int? Id { get; set; }
            public int Size { get; set; }
            public bool IsProcessed { get; set; } = false;
        }

        private string GetInput()
        {
            return FileInputAssistant.GetStringFromFile(TextInputFilePath);
        }
    }
}
