using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2019.challenge
{
    class Challenge
    {
        protected static string GetPath(int day)
        {
            return String.Format("{0}/day{1}.txt", Program.INPUT_FILE_DIR, day);
        }

        public static List<string> GetInputAsStringList(int day)
        {
            List<string> list = new List<string>();

            try
            {
                using (StreamReader sr = new StreamReader(GetPath(day)))
                {
                    while (!sr.EndOfStream)
                    {
                        list.Add(sr.ReadLine());
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return list;
        }

        public static List<int> GetInputAsIntList(int day)
        {
            List<int> list = new List<int>();

            try
            {
                using (StreamReader sr = new StreamReader(GetPath(day)))
                {
                    while (!sr.EndOfStream)
                    {
                        list.Add(int.Parse(sr.ReadLine()));
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return list;
        }
    }
}
