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


            int offset = 0;
            while (input[offset] != 99) 
            {
                switch (input[offset]) 
                {
                    case 1:
                        input[input[offset + 3]] = input[input[offset + 1]] + input[input[offset + 2]];
                        break;
                    case 2:
                        input[input[offset + 3]] = input[input[offset + 1]] * input[input[offset + 2]];
                        break;
                }
                offset += 4;
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

                    int offset = 0;
                    while (input[offset] != 99)
                    {
                        switch (input[offset])
                        {
                            case 1:
                                input[input[offset + 3]] = input[input[offset + 1]] + input[input[offset + 2]];
                                break;
                            case 2:
                                input[input[offset + 3]] = input[input[offset + 1]] * input[input[offset + 2]];
                                break;
                        }
                        offset += 4;
                    }

                    if (input[0] == 19690720)
                    {
                        answer = 100 * noun + verb;
                        return;
                    }
                }); 
            });

            return answer.ToString();
        }
    }
}
