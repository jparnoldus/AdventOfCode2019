using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode2019.challenge
{
    class Day22 : Challenge
    {
        public static string Solve1()
        {
            int deckSize = 10007;
            List<string> input = GetInputAsStringList(22);
            List<int> deck = Enumerable.Range(0, deckSize).ToList();

            foreach (string instruction in input)
            {
                if (instruction.StartsWith("cut -"))
                {
                    int skip = Math.Abs(int.Parse(instruction.Substring(4)));
                    deck = deck.TakeLast(skip).Union(deck.SkipLast(skip)).ToList();
                }
                else if (instruction.StartsWith("cut"))
                {
                    int skip = int.Parse(instruction.Substring(4));
                    deck = deck.Skip(skip).Union(deck.Take(skip)).ToList();
                }
                else if (instruction.StartsWith("deal into new stack"))
                {
                    deck.Reverse();
                }
                else if (instruction.StartsWith("deal with increment"))
                {
                    int increment = int.Parse(instruction.Substring("deal with increment ".Length));
                    List<int> newDeck = Enumerable.Repeat(0, deckSize).ToList();
                    int counter = 0;
                    deck.ForEach(v => newDeck[counter++ * increment % deckSize] = v);
                    deck = newDeck;
                }
            }

            return deck.IndexOf(2020).ToString();
        }

        public static string Solve2()
        {
            return "";
        }
    }
}
