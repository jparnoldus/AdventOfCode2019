using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode2019.challenge
{
    class Day23 : Challenge
    {
        public static string Solve1()
        {
            Dictionary<long, Queue<Packet>> inputs = new Dictionary<long, Queue<Packet>>();
            List<Computer> network = new List<Computer>();
            for (int i = 0; i < 50; i++)
            {
                network.Add(new Computer(i));
                inputs.Add(i, new Queue<Packet>());
            }

            while (true)
            {
                for (int i = 0; i < 50; i++)
                {
                    List<Packet> results;
                    do
                    {
                        if (inputs[i].Count > 0)
                            results = network[i].Continue(inputs[i].Dequeue());
                        else
                            results = network[i].Continue(null);
                        foreach (Packet result in results)
                        {
                            if (result != null && result.Address < 50)
                                inputs[result.Address].Enqueue(result);
                            else if (result != null && result.Address == 255)
                                return result.Y.ToString();
                        }
                    }
                    while (results.Count > 0 && inputs[i].Count > 0);
                }
            }
        }

        public class Computer
        {
            public long Address;
            private List<long> Commands = new List<long>();
            private long relativeBase = 0;
            private long offset = 0;
            private bool usedAddress = false;
            private bool usedNoPacket = false;

            public Computer(long address)
            {
                this.Address = address;
                this.Commands = GetInputAsCsFloatListList(23).First();
                this.Commands.AddRange(Enumerable.Repeat((long)0, 9999));
            }

            public List<Packet> Continue(Packet input)
            {
                List<Packet> output = new List<Packet>();
                usedNoPacket = false;
                List<long> outputBuffer = new List<long>();
                for (; this.Commands[(int)offset] != 99;)
                {
                    string instruction = this.Commands[(int)offset].ToString().PadLeft(5, '0');
                    char firstMode = instruction[2];
                    char secondMode = instruction[1];
                    char thirdMode = instruction[0];
                    int opcode = int.Parse(instruction[4].ToString());

                    switch (opcode)
                    {
                        case 1:
                            if (thirdMode == '0')
                                this.Commands[(int)this.Commands[(int)(offset + 3)]] = GetParameter(this.Commands, offset + 1, firstMode, relativeBase) + GetParameter(this.Commands, offset + 2, secondMode, relativeBase);
                            else if (thirdMode == '2')
                                this.Commands[(int)this.Commands[(int)(offset + 3)] + (int)relativeBase] = GetParameter(this.Commands, offset + 1, firstMode, relativeBase) + GetParameter(this.Commands, offset + 2, secondMode, relativeBase);
                            offset += 4;
                            break;
                        case 2:
                            if (thirdMode == '0')
                                this.Commands[(int)this.Commands[(int)(offset + 3)]] = GetParameter(this.Commands, offset + 1, firstMode, relativeBase) * GetParameter(this.Commands, offset + 2, secondMode, relativeBase);
                            else if (thirdMode == '2')
                                this.Commands[(int)this.Commands[(int)(offset + 3)] + (int)relativeBase] = GetParameter(this.Commands, offset + 1, firstMode, relativeBase) * GetParameter(this.Commands, offset + 2, secondMode, relativeBase);
                            offset += 4;
                            break;
                        case 3:
                            long temp;
                            if (!usedAddress)
                            {
                                temp = this.Address;
                                usedAddress = true;
                            }
                            else if (input == null)
                            {
                                if (usedNoPacket) {
                                    return output;
                                }
                                usedNoPacket = true;
                                temp = -1;
                            }
                            else if (!input.RetrievedX)
                            {
                                temp = input.X;
                                input.RetrievedX = true;
                            }
                            else
                            {
                                temp = input.Y;
                                input = null;
                            }
                            if (firstMode == '0')
                                this.Commands[(int)this.Commands[(int)(offset + 1)]] = temp;
                            else if (firstMode == '2')
                                this.Commands[(int)this.Commands[(int)(offset + 1)] + (int)relativeBase] = temp;
                            offset += 2;
                            break;
                        case 4:
                            outputBuffer.Add(GetParameter(this.Commands, offset + 1, firstMode, relativeBase));
                            if (outputBuffer.Count == 3) 
                            {
                                Packet p = new Packet(outputBuffer[0], outputBuffer[1], outputBuffer[2]);
                                Console.WriteLine("Address: " + p.Address + " X: " + p.X + " Y: " + p.Y);
                                output.Add(p);
                                outputBuffer.Clear();
                            }
                            offset += 2;
                            break;
                        case 5:
                            offset = GetParameter(this.Commands, offset + 1, firstMode, relativeBase) != 0 ? GetParameter(this.Commands, offset + 2, secondMode, relativeBase) : offset + 3;
                            break;
                        case 6:
                            offset = GetParameter(this.Commands, offset + 1, firstMode, relativeBase) == 0 ? GetParameter(this.Commands, offset + 2, secondMode, relativeBase) : offset + 3;
                            break;
                        case 7:
                            if (thirdMode == '0')
                                this.Commands[(int)this.Commands[(int)(offset + 3)]] = GetParameter(this.Commands, offset + 1, firstMode, relativeBase) < GetParameter(this.Commands, offset + 2, secondMode, relativeBase) ? 1 : 0;
                            else if (thirdMode == '2')
                                this.Commands[(int)this.Commands[(int)(offset + 3)] + (int)relativeBase] = GetParameter(this.Commands, offset + 1, firstMode, relativeBase) < GetParameter(this.Commands, offset + 2, secondMode, relativeBase) ? 1 : 0;
                            offset += 4;
                            break;
                        case 8:
                            if (thirdMode == '0')
                                this.Commands[(int)this.Commands[(int)(offset + 3)]] = GetParameter(this.Commands, offset + 1, firstMode, relativeBase) == GetParameter(this.Commands, offset + 2, secondMode, relativeBase) ? 1 : 0;
                            else if (thirdMode == '2')
                                this.Commands[(int)this.Commands[(int)(offset + 3)] + (int)relativeBase] = GetParameter(this.Commands, offset + 1, firstMode, relativeBase) == GetParameter(this.Commands, offset + 2, secondMode, relativeBase) ? 1 : 0;
                            offset += 4;
                            break;
                        case 9:
                            relativeBase += GetParameter(this.Commands, offset + 1, firstMode, relativeBase);
                            offset += 2;
                            break;
                    }
                }

                return null;
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
        }

        public class Packet
        {
            public long Address;
            public long X;
            public long Y;
            public bool RetrievedX = false;

            public Packet(long address, long x, long y)
            {
                this.Address = address;
                this.X = x;
                this.Y = y;
            }
        }

        public static string Solve2()
        {
            List<Packet> usedNATs = new List<Packet>();
            Packet NAT = null;
            Dictionary<long, Queue<Packet>> inputs = new Dictionary<long, Queue<Packet>>();
            List<Computer> network = new List<Computer>();
            for (int i = 0; i < 50; i++)
            {
                network.Add(new Computer(i));
                inputs.Add(i, new Queue<Packet>());
            }

            while (true)
            {
                for (int i = 0; i < 50; i++)
                {
                    List<Packet> results;
                    do
                    {
                        if (inputs[i].Count > 0)
                            results = network[i].Continue(inputs[i].Dequeue());
                        else
                            results = network[i].Continue(null);
                        foreach (Packet result in results)
                        {
                            if (result != null && result.Address < 50)
                                inputs[result.Address].Enqueue(result);
                            else if (result != null && result.Address == 255)
                                NAT = result;
                        }
                    }
                    while (results.Count > 0 && inputs[i].Count > 0);
                }

                if (inputs.All(q => q.Value.Count == 0))
                {
                    if (usedNATs.Any(q => q.Y == NAT.Y))
                    {
                        return NAT.Y.ToString();
                    }

                    inputs[0].Enqueue(NAT);
                    usedNATs.Add(NAT);
                }
            }
        }
    }
}
