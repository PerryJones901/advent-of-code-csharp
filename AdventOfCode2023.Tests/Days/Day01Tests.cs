using AdventOfCode2023.Days;

namespace AdventOfCode2023.Tests.Days
{
    public class Day01Tests
    {
        [SetUp]
        public void Setup() {}

        [TestCaseSource(nameof(TestCases))]
        public void Test1(string input, int output)
        {
            var setOfPasswords = new List<string>
            {
                input
            };
            var part1 = Day01.Part2(setOfPasswords);

            Assert.That(part1, Is.EqualTo(output));
        }

        public static object[] TestCases = new object[]
        {
            new object[] { "twone" , 21 },
            new object[] { "onetwothreefourfivesixseveneightnine", 19 },
            new object[] { "dqfournine5four2jmlqcgv", 42 },
            new object[] { "7ggzdnjxndfive", 75 },
            new object[] { "twofive4threenine", 29 },
            new object[] { "dttwonezbgmcseven5seven", 27 },
        };
    }
}
