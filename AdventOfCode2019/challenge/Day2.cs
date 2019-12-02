using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.challenge
{
    class Day2 : Challenge
    {
        public static string Solve1()
        {
            List<int> input = GetInputAsCsIntListList(2).First();
            Array.ForEach<int>(input.ToArray(), i => { 
                input[1] = 12; 
                input[2] = 2; 
            });

            for (int offset = 0; input[offset] != 99; offset += 4)
            {
                input[input[offset + 3]] = input[offset] == 1 ? 
                    input[input[offset + 1]] + input[input[offset + 2]] : input[offset] == 2 ? 
                        input[input[offset + 1]] * input[input[offset + 2]] : input[input[offset + 3]];
            }

            return input.First().ToString();
        }

        public static string Solve2()
        {
            int answer = 0;
            Enumerable.Range(0, 99).ToList().ForEach(noun => { 
                Enumerable.Range(0, 99).ToList().ForEach(verb => {
                    List<int> input = GetInputAsCsIntListList(2).First();
                    Array.ForEach<int>(input.ToArray(), i => {
                        input[1] = noun;
                        input[2] = verb;
                    });

                    for (int offset = 0; input[offset] != 99; offset += 4)
                    {
                        input[input[offset + 3]] = input[offset] == 1 ?
                            input[input[offset + 1]] + input[input[offset + 2]] : input[offset] == 2 ?
                                input[input[offset + 1]] * input[input[offset + 2]] : input[input[offset + 3]];
                    }

                    _ = input.First().Equals(19690720) ? answer = 100 * noun + verb : 0;
                }); 
            });

            return answer.ToString();
        }
    }
}
