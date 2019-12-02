using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.challenge
{
    class Day2 : Challenge
    {
        public static string Solve1()
        {
            List<int> input = new List<int>();
            GetInputAsStringList(2).First().Split(',').ToList().ForEach(i => input.Add(int.Parse(i)));

            input[1] = 12;
            input[2] = 2;

            int offset = 0;
            while (true) 
            {
                if (input[offset] == 1)
                {
                    input[input[offset + 3]] = input[input[offset + 1]] + input[input[offset + 2]];
                }
                else if (input[offset] == 2)
                {
                    input[input[offset + 3]] = input[input[offset + 1]] * input[input[offset + 2]];
                }
                else if (input[offset] == 99) 
                {
                    break;
                }

                offset += 4;
            }

            return input[0].ToString();
        }

        public static string Solve2()
        {
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    List<int> input = new List<int>();
                    GetInputAsStringList(2).First().Split(',').ToList().ForEach(i => input.Add(int.Parse(i)));

                    input[1] = i;
                    input[2] = j;

                    int offset = 0;
                    while (true)
                    {
                        if (input[offset] == 1)
                        {
                            input[input[offset + 3]] = input[input[offset + 1]] + input[input[offset + 2]];
                        }
                        else if (input[offset] == 2)
                        {
                            input[input[offset + 3]] = input[input[offset + 1]] * input[input[offset + 2]];
                        }
                        else if (input[offset] == 99)
                        {
                            break;
                        }

                        offset += 4;
                    }

                    if (input[0] == 19690720)
                    {
                        return (100 * i + j).ToString();
                    }
                }
            }

            return "";
        }
    }
}
