using System.Linq;

namespace AdventOfCode2019.challenge
{
    class Day1: Challenge
    {
        public static string Solve1()
        {
            return GetInputAsIntList(1).Select(i => i = (int)((float)i / 3) - 2).Sum().ToString();
        }

        public static string Solve2()
        {
            int total = 0;
            GetInputAsIntList(1).ForEach(i => { while (i > 0) { i = (int)((float)i / 3) - 2; total += i > 0 ? i : 0; }});

            return total.ToString();
        }
    }
}
