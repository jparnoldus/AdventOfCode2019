using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode2019.challenge
{
    class Day3 : Challenge
    {
        public static string Solve1()
        {
            int index = 0;
            List<List<Point>> locations = new List<List<Point>>() { new List<Point>(), new List<Point>() };
            GetInputAsCsStringListList(3).ForEach(s =>
            {
                int x = 0,
                    y = 0;
                s.ForEach(t =>
                {
                    switch (t.First())
                    {
                        case 'R':
                            Enumerable.Range(0, int.Parse(t.Substring(1))).ToList().ForEach(i => locations[index].Add(new Point(x++, y)));
                            break;
                        case 'L':
                            Enumerable.Range(0, int.Parse(t.Substring(1))).ToList().ForEach(i => locations[index].Add(new Point(x--, y)));
                            break;
                        case 'D':
                            Enumerable.Range(0, int.Parse(t.Substring(1))).ToList().ForEach(i => locations[index].Add(new Point(x, y--)));
                            break;
                        case 'U':
                            Enumerable.Range(0, int.Parse(t.Substring(1))).ToList().ForEach(i => locations[index].Add(new Point(x, y++)));
                            break;
                    }
                });
                index++;
            });

            return locations[0].Intersect(locations[1]).Except(new List<Point>() { new Point(0, 0) }).ToList().Min(i => Math.Abs(i.x) + Math.Abs(i.y)).ToString();
        }

        public static string Solve2()
        {
            int index = 0;
            List<List<Point>> locations = new List<List<Point>>() { new List<Point>(), new List<Point>() };
            GetInputAsCsStringListList(3).ForEach(s =>
            {
                int x = 0,
                    y = 0,
                    steps = 0;
                s.ForEach(t =>
                {
                    switch (t.First())
                    {
                        case 'R':
                            Enumerable.Range(0, int.Parse(t.Substring(1))).ToList().ForEach(i => locations[index].Add(new Point(x++, y, steps++)));
                            break;
                        case 'L':
                            Enumerable.Range(0, int.Parse(t.Substring(1))).ToList().ForEach(i => locations[index].Add(new Point(x--, y, steps++)));
                            break;
                        case 'D':
                            Enumerable.Range(0, int.Parse(t.Substring(1))).ToList().ForEach(i => locations[index].Add(new Point(x, y--, steps++)));
                            break;
                        case 'U':
                            Enumerable.Range(0, int.Parse(t.Substring(1))).ToList().ForEach(i => locations[index].Add(new Point(x, y++, steps++)));
                            break;
                    }
                });
                index++;
            });

            return locations[0].Intersect(locations[1]).Concat(locations[1].Intersect(locations[0])).GroupBy(i => new { i.x, i.y }).Select(i => new Point(i.First().x, i.First().y, i.Sum(x => x.steps))).Except(new List<Point>() { new Point(0, 0) }).Min(i => i.steps).ToString();
        }

        public class Point
        {
            public int x;
            public int y;
            public int steps;

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public Point(int x, int y, int steps)
            {
                this.x = x;
                this.y = y;
                this.steps = steps;
            }

            public Point Clone()
            {
                return new Point(this.x, this.y);
            }

            public override int GetHashCode()
            {
                return (this.x + this.y).GetHashCode();
            }

            public override bool Equals(object obj)
            {
                Point other = obj as Point;
                return other != null && (other.x == this.x && other.y == this.y);
            }
        }
    }
}
