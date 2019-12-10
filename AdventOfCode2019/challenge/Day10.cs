using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode2019.challenge
{
    class Day10 : Challenge
    {
        public static string Solve1()
        {
            List<List<string>> input = GetInputAsStringList(10).Select(s => s.ToArray().Select(c => c.ToString()).ToList()).ToList();
            List<Asteroid> asteroids = new List<Asteroid>();

            int x = 0;
            int y = 0;
            foreach (List<string> row in input)
            {
                x = 0;
                foreach (string location in row)
                {
                    if (location == "#")
                        asteroids.Add(new Asteroid(x, y));
                    x++;
                }
                y++;
            }

            foreach (Asteroid asteroid in asteroids)
            {
                asteroid.GetInView(asteroids);
            }

            return asteroids.Max(a => a.inView.Count()).ToString();
        }

        public static string Solve2()
        {
            List<List<string>> input = GetInputAsStringList(10).Select(s => s.ToArray().Select(c => c.ToString()).ToList()).ToList();
            List<Asteroid> asteroids = new List<Asteroid>();

            int x = 0;
            int y = 0;
            foreach (List<string> row in input)
            {
                x = 0;
                foreach (string location in row)
                {
                    if (location == "#")
                        asteroids.Add(new Asteroid(x, y));
                    x++;
                }
                y++;
            }

            foreach (Asteroid asteroid in asteroids)
            {
                asteroid.GetInView(asteroids);
            }

            Asteroid station = asteroids.OrderByDescending(a => a.inView.Count()).First();

            int n = 200;
            Asteroid answer = station.inView.OrderBy(i => i.Key.Item1).ThenBy(i => i.Key.Item2).ToList().Union(station.blocked.OrderBy(i => i.Key.Item1).ThenBy(i => i.Key.Item2).ToList()).ToList()[n - 1].Value;
            return (answer.x * 100 + answer.y).ToString();
        }

        public class Asteroid
        {
            public int x;
            public int y;
            public Dictionary<(double, double), Asteroid> inView = new Dictionary<(double, double), Asteroid>();
            public Dictionary<(double, double), Asteroid> blocked = new Dictionary<(double, double), Asteroid>();

            public Asteroid(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public void GetInView(List<Asteroid> nearbyAstroids)
            {
                foreach (Asteroid nearby in nearbyAstroids)
                {
                    if (nearby.Equals(this))
                        continue;
                    else if (this.inView.Values.Contains(nearby))
                        continue;
                    else if (this.blocked.Values.Contains(nearby))
                        continue;

                    Dictionary<(double, double), Asteroid> asteroidsWithSameAngle = new Dictionary<(double, double), Asteroid>();
                    double angle = Asteroid.GetAngle(this, nearby);
                    foreach (Asteroid other in nearbyAstroids)
                    {
                        if (other.Equals(this))
                            continue;

                        double otherAngle = Asteroid.GetAngle(this, other);
                        if (otherAngle == angle)
                        {
                            double otherDistance = Asteroid.GetDistance(this, other);
                            asteroidsWithSameAngle.Add((otherAngle, otherDistance), other);
                            this.blocked.Add((otherAngle, otherDistance), other);
                        }
                    }

                    KeyValuePair<(double, double), Asteroid> mostInView = asteroidsWithSameAngle.OrderBy(i => i.Key.Item2).First();
                    this.inView.Add(mostInView.Key, mostInView.Value);
                    this.blocked.Remove(mostInView.Key);
                }
            }

            private static double GetAngle(Asteroid from, Asteroid to)
            {
                var radian = Math.Atan2((double)to.y - (double)from.y, (double)to.x - (double)from.x);
                var angle = (radian * (180 / Math.PI) + 360) % 360;

                return angle >= 270 ? angle - 270 : angle + 90;
            }

            private static double GetDistance(Asteroid from, Asteroid to)
            {
                return Math.Sqrt(Math.Pow((double)to.x - (double)from.x, 2) + Math.Pow((double)to.y - (double)from.y, 2));
            }

            public Asteroid Clone()
            {
                return new Asteroid(this.x, this.y);
            }

            public override int GetHashCode()
            {
                return (this.x + this.y).GetHashCode();
            }

            public override bool Equals(object obj)
            {
                Asteroid other = obj as Asteroid;
                return other != null && (other.x == this.x && other.y == this.y);
            }
        }
    }
}
