using AdventOfCode2022.Helpers;

namespace AdventOfCode2022.Days
{
    public static class Day7
    {
        public static int THRESHOLD_FOR_PART1 = 100_000;
        public static int TOTAL_DISK_SPACE = 70_000_000;
        public static int FREE_DISK_SPACE_NEEDED = 30_000_000;

        public static int Part1Answer()
        {
            return Part1(GetSeparatedInputFromFile());
        }

        public static int Part2Answer()
        {
            return Part2(GetSeparatedInputFromFile());
        }

        public static int Part1(List<string> inputLines)
        {
            var fileSystem = GetFileSystem(inputLines);
            var tracker = new Tracker();
            var totalSizeOfFileSystem = TotalSizesFromFolder(fileSystem, "/", tracker);
            return tracker.TotalSizes;
        }

        public static int Part2(List<string> inputLines)
        {
            var fileSystem = GetFileSystem(inputLines);
            var tracker = new Tracker();
            var totalSizeOfFileSystem = TotalSizesFromFolder(fileSystem, "/", tracker);

            var currentFreeSpace = TOTAL_DISK_SPACE - totalSizeOfFileSystem;
            var extraFreeSpaceRequired = FREE_DISK_SPACE_NEEDED - currentFreeSpace;

            // We must find first folder which exceeds currentFreeSpace when ordered.
            return tracker.FolderSizes
                .OrderBy(x => x)
                .First(x => x > extraFreeSpaceRequired);
        }

        public class Tracker
        {
            public int TotalSizes = 0;
            public HashSet<int> FolderSizes = new();
        }

        public static int TotalSizesFromFolder(Folder folder, string folderName, Tracker tracker)
        {
            var sizeOfImmediateSmallChildren = folder.Files.Sum(f => f.Value.Size);

            var sizeOfThisFolder =
                sizeOfImmediateSmallChildren
                + folder.Folders.Sum(f => TotalSizesFromFolder(f.Value, f.Key, tracker));

            if (sizeOfThisFolder <= THRESHOLD_FOR_PART1) tracker.TotalSizes += sizeOfThisFolder;

            tracker.FolderSizes.Add(sizeOfThisFolder);

            return sizeOfThisFolder;
        }

        public static Folder GetFileSystem(List<string> inputLines)
        {
            var rootFolder = new Folder();
            var currentFolder = rootFolder;

            foreach (var line in inputLines)
            {
                if (line.StartsWith("$ cd"))
                {
                    var param = line.Split(' ')[2];
                    if (param == "..")
                    {
                        if (currentFolder.Parent == null) throw new Exception();
                        currentFolder = currentFolder.Parent;
                    }
                    else if (param == "/")
                    {
                        currentFolder = rootFolder;
                    }
                    else
                    {
                        currentFolder = currentFolder.Folders[param];
                    }
                }
                else if (line.StartsWith("$ ls"))
                {
                    // Do nothing for now
                }
                else if (line.StartsWith("dir"))
                {
                    var folderName = line.Split(' ')[1];
                    if (!currentFolder.Folders.ContainsKey(folderName))
                        currentFolder.Folders.Add(
                            folderName, 
                            new Folder { Parent = currentFolder }
                        );
                }
                else
                {
                    // Create File and add to current folder
                    var fileSize = int.Parse(line.Split(' ')[0]);
                    var fileName = line.Split(' ')[1];
                    currentFolder.Files.Add(fileName, new File
                    {
                        Name = fileName,
                        Size = fileSize,
                    });
                }
            }
            return rootFolder;
        }

        public static List<string> GetSeparatedInputFromFile() => FileInputHelper
            .GetStringListFromFile("Day7.txt");
    }

    public class Folder
    {
        public Folder? Parent = null;
        public Dictionary<string, Folder> Folders { get; set; } = new Dictionary<string, Folder>();
        public Dictionary<string, File> Files { get; set; } = new Dictionary<string, File>();
    }

    public class File
    {
        public string Name { get; set; } = "";
        public int Size { get; set; } = 0;
    }
}
