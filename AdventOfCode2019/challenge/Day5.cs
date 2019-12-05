using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode2019.challenge
{
    class Day5 : Challenge
    {
        public static string Solve1()
        {
            List<int> commands = GetInputAsCsIntListList(5).First();

            int input = 1;
            int output = 1337;
            for (int offset = 0; commands[offset] != 99;)
            {
                string instruction = commands[offset].ToString().PadLeft(5, '0');
                char firstMode = instruction[2];
                char secondMode = instruction[1];
                //char thirdMode = instruction[0];
                int opcode = int.Parse(instruction[4].ToString());

                switch (opcode) {
                    case 1:
                        if (firstMode == '0' && secondMode == '0')
                            commands[commands[offset + 3]] = commands[commands[offset + 1]] + commands[commands[offset + 2]];
                        else if (firstMode == '1' && secondMode == '0')
                            commands[commands[offset + 3]] = commands[offset + 1] + commands[commands[offset + 2]];
                        else if (firstMode == '0' && secondMode == '1')
                            commands[commands[offset + 3]] = commands[commands[offset + 1]] + commands[offset + 2];
                        else if (firstMode == '1' && secondMode == '1')
                            commands[commands[offset + 3]] = commands[offset + 1] + commands[offset + 2];
                        offset += 4;
                        break;
                    case 2:
                        if (firstMode == '0' && secondMode == '0')
                            commands[commands[offset + 3]] = commands[commands[offset + 1]] * commands[commands[offset + 2]];
                        else if (firstMode == '1' && secondMode == '0')
                            commands[commands[offset + 3]] = commands[offset + 1] * commands[commands[offset + 2]];
                        else if (firstMode == '0' && secondMode == '1')
                            commands[commands[offset + 3]] = commands[commands[offset + 1]] * commands[offset + 2];
                        else if (firstMode == '1' && secondMode == '1')
                            commands[commands[offset + 3]] = commands[offset + 1] * commands[offset + 2];
                        offset += 4;
                        break;
                    case 3:
                        commands[commands[offset + 1]] = input;
                        offset += 2;
                        break;
                    case 4:
                        output = commands[commands[offset + 1]];
                        offset += 2;
                        break;
                }
            }

            return output.ToString();
        }

        public static string Solve2()
        {
            List<int> commands = GetInputAsCsIntListList(5).First();

            int input = 5;
            int output = 1337;
            for (int offset = 0; commands[offset] != 99;)
            {
                string instruction = commands[offset].ToString().PadLeft(5, '0');
                char firstMode = instruction[2];
                char secondMode = instruction[1];
                //char thirdMode = instruction[0];
                int opcode = int.Parse(instruction[4].ToString());

                switch (opcode)
                {
                    case 1:
                        if (firstMode == '0' && secondMode == '0')
                            commands[commands[offset + 3]] = commands[commands[offset + 1]] + commands[commands[offset + 2]];
                        else if (firstMode == '1' && secondMode == '0')
                            commands[commands[offset + 3]] = commands[offset + 1] + commands[commands[offset + 2]];
                        else if (firstMode == '0' && secondMode == '1')
                            commands[commands[offset + 3]] = commands[commands[offset + 1]] + commands[offset + 2];
                        else if (firstMode == '1' && secondMode == '1')
                            commands[commands[offset + 3]] = commands[offset + 1] + commands[offset + 2];
                        offset += 4;
                        break;
                    case 2:
                        if (firstMode == '0' && secondMode == '0')
                            commands[commands[offset + 3]] = commands[commands[offset + 1]] * commands[commands[offset + 2]];
                        else if (firstMode == '1' && secondMode == '0')
                            commands[commands[offset + 3]] = commands[offset + 1] * commands[commands[offset + 2]];
                        else if (firstMode == '0' && secondMode == '1')
                            commands[commands[offset + 3]] = commands[commands[offset + 1]] * commands[offset + 2];
                        else if (firstMode == '1' && secondMode == '1')
                            commands[commands[offset + 3]] = commands[offset + 1] * commands[offset + 2];
                        offset += 4;
                        break;
                    case 3:
                        commands[commands[offset + 1]] = input;
                        offset += 2;
                        break;
                    case 4:
                        output = commands[commands[offset + 1]];
                        offset += 2;
                        break;
                    case 5:
                        if (firstMode == '0' && secondMode == '0')
                            offset = commands[commands[offset + 1]] != 0 ? commands[commands[offset + 2]] : offset + 3;
                        else if (firstMode == '1' && secondMode == '0')
                            offset = commands[offset + 1] != 0 ? commands[commands[offset + 2]] : offset + 3;
                        else if (firstMode == '0' && secondMode == '1')
                            offset = commands[commands[offset + 1]] != 0 ? commands[offset + 2] : offset + 3;
                        else if (firstMode == '1' && secondMode == '1')
                            offset = commands[offset + 1] != 0 ? commands[offset + 2] : offset + 3;
                        break;
                    case 6:
                        if (firstMode == '0' && secondMode == '0')
                            offset = commands[commands[offset + 1]] == 0 ? commands[commands[offset + 2]] : offset + 3;
                        else if (firstMode == '1' && secondMode == '0')
                            offset = commands[offset + 1] == 0 ? commands[commands[offset + 2]] : offset + 3;
                        else if (firstMode == '0' && secondMode == '1')
                            offset = commands[commands[offset + 1]] == 0 ? commands[offset + 2] : offset + 3;
                        else if (firstMode == '1' && secondMode == '1')
                            offset = commands[offset + 1] == 0 ? commands[offset + 2] : offset + 3;
                        break;
                    case 7:
                        if (firstMode == '0' && secondMode == '0')
                            commands[commands[offset + 3]] = commands[commands[offset + 1]] < commands[commands[offset + 2]] ? 1 : 0;
                        else if (firstMode == '1' && secondMode == '0')
                            commands[commands[offset + 3]] = commands[offset + 1] < commands[commands[offset + 2]] ? 1 : 0;
                        else if (firstMode == '0' && secondMode == '1')
                            commands[commands[offset + 3]] = commands[commands[offset + 1]] < commands[offset + 2] ? 1 : 0;
                        else if (firstMode == '1' && secondMode == '1')
                            commands[commands[offset + 3]] = commands[offset + 1] < commands[offset + 2] ? 1 : 0;
                        offset += 4;
                        break;
                    case 8:
                        if (firstMode == '0' && secondMode == '0')
                            commands[commands[offset + 3]] = commands[commands[offset + 1]] == commands[commands[offset + 2]] ? 1 : 0;
                        else if (firstMode == '1' && secondMode == '0')
                            commands[commands[offset + 3]] = commands[offset + 1] == commands[commands[offset + 2]] ? 1 : 0;
                        else if (firstMode == '0' && secondMode == '1')
                            commands[commands[offset + 3]] = commands[commands[offset + 1]] == commands[offset + 2] ? 1 : 0;
                        else if (firstMode == '1' && secondMode == '1')
                            commands[commands[offset + 3]] = commands[offset + 1] == commands[offset + 2] ? 1 : 0;
                        offset += 4;
                        break;
                }
            }

            return output.ToString();
        }
    }
}