using System;
using System.Collections.Generic;
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
        public double CalculateScore(Map map)
        {
            var agent = new Agent(map, this);
            double score = 0.0;
            int moves = 0;

            while (agent.MoveNext()) {
                score += agent.Score;
                ++moves;
            }

            return score / Math.Max(10000, moves);
        }
    }
}
