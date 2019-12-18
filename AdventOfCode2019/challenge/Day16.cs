﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode2019.challenge
{
    class Day16 : Challenge
    {
        public static string Solve1()
        {
            List<int> input = GetInputAsStringList(16).First().Select(s => int.Parse(s.ToString())).ToList();

            for (int k = 0; k < 100; k++)
            {
                List<int> output = new List<int>();
                Enumerable.Range(1, input.Count).ToList().ForEach(n =>
                {
                    List<int> pattern = Enumerable.Repeat(new List<int>() { 0, 1, 0, -1 }.SelectMany(i => Enumerable.Repeat(i, n)).ToList(), (input.Count / (n * 4)) + 1).SelectMany(i => i).Skip(1).ToList();
                    output.Add(int.Parse(input.Zip(pattern).Select(i => i.First * i.Second).Sum().ToString().Last().ToString()));
                });
                input = output;
            }

            return string.Join("", input.Take(8));
        }

        public static string Solve2()
        {
            List<int> input = GetInputAsStringList(16).First().Select(s => int.Parse(s.ToString())).ToList();
            int skip = int.Parse(string.Join("", input.Take(7)));

            for (int k = 0; k < 100; k++)
            {
                Dictionary<int, int> offsetSum = new Dictionary<int, int>();
                List<int> output = new List<int>();
                Enumerable.Range(1, input.Count).ToList().ForEach(n =>
                {
                    List<int> pattern = Enumerable.Repeat(new List<int>() { 0, 1, 0, -1 }.SelectMany(i => Enumerable.Repeat(i, n)).ToList(), (input.Count / (n * 4)) + 1).SelectMany(i => i).Skip(1).ToList();
                    output.Add(int.Parse(input.Zip(pattern).Select(i => i.First * i.Second).Sum().ToString().Last().ToString()));
                });
                input = output;
            }

            return string.Join("", input.Skip(skip).Take(8));
        }
    }
}
