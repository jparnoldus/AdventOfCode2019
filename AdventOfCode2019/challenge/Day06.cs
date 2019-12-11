using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode2019.challenge
{
    class Day06 : Challenge
    {
        public static string Solve1()
        {
            List<Orbit> orbits = GetInputAsStringList(6).Select(i => new Orbit(i.Split(')')[0], i.Split(')')[1])).ToList();
            orbits.ForEach(orbit => orbit.CountOrbits(orbits));

            return orbits.Sum(orbit => orbit.orbits).ToString();
        }

        public static string Solve2()
        {
            List<Orbit> orbits = GetInputAsStringList(6).Select(i => new Orbit(i.Split(')')[0], i.Split(')')[1])).ToList();

            Orbit start = orbits.Where(orbit => orbit.name == "YOU").First();
            return start.SearchSanta(orbits, start, new List<Orbit>()).ToString();
        }
    }

    public class Orbit
    {
        public string before;
        public string name;
        public int orbits;

        public Orbit(string before, string name)
        {
            this.orbits = 0;
            this.before = before;
            this.name = name;
        }

        public void CountOrbits(List<Orbit> orbits) 
        {
            this.orbits++;
            foreach (Orbit orbit in orbits.Where(orbit => orbit.before == this.name))
            {
                orbit.CountOrbits(orbits);
            }
        }

        public int SearchSanta(List<Orbit> orbits, Orbit from, List<Orbit> path) {
            if (this.name == "SAN")
                return path.Count - 2;

            List<int> answers = new List<int> { 0 };
            foreach (Orbit orbit in orbits.Where(orbit => (orbit.before == this.name || orbit.name == this.before) && orbit.name != from.name))
            {
                List<Orbit> clone = new List<Orbit> { this };
                path.ForEach(p => clone.Add(p.Clone()));

                answers.Add(orbit.SearchSanta(orbits, this, clone));
            }

            return answers.Max(a => a);
        }

        public Orbit Clone()
        {
            return new Orbit(this.before, this.name);
        }
    }
}
