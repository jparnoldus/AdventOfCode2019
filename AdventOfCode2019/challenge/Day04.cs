using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode2019.challenge
{
    class Day04: Challenge
    {
        public static string Solve1()
        {
            return Enumerable.Range(int.Parse(GetInputAsStringList(4).First().Substring(0, 6)), int.Parse(GetInputAsStringList(4).First().Substring(7, 6)) - int.Parse(GetInputAsStringList(4).First().Substring(0, 6))).Where(n => n.ToString().Skip(1).Zip(n.ToString(), (c, p) => new { c, p }).All(i => i.c >= i.p)).Count(n => Enumerable.Range(1, 9).Select(i => i.ToString() + i.ToString()).Any(w => n.ToString().Contains(w))).ToString();
        }

        public static string Solve2()
        {
            return Enumerable.Range(int.Parse(GetInputAsStringList(4).First().Substring(0, 6)), int.Parse(GetInputAsStringList(4).First().Substring(7, 6)) - int.Parse(GetInputAsStringList(4).First().Substring(0, 6))).Where(n => n.ToString().Skip(1).Zip(n.ToString(), (c, p) => new { c, p }).All(i => i.c >= i.p)).Count(n => Enumerable.Range(1111, 9999).Select(i => i.ToString()).Where(i => i[0] != i[1] && i[3] != i[1] && i[1] == i[2]).Any(w => n.ToString().Contains(w) || n.ToString().StartsWith(w.Substring(1)) || n.ToString().EndsWith(w.Substring(0, 3)))).ToString();
        }
    }
}
