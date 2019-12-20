using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode2019.challenge
{
    class Day19 : Challenge
    {
        public static string Solve1()
        {
            List<Point> map = new List<Point>();
            for (int y = 0; y < 50; y++)
            {
                for (int x = 0; x < 50; x++)
                {
                    List<long> commands = GetInputAsCsFloatListList(19).First();
                    commands.AddRange(Enumerable.Repeat((long)0, 99999));

                    bool hadX = false;
                    long relativeBase = 0;
                    for (long offset = 0; commands[(int)offset] != 99;)
                    {
                        string instruction = commands[(int)offset].ToString().PadLeft(5, '0');
                        char firstMode = instruction[2];
                        char secondMode = instruction[1];
                        char thirdMode = instruction[0];
                        int opcode = int.Parse(instruction[4].ToString());

                        switch (opcode)
                        {
                            case 1:
                                if (thirdMode == '0')
                                    commands[(int)commands[(int)(offset + 3)]] = GetParameter(commands, offset + 1, firstMode, relativeBase) + GetParameter(commands, offset + 2, secondMode, relativeBase);
                                else if (thirdMode == '2')
                                    commands[(int)commands[(int)(offset + 3)] + (int)relativeBase] = GetParameter(commands, offset + 1, firstMode, relativeBase) + GetParameter(commands, offset + 2, secondMode, relativeBase);
                                offset += 4;
                                break;
                            case 2:
                                if (thirdMode == '0')
                                    commands[(int)commands[(int)(offset + 3)]] = GetParameter(commands, offset + 1, firstMode, relativeBase) * GetParameter(commands, offset + 2, secondMode, relativeBase);
                                else if (thirdMode == '2')
                                    commands[(int)commands[(int)(offset + 3)] + (int)relativeBase] = GetParameter(commands, offset + 1, firstMode, relativeBase) * GetParameter(commands, offset + 2, secondMode, relativeBase);
                                offset += 4;
                                break;
                            case 3:
                                long input = x;
                                if (hadX) { input = y; hadX = false; }
                                else hadX = true;
                                if (firstMode == '0')
                                    commands[(int)commands[(int)(offset + 1)]] = input;
                                else if (firstMode == '2')
                                    commands[(int)commands[(int)(offset + 1)] + (int)relativeBase] = input;
                                offset += 2;
                                break;
                            case 4:
                                long output = GetParameter(commands, offset + 1, firstMode, relativeBase);
                                if (output == 0)
                                    map.Add(new Point(x, y, '.'));
                                else if (output == 1)
                                    map.Add(new Point(x, y, '#'));
                                offset += 2;
                                break;
                            case 5:
                                offset = GetParameter(commands, offset + 1, firstMode, relativeBase) != 0 ? GetParameter(commands, offset + 2, secondMode, relativeBase) : offset + 3;
                                break;
                            case 6:
                                offset = GetParameter(commands, offset + 1, firstMode, relativeBase) == 0 ? GetParameter(commands, offset + 2, secondMode, relativeBase) : offset + 3;
                                break;
                            case 7:
                                if (thirdMode == '0')
                                    commands[(int)commands[(int)(offset + 3)]] = GetParameter(commands, offset + 1, firstMode, relativeBase) < GetParameter(commands, offset + 2, secondMode, relativeBase) ? 1 : 0;
                                else if (thirdMode == '2')
                                    commands[(int)commands[(int)(offset + 3)] + (int)relativeBase] = GetParameter(commands, offset + 1, firstMode, relativeBase) < GetParameter(commands, offset + 2, secondMode, relativeBase) ? 1 : 0;
                                offset += 4;
                                break;
                            case 8:
                                if (thirdMode == '0')
                                    commands[(int)commands[(int)(offset + 3)]] = GetParameter(commands, offset + 1, firstMode, relativeBase) == GetParameter(commands, offset + 2, secondMode, relativeBase) ? 1 : 0;
                                else if (thirdMode == '2')
                                    commands[(int)commands[(int)(offset + 3)] + (int)relativeBase] = GetParameter(commands, offset + 1, firstMode, relativeBase) == GetParameter(commands, offset + 2, secondMode, relativeBase) ? 1 : 0;
                                offset += 4;
                                break;
                            case 9:
                                relativeBase += GetParameter(commands, offset + 1, firstMode, relativeBase);
                                offset += 2;
                                break;
                        }
                    }
                }
            }

            for (int y = 0; y < map.Max(t => t.y); y++)
            {
                for (int x = 0; x < map.Max(t => t.x); x++)
                {
                    if (map.Contains(new Point(x, y, '?')))
                        Console.Write(map.First(t => t.Equals(new Point(x, y, '?'))).tile);
                    else
                        Console.Write("?");
                }
                Console.WriteLine();
            }

            return map.Count(p => p.tile == '#').ToString();
        }

        public static long GetParameter(List<long> commands, long position, char mode, long relativeBase)
        {
            if (mode == '0')
            {
                if ((int)commands[(int)position] < 0) position = 0;
                else position = commands[(int)position];
                return commands[(int)position];
            }
            else if (mode == '1')
            {
                return commands[(int)position];
            }
            else if (mode == '2')
            {
                if (position < 0) position = 0;
                if ((int)commands[(int)position] + (int)relativeBase < 0) position = 0;
                else position = commands[(int)position] + (int)relativeBase;
                return commands[(int)position];
            }

            throw new Exception("Oops");
        }

        public class Point
        {
            public long x;
            public long y;
            public char tile;

            public Point(long x, long y, char tile)
            {
                this.x = x;
                this.y = y;
                this.tile = tile;
            }

            public List<Point> GetTopNeighbours()
            {
                return new List<Point>
                {
                    new Point(this.x, this.y - 1, '?'),
                    new Point(this.x + 1, this.y, '?')
                };
            }

            public List<Point> GetBottomNeighbours()
            {
                return new List<Point>
                {
                    new Point(this.x - 1, this.y, '?'),
                    new Point(this.x, this.y + 1, '?')
                };
            }

            public Point Clone()
            {
                return new Point(this.x, this.y, this.tile);
            }

            public override int GetHashCode()
            {
                return (this.x.ToString() + "." + this.y.ToString()).GetHashCode();
            }

            public override bool Equals(object obj)
            {
                return obj is Point other && (other.x == this.x && other.y == this.y);
            }
        }

        public static string Solve2()
        {
            List<Point> map = new List<Point>();
            List<Point> topEdges = new List<Point>();
            List<Point> bottomEdges = new List<Point>();
            List<Point> additions = new List<Point>();
            bool notfound = true;
            int max = 4020;
            int minX = 4000;
            int minY = 4000;
            while (notfound)
            {
                additions.Clear();
                for (int y = minY; y < max; y++)
                {
                    for (int x = minX; x < max; x++)
                    {
                        if (map.Contains(new Point(x, y, '.')))
                            continue;

                        List<long> commands = GetInputAsCsFloatListList(19).First();
                        commands.AddRange(Enumerable.Repeat((long)0, 99999));

                        bool hadX = false;
                        long relativeBase = 0;
                        for (long offset = 0; commands[(int)offset] != 99;)
                        {
                            string instruction = commands[(int)offset].ToString().PadLeft(5, '0');
                            char firstMode = instruction[2];
                            char secondMode = instruction[1];
                            char thirdMode = instruction[0];
                            int opcode = int.Parse(instruction[4].ToString());

                            switch (opcode)
                            {
                                case 1:
                                    if (thirdMode == '0')
                                        commands[(int)commands[(int)(offset + 3)]] = GetParameter(commands, offset + 1, firstMode, relativeBase) + GetParameter(commands, offset + 2, secondMode, relativeBase);
                                    else if (thirdMode == '2')
                                        commands[(int)commands[(int)(offset + 3)] + (int)relativeBase] = GetParameter(commands, offset + 1, firstMode, relativeBase) + GetParameter(commands, offset + 2, secondMode, relativeBase);
                                    offset += 4;
                                    break;
                                case 2:
                                    if (thirdMode == '0')
                                        commands[(int)commands[(int)(offset + 3)]] = GetParameter(commands, offset + 1, firstMode, relativeBase) * GetParameter(commands, offset + 2, secondMode, relativeBase);
                                    else if (thirdMode == '2')
                                        commands[(int)commands[(int)(offset + 3)] + (int)relativeBase] = GetParameter(commands, offset + 1, firstMode, relativeBase) * GetParameter(commands, offset + 2, secondMode, relativeBase);
                                    offset += 4;
                                    break;
                                case 3:
                                    long input = x;
                                    if (hadX) { input = y; hadX = false; }
                                    else hadX = true;
                                    if (firstMode == '0')
                                        commands[(int)commands[(int)(offset + 1)]] = input;
                                    else if (firstMode == '2')
                                        commands[(int)commands[(int)(offset + 1)] + (int)relativeBase] = input;
                                    offset += 2;
                                    break;
                                case 4:
                                    long output = GetParameter(commands, offset + 1, firstMode, relativeBase);
                                    Point p = null;
                                    if (output == 0)
                                        p = new Point(x, y, '.');
                                    else if (output == 1)
                                        p = new Point(x, y, '#');
                                    map.Add(p);
                                    additions.Add(p);
                                    offset += 2;
                                    break;
                                case 5:
                                    offset = GetParameter(commands, offset + 1, firstMode, relativeBase) != 0 ? GetParameter(commands, offset + 2, secondMode, relativeBase) : offset + 3;
                                    break;
                                case 6:
                                    offset = GetParameter(commands, offset + 1, firstMode, relativeBase) == 0 ? GetParameter(commands, offset + 2, secondMode, relativeBase) : offset + 3;
                                    break;
                                case 7:
                                    if (thirdMode == '0')
                                        commands[(int)commands[(int)(offset + 3)]] = GetParameter(commands, offset + 1, firstMode, relativeBase) < GetParameter(commands, offset + 2, secondMode, relativeBase) ? 1 : 0;
                                    else if (thirdMode == '2')
                                        commands[(int)commands[(int)(offset + 3)] + (int)relativeBase] = GetParameter(commands, offset + 1, firstMode, relativeBase) < GetParameter(commands, offset + 2, secondMode, relativeBase) ? 1 : 0;
                                    offset += 4;
                                    break;
                                case 8:
                                    if (thirdMode == '0')
                                        commands[(int)commands[(int)(offset + 3)]] = GetParameter(commands, offset + 1, firstMode, relativeBase) == GetParameter(commands, offset + 2, secondMode, relativeBase) ? 1 : 0;
                                    else if (thirdMode == '2')
                                        commands[(int)commands[(int)(offset + 3)] + (int)relativeBase] = GetParameter(commands, offset + 1, firstMode, relativeBase) == GetParameter(commands, offset + 2, secondMode, relativeBase) ? 1 : 0;
                                    offset += 4;
                                    break;
                                case 9:
                                    relativeBase += GetParameter(commands, offset + 1, firstMode, relativeBase);
                                    offset += 2;
                                    break;
                            }
                        }
                    }
                }

                //check square
                List<Point> newTopEdges = additions.Where(a => a.tile == '#' && a.GetTopNeighbours().Where(n => map.Exists(k => k.Equals(n))).Any(n => map.First(k => k.Equals(n)).tile == '.')).ToList();
                List<Point> newBottomEdges = additions.Where(a => a.tile == '#' && a.GetBottomNeighbours().Where(n => map.Exists(k => k.Equals(n))).Any(n => map.First(k => k.Equals(n)).tile == '.')).ToList();
                topEdges.AddRange(newTopEdges);

                newBottomEdges.ForEach(a =>
                {
                    if (topEdges.Any(p => p.x == a.x + 100 && p.y == a.y - 100))
                    {
                        notfound = false;
                    }
                });

                if (newTopEdges.Count > 0)
                    minY = (int)newTopEdges.First().y - 5;
                if (newBottomEdges.Count > 0)
                    minX = (int)newBottomEdges.First().x - 5;
                bottomEdges.AddRange(newBottomEdges);
                max++;
            }

            for (int y = 0; y < map.Max(t => t.y); y++)
            {
                for (int x = 0; x < map.Max(t => t.x); x++)
                {
                    if (map.Contains(new Point(x, y, '?')))
                        Console.Write(map.First(t => t.Equals(new Point(x, y, '?'))).tile);
                    else
                        Console.Write("?");
                }
                Console.WriteLine();
            }

            return map.Count(p => p.tile == '#').ToString();
        }
    }
}
