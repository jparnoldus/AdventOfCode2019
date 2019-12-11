using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode2019.challenge
{
    class Day07 : Challenge
    {
        public static string Solve1()
        {
            List<int> outputs = new List<int>();
            Enumerable.Range(11111, 44444).ToList().Select(i => i.ToString()).Where(i => i.Distinct().Count() == 5).ToList().ForEach(i =>
            {
                Amplifier A = new Amplifier(int.Parse(i[0].ToString()), GetInputAsCsIntListList(7).First());
                Amplifier B = new Amplifier(int.Parse(i[1].ToString()), GetInputAsCsIntListList(7).First());
                Amplifier C = new Amplifier(int.Parse(i[2].ToString()), GetInputAsCsIntListList(7).First());
                Amplifier D = new Amplifier(int.Parse(i[3].ToString()), GetInputAsCsIntListList(7).First());
                Amplifier E = new Amplifier(int.Parse(i[4].ToString()), GetInputAsCsIntListList(7).First());

                A.next = B;
                B.next = C;
                C.next = D;
                D.next = E;

                int output = 0;
                Amplifier pointer = A;
                while (pointer != E)
                {
                    output = pointer.Continue(output);
                    pointer = pointer.next;
                }

                outputs.Add(pointer.Continue(output));
            });

            return outputs.Max().ToString();
        }

        public static string Solve2()
        {
            List<int> outputs = new List<int>();
            Enumerable.Range(55555, 44444).ToList().Select(i => i.ToString()).Where(i => i.Distinct().Count() == 5).ToList().ForEach(i =>
            {
                Amplifier A = new Amplifier(int.Parse(i[0].ToString()), GetInputAsCsIntListList(7).First());
                Amplifier B = new Amplifier(int.Parse(i[1].ToString()), GetInputAsCsIntListList(7).First());
                Amplifier C = new Amplifier(int.Parse(i[2].ToString()), GetInputAsCsIntListList(7).First());
                Amplifier D = new Amplifier(int.Parse(i[3].ToString()), GetInputAsCsIntListList(7).First());
                Amplifier E = new Amplifier(int.Parse(i[4].ToString()), GetInputAsCsIntListList(7).First());

                A.next = B;
                B.next = C;
                C.next = D;
                D.next = E;
                E.next = A;

                int output = 0;
                Amplifier pointer = A;
                while (!pointer.halted) {
                    output = pointer.Continue(output);
                    pointer = pointer.next;
                }

                outputs.Add(output);
            });

            return outputs.Max().ToString();
        }

        public class Amplifier
        {
            private bool phaseUsed;
            public int phase;
            private int offset;
            private List<int> state;
            public Amplifier next;
            public bool halted;

            public Amplifier(int phase, List<int> state)
            {
                this.phaseUsed = false;
                this.phase = phase;
                this.state = state;
            }


            public int Continue(int input) 
            {
                int output = 0;
                bool running = true;
                while (running)
                {
                    if (this.state[this.offset] == 99)
                    {
                        output = input;
                        this.halted = true;
                        running = false;
                    }

                    string instruction = this.state[this.offset].ToString().PadLeft(5, '0');
                    char firstMode = instruction[2];
                    char secondMode = instruction[1];
                    int opcode = int.Parse(instruction[4].ToString());

                    switch (opcode)
                    {
                        case 1:
                            if (firstMode == '0' && secondMode == '0')
                                this.state[this.state[this.offset + 3]] = this.state[this.state[this.offset + 1]] + this.state[this.state[this.offset + 2]];
                            else if (firstMode == '1' && secondMode == '0')
                                this.state[this.state[this.offset + 3]] = this.state[this.offset + 1] + this.state[this.state[this.offset + 2]];
                            else if (firstMode == '0' && secondMode == '1')
                                this.state[this.state[this.offset + 3]] = this.state[this.state[this.offset + 1]] + this.state[this.offset + 2];
                            else if (firstMode == '1' && secondMode == '1')
                                this.state[this.state[this.offset + 3]] = this.state[this.offset + 1] + this.state[this.offset + 2];
                            this.offset += 4;
                            break;
                        case 2:
                            if (firstMode == '0' && secondMode == '0')
                                this.state[this.state[this.offset + 3]] = this.state[this.state[this.offset + 1]] * this.state[this.state[this.offset + 2]];
                            else if (firstMode == '1' && secondMode == '0')
                                this.state[this.state[this.offset + 3]] = this.state[this.offset + 1] * this.state[this.state[this.offset + 2]];
                            else if (firstMode == '0' && secondMode == '1')
                                this.state[this.state[this.offset + 3]] = this.state[this.state[this.offset + 1]] * this.state[this.offset + 2];
                            else if (firstMode == '1' && secondMode == '1')
                                this.state[this.state[this.offset + 3]] = this.state[this.offset + 1] * this.state[this.offset + 2];
                            this.offset += 4;
                            break;
                        case 3:
                            if (phaseUsed)
                            {
                                this.state[this.state[this.offset + 1]] = input;
                            }
                            else 
                            {
                                this.state[this.state[this.offset + 1]] = this.phase;
                                this.phaseUsed = true;
                            }
                            this.offset += 2;
                            break;
                        case 4:
                            this.offset += 2;
                            output = this.state[this.state[this.offset - 1]];
                            running = false;
                            break;
                        case 5:
                            if (firstMode == '0' && secondMode == '0')
                                this.offset = this.state[this.state[this.offset + 1]] != 0 ? this.state[this.state[this.offset + 2]] : this.offset + 3;
                            else if (firstMode == '1' && secondMode == '0')
                                this.offset = this.state[this.offset + 1] != 0 ? this.state[this.state[this.offset + 2]] : this.offset + 3;
                            else if (firstMode == '0' && secondMode == '1')
                                this.offset = this.state[this.state[this.offset + 1]] != 0 ? this.state[this.offset + 2] : this.offset + 3;
                            else if (firstMode == '1' && secondMode == '1')
                                this.offset = this.state[this.offset + 1] != 0 ? this.state[this.offset + 2] : this.offset + 3;
                            break;
                        case 6:
                            if (firstMode == '0' && secondMode == '0')
                                this.offset = this.state[this.state[this.offset + 1]] == 0 ? this.state[this.state[this.offset + 2]] : this.offset + 3;
                            else if (firstMode == '1' && secondMode == '0')
                                this.offset = this.state[this.offset + 1] == 0 ? this.state[this.state[this.offset + 2]] : this.offset + 3;
                            else if (firstMode == '0' && secondMode == '1')
                                this.offset = this.state[this.state[this.offset + 1]] == 0 ? this.state[this.offset + 2] : this.offset + 3;
                            else if (firstMode == '1' && secondMode == '1')
                                this.offset = this.state[this.offset + 1] == 0 ? this.state[this.offset + 2] : this.offset + 3;
                            break;
                        case 7:
                            if (firstMode == '0' && secondMode == '0')
                                this.state[this.state[this.offset + 3]] = this.state[this.state[this.offset + 1]] < this.state[this.state[this.offset + 2]] ? 1 : 0;
                            else if (firstMode == '1' && secondMode == '0')
                                this.state[this.state[this.offset + 3]] = this.state[this.offset + 1] < this.state[this.state[this.offset + 2]] ? 1 : 0;
                            else if (firstMode == '0' && secondMode == '1')
                                this.state[this.state[this.offset + 3]] = this.state[this.state[this.offset + 1]] < this.state[this.offset + 2] ? 1 : 0;
                            else if (firstMode == '1' && secondMode == '1')
                                this.state[this.state[this.offset + 3]] = this.state[this.offset + 1] < this.state[this.offset + 2] ? 1 : 0;
                            this.offset += 4;
                            break;
                        case 8:
                            if (firstMode == '0' && secondMode == '0')
                                this.state[this.state[this.offset + 3]] = this.state[this.state[this.offset + 1]] == this.state[this.state[this.offset + 2]] ? 1 : 0;
                            else if (firstMode == '1' && secondMode == '0')
                                this.state[this.state[this.offset + 3]] = this.state[this.offset + 1] == this.state[this.state[this.offset + 2]] ? 1 : 0;
                            else if (firstMode == '0' && secondMode == '1')
                                this.state[this.state[this.offset + 3]] = this.state[this.state[this.offset + 1]] == this.state[this.offset + 2] ? 1 : 0;
                            else if (firstMode == '1' && secondMode == '1')
                                this.state[this.state[this.offset + 3]] = this.state[this.offset + 1] == this.state[this.offset + 2] ? 1 : 0;
                            this.offset += 4;
                            break;
                    }
                }

                return output;
            }
        }
    }
}
