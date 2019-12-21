using System;
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
                if (o == 35 || o == 46)
                    map.Add(new Point(x, y, (char)o));
                else if (o == 10)
                {
                    y++;
                    x = -1;
                }
                else
                    map.Add(new Point(x, y, (char)o));
                x++;
            });

            // Get full path
            List<string> path = new List<string>();
            int counter = 0;
            List<Point> visited = new List<Point>();
            List<char> instructions = new List<char>();
            Point current = map.First(p => p.tile != '#' && p.tile != '.');

            visited.Add(current);
            bool ended = false;
            while (!ended)
            {
                List<Point> possibilities = current.GetNeighbours().Except(visited.TakeLast(2)).Where(p => map.Contains(p)).Select(p => map.Find(n => n.Equals(p))).Where(p => p.tile != '.').ToList();
                if (possibilities.Count == 1)
                {
                    char direction = current.tile;
                    if ((current.tile == '^' || current.tile == 'v') && possibilities.First().x != current.x)
                    {
                        // making a turn
                        path.Add(counter.ToString());
                        counter = 0;
                        if (current.tile == '^')
                        {
                            if (possibilities.First().x == current.x - 1)
                            {
                                path.Add("L");
                                direction = '<';
                            }
                            else if (possibilities.First().x == current.x + 1)
                            {
                                path.Add("R");
                                direction = '>';
                            }
                        }
                        else if (current.tile == 'v')
                        {
                            if (possibilities.First().x == current.x - 1)
                            {
                                path.Add("R");
                                direction = '<';
                            }
                            else if (possibilities.First().x == current.x + 1)
                            {
                                path.Add("L");
                                direction = '>';
                            }
                        }
                    }
                    else if ((current.tile == '<' || current.tile == '>') && possibilities.First().y != current.y)
                    {
                        // making a turn
                        path.Add(counter.ToString());
                        counter = 0;
                        if (current.tile == '<')
                        {
                            if (possibilities.First().y == current.y - 1)
                            {
                                path.Add("R");
                                direction = '^';
                            }
                            else if (possibilities.First().y == current.y + 1)
                            {
                                path.Add("L");
                                direction = 'v';
                            }
                        }
                        else if (current.tile == '>')
                        {
                            if (possibilities.First().y == current.y - 1)
                            {
                                path.Add("L");
                                direction = '^';
                            }
                            else if (possibilities.First().y == current.y + 1)
                            {
                                path.Add("R");
                                direction = 'v';
                            }
                        }
                    }
                    current = possibilities.First();
                    current.tile = direction;
                }
                else if (possibilities.Count > 1)
                {
                    if (current.tile == '^' || current.tile == 'v')
                    {
                        current = possibilities.First(p => p.x == current.x);
                    }
                    else if (current.tile == '<' || current.tile == '>')
                    {
                        current = possibilities.First(p => p.y == current.y);
                    }
                    current.tile = visited.Last().tile;
                }
                else
                {
                    path.Add(counter.ToString());
                    ended = true;
                }
                
                counter++;
                visited.Add(current);
            }

            path = path.Skip(1).ToList();
            Console.WriteLine(string.Join(',', path));

            List<Pair> pairs = new List<Pair>();
            for (int i = 0; i < path.Count; i += 2)
            {
                pairs.Add(new Pair(path[i].First(), int.Parse(path[i + 1])));
            }

            // Just figured out the sequences with pen and paper
            // ABACABCBCB
            // A. L10 R8 L6 R6 10
            // B. L8 L8 R8 10
            // C. R8 L6 L10 L10 10
            List<int> sequence = new List<int>();
            sequence.AddRange(Encoding.ASCII.GetBytes("A,B,A,C,A,B,C,B,C,B").Select(i => (int)i));
            sequence.Add(10);
            sequence.AddRange(Encoding.ASCII.GetBytes("L,10,R,8,L,6,R,6").Select(i => (int)i));
            sequence.Add(10);
            sequence.AddRange(Encoding.ASCII.GetBytes("L,8,L,8,R,8").Select(i => (int)i));
            sequence.Add(10);
            sequence.AddRange(Encoding.ASCII.GetBytes("R,8,L,6,L,10,L,10").Select(i => (int)i));
            sequence.Add(10);
            sequence.AddRange(Encoding.ASCII.GetBytes("n").Select(i => (int)i));
            sequence.Add(10);

            commands = GetInputAsCsFloatListList(17).First();
            commands.AddRange(Enumerable.Repeat((long)0, 99999));

            commands[0] = 2; // wake up robot
            int inputCounter = 0;
            outputs = new List<long>();
            relativeBase = 0;
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
                            commands[(int)commands[(int)(offset + 1)]] = sequence[inputCounter];
                        else if (firstMode == '2')
                            commands[(int)commands[(int)(offset + 1)] + (int)relativeBase] = sequence[inputCounter];
                        offset += 2;
                        inputCounter++;
                        break;
                    case 4:
                        long temp = GetParameter(commands, offset + 1, firstMode, relativeBase);
                        if (temp <= 255)
                            Console.Write(Convert.ToChar(temp));
                        outputs.Add(temp);
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

            return outputs.Last().ToString();
        }

        public class Pair
        {
            public char turn;
            public int steps;

            public Pair(char turn, int steps)
            {
                this.turn = turn;
                this.steps = steps;
            }
        }
    }
}
