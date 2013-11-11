using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;

namespace ProgComp2013
{
    /// <summary>
    /// Enumeration of the cardinal directions.
    /// </summary>
    public enum Direction
    {
        None = 0,
        North = 1,
        East = 2,
        South = 3,
        West = 4
    }

    /// <summary>
    /// Respresents a series of directions taken to traverse a map.
    /// </summary>
    public class Route : IEnumerable<Direction>
    {
        public static Route FromFile(String path)
        {
            return FromString(File.ReadAllLines(path)[1]);
        }

        public static Route FromFile(String path, out String how, out DateTime when)
        {
            var lines = File.ReadAllLines(path);
            var info = lines[0].Split(',').Select(x => x.Trim()).ToArray();

            how = info[0];
            when = DateTime.Parse(info[1]);

            return FromString(lines[1]);
        }

        public static Route FromString(String str)
        {
            var dict = new Direction[] { Direction.North, Direction.South, Direction.East, Direction.West }
                .SelectMany(x => new Dictionary<char, Direction> { 
                    { x.ToString().ToUpper()[0], x },
                    { x.ToString().ToLower()[0], x }
                }).ToDictionary(x => x.Key, x => x.Value);

            return new Route(str.Where(x => dict.ContainsKey(x)).Select(x => dict[x]));
        }

        private List<Direction> _dirs;

        /// <summary>
        /// Creates a new route with an empty list of directions.
        /// </summary>
        public Route()
        {
            _dirs = new List<Direction>();
        }

        /// <summary>
        /// Creates a new route with a specified initial list of directions.
        /// </summary>
        /// <param name="dirs">Initial direction sequence.</param>
        public Route(IEnumerable<Direction> dirs)
        {
            _dirs = dirs.ToList();
        }

        /// <summary>
        /// Appends a direction to the route.
        /// </summary>
        /// <param name="dir">Direction to append.</param>
        public void Add(Direction dir)
        {
            _dirs.Add(dir);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the route.
        /// </summary>
        /// <returns>An enumerator that iterates through the route.</returns>
        public IEnumerator<Direction> GetEnumerator()
        {
            return _dirs.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _dirs.GetEnumerator();
        }

        /// <summary>
        /// Finds an objective score representing the quality of
        /// the route as a value between 0.0 and 1.0.
        /// </summary>
        /// <param name="map">The map to evaluate the route on.</param>
        /// <returns>A score between 0.0 and 1.0.</returns>
        public double CalculateScore(Map map, Point start)
        {
            var agent = new Agent(map, this, start);
            double score = 0.0;
            int moves = 0;

            while (agent.MoveNext()) {
                score += agent.Score;
                ++moves;
            }

            return score / Math.Max(10000, moves);
        }

        /// <summary>
        /// Generates an image representing this route.
        /// </summary>
        /// <returns>An image generated from this route.</returns>
        public Image ToImage(Map map, Point start)
        {
            var bmp = new Bitmap(Map.Width * 3, Map.Height * 3);
            using (var ctx = Graphics.FromImage(bmp)) {
                var agent = new Agent(map, this, start);
                var initClr = Color.Red;
                var finlClr = Color.Green;

                ctx.InterpolationMode = InterpolationMode.NearestNeighbor;
                ctx.DrawImage(map.ToImage(), new Rectangle(0, 0, Map.Width * 3, Map.Height * 3),
                    -1f / 3f, -1f / 3f, Map.Width, Map.Height, GraphicsUnit.Pixel);

                var length = this.Count();

                var lastPos = new Point(agent.X * 3 + 1, agent.Y * 3 + 1);
                int i = 0;
                while (agent.MoveNext()) {
                    var t = (i++ + 0.0) / length;
                    var clr = Color.FromArgb(
                        (byte) (initClr.R * (1.0 - t) + finlClr.R * t),
                        (byte) (initClr.G * (1.0 - t) + finlClr.G * t),
                        (byte) (initClr.B * (1.0 - t) + finlClr.B * t));

                    var pen = new Pen(clr);

                    var nextPos = new Point(agent.X * 3 + 1, agent.Y * 3 + 1);
                    ctx.DrawLine(pen, lastPos, nextPos);
                    lastPos = nextPos;
                }
            }

            return bmp;
        }

        /// <summary>
        /// Converts the route into a sequence of N, S, E and W characters.
        /// </summary>
        /// <returns>A sequence of characters representing this route.</returns>
        public override string ToString()
        {
            return new String(_dirs.Select(x => x.ToString()[0]).ToArray());
        }
    }
}
