using AdventOfCode2022.Days;

namespace AdventOfCode2022.Tests.Days
{
    public class Day8Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test1()
        {
            var day8part2 = Day8.Part2(new List<string>()
            {
                "30373",
                "25512",
                "65332",
                "33549",
                "35390",
            });

            Assert.That(day8part2, Is.EqualTo(8));
        }
    }
}
