using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode2019.challenge
{
    class Day11 : Challenge
    {
        public static string Solve1()
        {
            List<long> commands = GetInputAsCsFloatListList(11).First();
            commands.AddRange(Enumerable.Repeat((long)0, 99999));

            Point current = new Point(0, 0);
            List<Point> path = new List<Point> { current };
            int direction = 0;
            bool o1or2 = true;
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
                            commands[(int)commands[(int)(offset + 1)]] = current.color;
                        else if (firstMode == '2')
                            commands[(int)commands[(int)(offset + 1)] + (int)relativeBase] = current.color;
                        offset += 2;
                        break;
                    case 4:
                        if (o1or2)
                        {
                            long output1 = GetParameter(commands, offset + 1, firstMode, relativeBase);
                            current.color = output1;
                        }
                        else
                        {
                            long output2 = GetParameter(commands, offset + 1, firstMode, relativeBase);
                            Point nextStep = null;
                            switch (output2)
                            {
                                case 0: // left
                                    switch (direction)
                                    {
                                        case 0: //north
                                            direction = 3;
                                            nextStep = new Point(current.x - 1, current.y);
                                            break;
                                        case 1: //east
                                            direction = 0;
                                            nextStep = new Point(current.x, current.y - 1);
                                            break;
                                        case 2: //south
                                            direction = 1;
                                            nextStep = new Point(current.x + 1, current.y);
                                            break;
                                        case 3: //west
                                            direction = 2;
                                            nextStep = new Point(current.x, current.y + 1);
                                            break;
                                    }
                                    break;
                                case 1: // right
                                    switch (direction)
                                    {
                                        case 0:
                                            direction = 1;
                                            nextStep = new Point(current.x + 1, current.y);
                                            break;
                                        case 1:
                                            direction = 2;
                                            nextStep = new Point(current.x, current.y + 1);
                                            break;
                                        case 2:
                                            direction = 3;
                                            nextStep = new Point(current.x - 1, current.y);
                                            break;
                                        case 3:
                                            direction = 0;
                                            nextStep = new Point(current.x, current.y - 1);
                                            break;
                                    }
                                    break;
                            }

                            if (!path.Contains(nextStep))
                            {
                                current = nextStep;
                                path.Add(current);
                            }
                            else 
                            {
                                current = path.First(p => p.Equals(nextStep));
                            }
                        }
                        o1or2 = !o1or2;
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

            return path.Distinct().Count().ToString();
        }

        public static string Solve2()
        {
            List<long> commands = GetInputAsCsFloatListList(11).First();
            commands.AddRange(Enumerable.Repeat((long)0, 99999));

            Point current = new Point(0, 0);
            current.color = 1;
            List<Point> path = new List<Point> { current };
            int direction = 0;
            bool o1or2 = true;
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
                            commands[(int)commands[(int)(offset + 1)]] = current.color;
                        else if (firstMode == '2')
                            commands[(int)commands[(int)(offset + 1)] + (int)relativeBase] = current.color;
                        offset += 2;
                        break;
                    case 4:
                        if (o1or2)
                        {
                            long output1 = GetParameter(commands, offset + 1, firstMode, relativeBase);
                            current.color = output1;
                        }
                        else
                        {
                            long output2 = GetParameter(commands, offset + 1, firstMode, relativeBase);
                            Point nextStep = null;
                            switch (output2)
                            {
                                case 0: // left
                                    switch (direction)
                                    {
                                        case 0: //north
                                            direction = 3;
                                            nextStep = new Point(current.x - 1, current.y);
                                            break;
                                        case 1: //east
                                            direction = 0;
                                            nextStep = new Point(current.x, current.y - 1);
                                            break;
                                        case 2: //south
                                            direction = 1;
                                            nextStep = new Point(current.x + 1, current.y);
                                            break;
                                        case 3: //west
                                            direction = 2;
                                            nextStep = new Point(current.x, current.y + 1);
                                            break;
                                    }
                                    break;
                                case 1: // right
                                    switch (direction)
                                    {
                                        case 0:
                                            direction = 1;
                                            nextStep = new Point(current.x + 1, current.y);
                                            break;
                                        case 1:
                                            direction = 2;
                                            nextStep = new Point(current.x, current.y + 1);
                                            break;
                                        case 2:
                                            direction = 3;
                                            nextStep = new Point(current.x - 1, current.y);
                                            break;
                                        case 3:
                                            direction = 0;
                                            nextStep = new Point(current.x, current.y - 1);
                                            break;
                                    }
                                    break;
                            }

                            if (!path.Contains(nextStep))
                            {
                                current = nextStep;
                                path.Add(current);
                            }
                            else
                            {
                                current = path.First(p => p.Equals(nextStep));
                            }
                        }
                        o1or2 = !o1or2;
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

            for (int y = path.Min(p => p.y) - 1 ; y < path.Max(p => p.y) + 2; y++) {
                for (int x = path.Min(p => p.x) - 1; x < path.Max(p => p.x); x++)
                {
                    if (path.Contains(new Point(x, y)))
                    {
                        if (path.First(p => p.x == x && p.y == y).color == 1)
                        {
                            Console.Write("#");
                        }
                        else
                        {
                            Console.Write("-");
                        }
                    }
                    else {
                        Console.Write("-");
                    }
                }
                Console.WriteLine();
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
            public int x;
            public int y;
            public long color;

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
                this.color = 0;
            }

            public Point Clone()
            {
                return new Point(this.x, this.y);
            }

            public override int GetHashCode()
            {
                return (this.x + this.y).GetHashCode();
            }

            public override bool Equals(object obj)
            {
                Point other = obj as Point;
                return other != null && (other.x == this.x && other.y == this.y);
            }
        }
    }
}
