using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode2019.challenge
{
    class Day14 : Challenge
    {
        public static string Solve1()
        {
            List<string> input = GetInputAsStringList(14);
            List<Chemical> chemicals = input.Select(s => Chemical.Parse(s)).ToList();
            chemicals.Add(new Chemical("ORE", 1));
            chemicals.ForEach(c => c.SetRequirements(chemicals));

            Chemical fuel = chemicals.First(c => c.Name == "FUEL");
            fuel.Needed = 1;
            fuel.Calculated = true;

            while (chemicals.Any(c => !c.Calculated))
            {
                List<Chemical> canBeCalculated = chemicals.Where(c => !c.Calculated && chemicals.Where(r => r.Requirements.ContainsKey(c)).All(r => r.Calculated)).ToList();
                foreach (Chemical chemical in canBeCalculated)
                {
                    // Get parents
                    List<Chemical> parents = chemicals.Where(c => c.Requirements.ContainsKey(chemical)).ToList();

                    // How many do they need each
                    int total = (int)parents.Sum(p => p.Needed * p.Requirements[chemical] / p.Amount);

                    // Round up
                    int sum = (int)(total / chemical.Amount);
                    if (total % chemical.Amount != 0) sum++;
                    chemical.Needed = sum * chemical.Amount;
                    chemical.Calculated = true;
                }
            }

            return chemicals.First(c => c.Name == "ORE").Needed.ToString();
        }

        public static string Solve2()
        {
            long counter = 0; // Just keep inputting numbers basically until the loop doesn't take too long and console shows number around 1 trillion
            List<Chemical> chemicals = new List<Chemical>();
            do
            {
                List<string> input = GetInputAsStringList(14);
                chemicals = input.Select(s => Chemical.Parse(s)).ToList();
                chemicals.Add(new Chemical("ORE", 1));
                chemicals.ForEach(c => c.SetRequirements(chemicals));

                Chemical fuel = chemicals.First(c => c.Name == "FUEL");
                fuel.Needed = counter++;
                fuel.Calculated = true;

                while (chemicals.Any(c => !c.Calculated))
                {
                    List<Chemical> canBeCalculated = chemicals.Where(c => !c.Calculated && chemicals.Where(r => r.Requirements.ContainsKey(c)).All(r => r.Calculated)).ToList();
                    foreach (Chemical chemical in canBeCalculated)
                    {
                        // Get parents
                        List<Chemical> parents = chemicals.Where(c => c.Requirements.ContainsKey(chemical)).ToList();

                        // How many do they need each
                        long total = parents.Sum(p => p.Needed * p.Requirements[chemical] / p.Amount);

                        // Round up
                        long sum = total / chemical.Amount;
                        if (total % chemical.Amount != 0) sum++;
                        chemical.Needed = sum * chemical.Amount;
                        chemical.Calculated = true;
                    }
                }

                Console.WriteLine(chemicals.First(c => c.Name == "ORE").Needed);
            }
            while (chemicals.First(c => c.Name == "ORE").Needed <= 1000000000000);

            return (counter - 2).ToString();
        }

        public class Chemical
        {
            public string Name;
            public long Amount;
            public long Needed;
            public bool Calculated = false;

            public Dictionary<Chemical, int> Requirements;
            private string requirementsInput;

            public Chemical(string name, long amount)
            {
                this.Name = name;
                this.Amount = amount;
                this.Requirements = new Dictionary<Chemical, int>();
            }

            public static Chemical Parse(string input)
            {
                List<string> sections = input.Split('>').Select(s => s.TrimEnd('=').Trim()).ToList();
                return new Chemical(sections[1].Split(' ')[1], int.Parse(sections[1].Split(' ')[0]))
                {
                    requirementsInput = sections[0]
                };
            }

            public void SetRequirements(List<Chemical> chemicals)
            {
                if (this.Name == "ORE") return;
                this.requirementsInput.Split(',').Select(s => s.Trim()).ToList().ForEach(r => this.Requirements.Add(chemicals.First(c => c.Name == r.Split(' ')[1]), int.Parse(r.Split(' ')[0])));
            }
        }
    }
}
