using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode2019.challenge
{
    class Day8 : Challenge
    {
        public static string Solve1()
        {
            List<int> input = GetInputAsIntRow(8);
            List<List<int>> layers = new List<List<int>>();

            int width = 25;
            int height = 6;
            for (int i = 0; i < input.Count; i += width * height)
            {
                List<int> layer = new List<int>();
                for (int j = 0; j < width * height; j++)
                {
                    layer.Add(input[j+i]);
                }
                layers.Add(layer);
            }

            List<int> mostZeroes = layers.OrderBy(i => i.Count(t => t == 0)).First();
            return (mostZeroes.Count(i => i == 1) * mostZeroes.Count(i => i == 2)).ToString();
        }

        public static string Solve2()
        {
            List<int> input = GetInputAsIntRow(8);
            List<List<int>> layers = new List<List<int>>();

            int width = 25;
            int height = 6;
            for (int i = 0; i < input.Count; i += width * height)
            {
                List<int> layer = new List<int>();
                for (int j = 0; j < width * height; j++)
                {
                    layer.Add(input[i + j]);
                }
                layers.Add(layer);
            }

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int layer = 0;
                    while (layer != layers.Count)
                    {
                        if (layers[layer][(i*width) + j] == 0)
                        {
                            Console.Write('#');
                            break;
                        }
                        else if (layers[layer][(i * width) + j] == 1)
                        {
                            Console.Write('-');
                            break;
                        }
                        layer++;
                    }
                }
                Console.WriteLine();
            }

            return "";
        }
    }
}
