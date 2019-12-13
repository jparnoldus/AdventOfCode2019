using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode2019.challenge
{
    class Day13: Challenge
    {
        public static string Solve1()
        {
            List<long> commands = GetInputAsCsFloatListList(13).First();
            commands.AddRange(Enumerable.Repeat((long)0, 99999));
            List<Point> map = BuildMap(commands);

            for (int y = 0; y < map.Max(t => t.y); y++)
            {
                for (int x = 0; x < map.Max(t => t.x) + 1; x++) 
                {
                    Console.Write(map.First(t => t.Equals(new Point(x, y, 0))).tileId);
                }
                Console.WriteLine();
            }

            return map.Count(t => t.tileId == 2).ToString();
        }

        private static List<Point> BuildMap(List<long> commands) 
        {
            List<long> output = new List<long>();
            long input = 0;
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
                        output.Add(GetParameter(commands, offset + 1, firstMode, relativeBase));
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

            List<Point> map = new List<Point>();
            for (int i = 0; i < output.Count; i += 3)
            {
                map.Add(new Point(output[i], output[i + 1], output[i + 2]));
            }

            return map;
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
            public long tileId;

            public Point(long x, long y, long tileId)
            {
                this.x = x;
                this.y = y;
                this.tileId = tileId;
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
            List<long> commands = GetInputAsCsFloatListList(13).First();
            commands.AddRange(Enumerable.Repeat((long)0, 99999));
            commands[0] = 2;
            List<long> output = new List<long>();
            List<long> temp = new List<long>();
            long input = 0;
            long relativeBase = 0;
            bool mapBuilt = false;
            List<Point> map = new List<Point>();
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
                        if (!mapBuilt)
                        {
                            mapBuilt = true;
                            for (int i = 0; i < output.Count; i += 3)
                            {
                                map.Add(new Point(output[i], output[i + 1], output[i + 2]));
                            }
                        }

                        for (int y = 0; y < map.Max(t => t.y); y++)
                        {
                            for (int x = 0; x < map.Max(t => t.x) + 1; x++)
                            {
                                Console.Write(map.First(t => t.Equals(new Point(x, y, 0))).tileId);
                            }
                            Console.WriteLine();
                        }
                        Console.WriteLine(map.First(p => p.Equals(new Point(-1, 0, 0))).tileId);

                        string line = Console.ReadLine();
                        if (line != "")
                            input = int.Parse(Console.ReadLine());
                        else
                            input = 0;
                        if (firstMode == '0')
                            commands[(int)commands[(int)(offset + 1)]] = input;
                        else if (firstMode == '2')
                            commands[(int)commands[(int)(offset + 1)] + (int)relativeBase] = input;
                        offset += 2;
                        Console.WriteLine("------------------------------");
                        break;
                    case 4:
                        if (!mapBuilt)
                        {
                            output.Add(GetParameter(commands, offset + 1, firstMode, relativeBase));
                        }
                        else 
                        {
                            temp.Add(GetParameter(commands, offset + 1, firstMode, relativeBase));
                            if (temp.Count == 3) 
                            {
                                map.First(p => p.Equals(new Point(temp[0], temp[1], 0))).tileId = temp[2];
                                temp.Clear();
                            }
                        }
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

            return "";
        }
    }
}
