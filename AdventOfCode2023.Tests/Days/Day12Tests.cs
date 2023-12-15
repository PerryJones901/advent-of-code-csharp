using AdventOfCode2023.Days;

namespace AdventOfCode2023.Tests.Days
{
    public class Day12Tests
    {
        [SetUp]
        public void Setup() {}

        [TestCaseSource(nameof(TestCases))]
        public void Test1(string input, int output)
        {
            var inputList = new List<string>
            {
                input
            };
            var part1 = Day12.Part1(inputList);

            Assert.That(part1, Is.EqualTo(output));
        }

        public static object[] TestCases = new object[]
        {
            new object[] { "????? 2", 4 },
            new object[] { "??#??## 1,5", 1 },
            new object[] { "???#??## 1,5", 2 },
            new object[] { "????#??## 1,5", 3 },
            new object[] { "????#??##.?? 1,5,2", 3 },
            new object[] { "????#??##.????? 1,5,2", 12 },
            new object[] { "..#???.??.. 4,1", 2 },
            new object[] { "??.??????? 1,4,1", 6 },
            new object[] { "#??.????#??. 2,1,2,1", 2 },
            new object[] { "?#????##..#????? 8,1,1,1", 3 },
            new object[] { ".????.?.??. 2,1,1", 9 },
        };
    }
}
