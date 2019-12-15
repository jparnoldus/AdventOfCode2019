using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode2019.challenge
{
    class Day15 : Challenge
    {
        public static string Solve1()
        {
            Point current = new Point(0, 0, '.');
            List<Point> map = new List<Point> { current };
            Dictionary<Point, long> path = new Dictionary<Point, long> { { current, 0 } };

            List<long> commands = GetInputAsCsFloatListList(15).First();
            commands.AddRange(Enumerable.Repeat((long)0, 99999));

            long input = 1; // Start going north
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
                        // Add output of what robot sees to map
                        long output = GetParameter(commands, offset + 1, firstMode, relativeBase);
                        Point next = null;
                        switch (input)
                        {
                            case 1:
                                next = new Point(current.x, current.y - 1, '?');
                                break;
                            case 2:
                                next = new Point(current.x, current.y + 1, '?');
                                break;
                            case 3:
                                next = new Point(current.x - 1, current.y, '?');
                                break;
                            case 4:
                                next = new Point(current.x + 1, current.y, '?');
                                break;
                        }
                        switch (output) {
                            case 0:
                                // Register wall on the map
                                next.tile = '#';
                                map.Add(next);
                                break;
                            case 1:
                                // Register move
                                current = next;
                                current.tile = '.';
                                if (!path.ContainsKey(current))
                                {
                                    path.Add(current, input);
                                    map.Add(current);
                                }
                                else
                                {
                                    path.Remove(path.Last().Key);
                                    current = path.Last().Key;
                                }
                                break;
                            case 2:
                                return path.Count.ToString();
                        }
                        List<Point> potential = current.GetNeighbours().Except(path.Keys).Except(map).ToList();
                        if (potential.Count > 0)
                        {
                            // Move potential direction
                            input = long.Parse(potential.First().tile.ToString());
                        }
                        else
                        {
                            // Go back on path
                            switch (path.Last().Value)
                            {
                                case 1:
                                    input = 2;
                                    break;
                                case 2:
                                    input = 1;
                                    break;
                                case 3:
                                    input = 4;
                                    break;
                                case 4:
                                    input = 3;
                                    break;
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
                    new Point(this.x - 1, this.y, '3'),
                    new Point(this.x, this.y - 1, '1'),
                    new Point(this.x + 1, this.y, '4'),
                    new Point(this.x, this.y + 1, '2')
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
            Point current = new Point(0, 0, '.');
            List<Point> map = new List<Point> { current };
            Dictionary<Point, long> path = new Dictionary<Point, long> { { current, 0 } };

            List<long> commands = GetInputAsCsFloatListList(15).First();
            commands.AddRange(Enumerable.Repeat((long)0, 99999));

            long input = 1; // Start going north
            long relativeBase = 0;
            long offset = 0;
            bool filled = false;
            while (!filled)
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
                        // Add output of what robot sees to map
                        long output = GetParameter(commands, offset + 1, firstMode, relativeBase);
                        Point next = null;
                        switch (input)
                        {
                            case 1:
                                next = new Point(current.x, current.y - 1, '?');
                                break;
                            case 2:
                                next = new Point(current.x, current.y + 1, '?');
                                break;
                            case 3:
                                next = new Point(current.x - 1, current.y, '?');
                                break;
                            case 4:
                                next = new Point(current.x + 1, current.y, '?');
                                break;
                        }
                        switch (output)
                        {
                            case 0:
                                // Register wall on the map
                                next.tile = '#';
                                map.Add(next);
                                break;
                            case 1:
                                // Register move
                                current = next;
                                current.tile = '.';
                                if (!path.ContainsKey(current))
                                {
                                    path.Add(current, input);
                                    map.Add(current);
                                }
                                else
                                {
                                    path.Remove(path.Last().Key);
                                    current = path.Last().Key;
                                }
                                break;
                            case 2:
                                // Register move and location
                                current = next;
                                current.tile = 'O';
                                if (!path.ContainsKey(current))
                                {
                                    path.Add(current, input);
                                    map.Add(current);
                                }
                                else
                                {
                                    path.Remove(path.Last().Key);
                                    current = path.Last().Key;
                                }
                                break;
                        }
                        List<Point> potential = current.GetNeighbours().Except(path.Keys).Except(map).ToList();
                        if (potential.Count > 0)
                        {
                            // Move potential direction
                            input = long.Parse(potential.First().tile.ToString());
                        }
                        else
                        {
                            filled = current.Equals(new Point(0, 0, 'S'));

                            // Go back on path
                            switch (path.Last().Value)
                            {
                                case 1:
                                    input = 2;
                                    break;
                                case 2:
                                    input = 1;
                                    break;
                                case 3:
                                    input = 4;
                                    break;
                                case 4:
                                    input = 3;
                                    break;
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

            int minutes = 0;
            while (map.Any(p => p.tile == '.'))
            {
                minutes++;
                map.Where(p => p.tile == '.' && p.GetNeighbours().Any(n => map.First(i => i.Equals(n)).tile == 'O')).ToList().ForEach(p => p.tile = 'O');
            }

            return minutes.ToString();
        }
    }
}
