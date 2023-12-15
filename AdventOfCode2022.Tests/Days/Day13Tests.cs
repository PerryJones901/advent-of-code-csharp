using AdventOfCode2022.Days;

namespace AdventOfCode2022.Tests.Days
{
    public class Day13Tests
    {
        private const string LINE_SEPARATOR = "\r\n";

        [SetUp]
        public void Setup()
        {

        }

        [TestCase($"[1,1,3,1,1]{LINE_SEPARATOR}[1,1,5,1,1]", 1)]
        [TestCase($"[[1],[2,3,4]]{LINE_SEPARATOR}[[1],4]", 1)] //
        [TestCase($"[9]{LINE_SEPARATOR}[[8,7,6]]", 0)]
        [TestCase($"[[4,4],4,4]{LINE_SEPARATOR}[[4,4],4,4,4]", 1)]
        [TestCase($"[7,7,7,7]{LINE_SEPARATOR}[7,7,7]", 0)]
        [TestCase($"[]{LINE_SEPARATOR}[3]", 1)]
        [TestCase($"[[[]]]{LINE_SEPARATOR}[[]]", 0)]
        [TestCase($"[1,[2,[3,[4,[5,6,7]]]],8,9]{LINE_SEPARATOR}[1,[2,[3,[4,[5,6,0]]]],8,9]", 0)] //
        public void Part1Test(string input, int rightOrderCount)
        {
            var answer = Day13.Part1(new List<string>(){ input });

            Assert.That(answer, Is.EqualTo(rightOrderCount));
        }
    }
}
