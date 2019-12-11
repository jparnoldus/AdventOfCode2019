using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode2019.challenge
{
    class Day09 : Challenge
    {
        public static string Solve1()
        {
            List<long> commands = GetInputAsCsFloatListList(9).First();
            commands.AddRange(Enumerable.Repeat((long)0, 99999));

            long input = 1;
            long output = 1337;
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
                        output = GetParameter(commands, offset + 1, firstMode, relativeBase);
                        Console.WriteLine(output);
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

            return output.ToString();
        }

        public static long GetParameter(List<long> commands, long position, char mode, long relativeBase) {
            if (mode == '0') {
                if ((int)commands[(int)position] < 0) position = 0;
                else position = commands[(int)position];
                return commands[(int)position];
            } else if (mode == '1') {
                return commands[(int)position];
            } else if (mode == '2') {
                if (position < 0) position = 0;
                if ((int)commands[(int)position] + (int)relativeBase < 0) position = 0;
                else position = commands[(int)position] + (int)relativeBase;
                return commands[(int)position];
            }

            throw new Exception("Oops");
        }

        public static string Solve2()
        {
            List<long> commands = GetInputAsCsFloatListList(9).First();
            commands.AddRange(Enumerable.Repeat((long)0, 99999));

            long input = 2;
            long output = 1337;
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
                        output = GetParameter(commands, offset + 1, firstMode, relativeBase);
                        Console.WriteLine(output);
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

            return output.ToString();
        }
    }
}
