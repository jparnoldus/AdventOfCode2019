using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode2019.challenge
{
    class Day4: Challenge
    {
        public static string Solve1()
        {
            var possibilities = Enumerable.Range(1, 9).Select(i => i.ToString() + i.ToString());

            var input = GetInputAsStringList(4).First().Split('-');
            int start = int.Parse(input[0]);
            int end = int.Parse(input[1]);

            int total = 0;
            for (int i = start; i < end; i++) {
                string stringVersion = i.ToString();
                if (possibilities.Any(w => stringVersion.Contains(w))) {

                    bool ok = true;
                    for (int j = 1; j < stringVersion.Length; j++) {
                        if (stringVersion[j] < stringVersion[j - 1]) {
                            ok = false;
                        }
                    }

                    if (!ok) {
                        continue;
                    }

                    total++;
                }
            }

            return total.ToString();
        }

        public static string Solve2()
        {
            var possibilities = Enumerable.Range(1111, 9999).Select(i => i.ToString()).Where(i => i[0] != i[1] && i[3] != i[1] && i[1] == i[2]);

            var input = GetInputAsStringList(4).First().Split('-');
            int start = int.Parse(input[0]);
            int end = int.Parse(input[1]);

            int total = 0;
            for (int i = start; i < end; i++)
            {
                string stringVersion = i.ToString();
                if (possibilities.Any(w => stringVersion.Contains(w) || stringVersion.StartsWith(w.Substring(1)) || stringVersion.EndsWith(w.Substring(0, 3))))
                {

                    bool ok = true;
                    for (int j = 1; j < stringVersion.Length; j++)
                    {
                        if (stringVersion[j] < stringVersion[j - 1])
                        {
                            ok = false;
                        }
                    }

                    if (!ok)
                    {
                        continue;
                    }

                    Console.WriteLine(i.ToString());
                    total++;
                }
            }

            return total.ToString();
        }
    }
}
