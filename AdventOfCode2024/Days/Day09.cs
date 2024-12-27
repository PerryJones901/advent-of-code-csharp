using AdventOfCodeCommon;
using System.Diagnostics;

namespace AdventOfCode2024.Days
{
    internal class Day09(bool isTest) : DayBase(9, isTest)
    {
        public override string Part1()
        {
            var input = GetInput();
            var diskInfos = input.ToCharArray().Select((c, index) => new DiskInfo
            {
                Id = index % 2 == 0 ? index / 2 : null,
                Size = int.Parse(c.ToString())
            }).ToList();
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
            // TOO LOW: 4456112330
            var input = GetInput();
            var diskInfos = input.ToCharArray().Select((c, index) => new DiskInfo
            {
                Id = index % 2 == 0 ? index / 2 : null,
                Size = int.Parse(c.ToString())
            }).ToList();

            // InitIndexOnDrive
            var currentDiskInd = 0;
            foreach (var diskI in diskInfos)
            {
                diskI.IndexOnDrive = currentDiskInd;
                currentDiskInd += diskI.Size;
            }

            var newDiskInfos = diskInfos.ToList();
            var reverseFiles = diskInfos.Where(x => x.Id.HasValue).ToList();
            reverseFiles.Reverse();

            // In each iter, go from furthest right file, and see if there's a space for it near beginning
            // If so, place it there (by removing free space, placing file then any spare space left)
            // Keep going down file list
            // Now, make sure we always move file LEFT and never RIGHT
            foreach (var file in reverseFiles)
            {
                var currentDiskIndex = 0;

                for (var index = 0; index < newDiskInfos.Count; index++)
                {
                    var item = newDiskInfos[index];

                    // If too far, break;
                    if (item.IndexOnDrive < currentDiskIndex)
                        break;

                    // If NOT free space, continue
                    if (item.Id.HasValue)
                    {
                        currentDiskIndex += item.Size;
                        continue;
                    }
                    // If next file is size 0, need to do more logic, so throw for now
                    if (index < diskInfos.Count - 1 && newDiskInfos[index + 1].Size == 0)
                        throw new Exception("File size of 0 here");

                    // Now, check for enough free space
                    if (file.Size <= item.Size)
                    {
                        // We have enough! Let's move
                        newDiskInfos[index] = file;
                        newDiskInfos[index].IndexOnDrive = currentDiskIndex;

                        var freeSpaceDiff = item.Size - file.Size;
                        if (freeSpaceDiff > 0)
                        {
                            newDiskInfos.Insert(index + 1, new DiskInfo {
                                Size = freeSpaceDiff,
                                IndexOnDrive = currentDiskIndex + file.Size,
                            });
                        }

                        // TODO: Now. to remove original, and to merge any empty spaces

                        currentDiskIndex += item.Size;

                        break;
                    }
                }
            }

            var checksum = 0L;
            var driveIndex = 0;
            foreach (var diskInfo in newDiskInfos)
            {
                if (diskInfo.Id is not null)
                    checksum += (driveIndex * diskInfo.Size) + (diskInfo.Size) * (diskInfo.Size - 1) / 2;

                driveIndex += diskInfo.Size;
            }

            return checksum.ToString();
        }

        [DebuggerDisplay("Id:{Id},Size:{Size}")]
        private class DiskInfo
        {
            // If null, it's free space
            public int? Id { get; set; }
            public int Size { get; set; }
            public int? IndexOnDrive { get; set; } = null;
        }

        private string GetInput()
        {
            return FileInputAssistant.GetStringFromFile(TextInputFilePath);
        }
    }
}
