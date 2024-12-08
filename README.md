# Advent of Code solutions
Here you'll find my solutions to the annual [Advent of Code](https://adventofcode.com/) coding competition.

## Solution links
- [2024 solutions](https://github.com/PerryJones901/advent-of-code-csharp/tree/main/AdventOfCode2024/Days)
- [2023 solutions](https://github.com/PerryJones901/advent-of-code-csharp/tree/main/AdventOfCode2023/Days)
- [2022 solutions](https://github.com/PerryJones901/advent-of-code-csharp/tree/main/AdventOfCode2022/Days)

## Code structure
Each year has two projects:
1. AdventOfCode(YYYY)
2. AdventOfCode(YYYY).Tests

I tend to only use the test one on days where I'm very stumped.

There is also a [AdventOfCodeCommon](https://github.com/PerryJones901/advent-of-code-csharp/tree/main/AdventOfCodeCommon) project for reusable bits of code.

## How to run
Whilst writing this section out, I realise how unfriendly this code is for someone that's using a PC other than my own.
I would therefore highly advise you to just read the code in GitHub.

For the brave few:
I have some hardcoded paths for the input files, which requires you to checkout this repo in a folder called `"C:\Work\advent-of-code-csharp"`.
Input files for e.g. 2024 should be placed in `"C:\Work\advent-of-code-csharp\AdventOfCode2024\InputFiles"`.
Each year project will have a `Program.cs` file. You'll need to edit them slightly to run the correct day.

Oh how great it would be to have a spectacular CLI tool that not only relies on _relative_ paths for input, but handles all years flawlessly.
I yearn for such a setup.
For those of you reading this who have done this already: you are the best of us.
