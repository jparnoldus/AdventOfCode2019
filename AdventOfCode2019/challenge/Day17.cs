﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode2019.challenge
{
    class Day17 : Challenge
    {
        public static string Solve1()
        {
            List<long> commands = GetInputAsCsFloatListList(17).First();
            commands.AddRange(Enumerable.Repeat((long)0, 99999));

            long input = 0;
            List<long> outputs = new List<long>();
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
                        if (firstMode == '0')
                            commands[(int)commands[(int)(offset + 1)]] = input;
                        else if (firstMode == '2')
                            commands[(int)commands[(int)(offset + 1)] + (int)relativeBase] = input;
                        offset += 2;
                        break;
                    case 4:
                        outputs.Add(GetParameter(commands, offset + 1, firstMode, relativeBase));
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

            int x = 0,
                y = 0;
            List<Point> map = new List<Point>();
            outputs.ForEach(o => {
                if (o == 35)
                    map.Add(new Point(x, y, '#'));
                else if (o == 46)
                    map.Add(new Point(x, y, '.'));
                else if (o == 10)
                {
                    y++;
                    x = -1;
                }
                x++;
            });

            for (y = 0; y < map.Max(t => t.y); y++)
            {
                for (x = 0; x < map.Max(t => t.x); x++)
                {
                    if (map.Contains(new Point(x, y, '?')))
                        Console.Write(map.First(t => t.Equals(new Point(x, y, '?'))).tile);
                    else
                        Console.Write("?");
                }
                Console.WriteLine();
            }

            return map.Where(p => p.GetNeighbours().TrueForAll(n => map.Contains(n) && map.First(m => m.Equals(n)).tile == '#')).Select(p => p.x * p.y).Sum().ToString();
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

            public List<Point> GetNeighbours()
            {
                return new List<Point>
                {
                    new Point(this.x - 1, this.y, '?'),
                    new Point(this.x, this.y - 1, '?'),
                    new Point(this.x + 1, this.y, '?'),
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
            return "";
        }
    }
}
