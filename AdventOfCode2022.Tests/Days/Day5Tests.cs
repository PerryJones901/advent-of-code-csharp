using AdventOfCode2022.Days;

namespace AdventOfCode2022.Tests.Days
{
    public class Day5Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test1()
        {
            var hello = Day5.Part2(new List<string>()
            {
@"    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3",
@"move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2",
            });

            Assert.That(hello, Is.EqualTo("MCD"));
        }
    }
}
