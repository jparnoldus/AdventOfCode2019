using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode2019.challenge
{
    class Day12 : Challenge
    {
        public static string Solve1()
        {
            List<string> input = GetInputAsStringList(12);
            List<Moon> moons = new List<Moon>();
            input.ForEach(s => {
                List<string> coordinates = s.Trim().Trim('<').Trim('>').Split(',').Select(s => s.Trim()).ToList();
                int x = int.Parse(coordinates[0].Substring(2));
                int y = int.Parse(coordinates[1].Substring(2));
                int z = int.Parse(coordinates[2].Substring(2));
                moons.Add(new Moon(x, y, z));
            });

            int n = 1000;
            for (int i = 0; i < n; i++) {
                moons.ForEach(m => m.SetNewVelocity(moons) );
                moons.ForEach(m => m.Step() );
            }

            return moons.Sum(m => m.GetTotalEnergy()).ToString();
        }

        public static string Solve2()
        {
            List<string> input = GetInputAsStringList(12);
            List<Moon> moons = new List<Moon>();
            input.ForEach(s => {
                List<string> coordinates = s.Trim().Trim('<').Trim('>').Split(',').Select(s => s.Trim()).ToList();
                int x = int.Parse(coordinates[0].Substring(2));
                int y = int.Parse(coordinates[1].Substring(2));
                int z = int.Parse(coordinates[2].Substring(2));
                moons.Add(new Moon(x, y, z));
            });

            int n = 1;
            while(!moons.All(m => m.orbitCompleted)) {
                moons.ForEach(m => m.SetNewVelocity(moons));
                moons.ForEach(m => m.Step());
                n++;
            }

            return moons.Select(m => m.orbit.Count).Aggregate(1, (x, y) => x * y).ToString();
        }

        public class Moon
        {
            public bool orbitCompleted = false;
            public List<Point> orbit;
            public Point position;
            public Point velocity;

            public Moon(int x, int y, int z)
            {
                this.position = new Point(x, y, z);
                this.velocity = new Point(0, 0, 0);

                this.orbit = new List<Point>();
                this.orbit.Add(this.position.Clone());
            }

            public void SetNewVelocity(List<Moon> moons) {
                Point newVel = new Point(0, 0, 0);
                moons.ForEach(m => {
                    newVel.x += Compare(m.position.x, this.position.x);
                    newVel.y += Compare(m.position.y, this.position.y);
                    newVel.z += Compare(m.position.z, this.position.z);
                });
                this.velocity.Add(newVel);
            }

            public void Step()
            {
                if (orbitCompleted)
                    return;

                this.position.Add(this.velocity);
                if (!orbit.First().Equals(this.position))
                    this.orbit.Add(this.position.Clone());
                else
                    this.orbitCompleted = true;
            }

            public int GetTotalEnergy()
            {
                return this.position.AbsSum() * this.velocity.AbsSum();
            }

            private static int Compare(int pos1, int pos2) {
                if (pos1 > pos2)
                    return 1;
                else if (pos1 == pos2)
                    return 0;
                else
                    return -1;
            }

            public Moon Clone()
            {
                Moon clone = new Moon(0,0,0);
                clone.position = this.position.Clone();
                clone.velocity = this.velocity.Clone();
                return clone;
            }
        }

        public class Point
        {
            public int x;
            public int y;
            public int z;

            public Point(int x, int y, int z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public void Add(Point point) 
            {
                this.x += point.x;
                this.y += point.y;
                this.z += point.z;
            }

            public int AbsSum()
            {
                return Math.Abs(this.x) + Math.Abs(this.y) + Math.Abs(this.z);
            }

            public Point Clone()
            {
                return new Point(x, y, z);
            }

            public override int GetHashCode()
            {
                return (this.x.ToString() + this.y.ToString(), this.z.ToString()).GetHashCode();
            }

            public override bool Equals(object obj)
            {
                Point other = obj as Point;
                return other != null && (other.x == this.x && other.y == this.y && other.z == this.z);
            }
        }
    }
}
