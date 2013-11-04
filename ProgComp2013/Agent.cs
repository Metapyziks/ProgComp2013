using System;
using System.Collections.Generic;
using System.Drawing;

namespace ProgComp2013
{
    /// <summary>
    /// Represents an entity that follows a route across a given map,
    /// and keeps track of the combined probabilities of all visited tiles.
    /// </summary>
    public class Agent : IEnumerator<Direction>
    {
        private IEnumerator<Direction> _dirs;

        /// <summary>
        /// Current horizontal position of the agent.
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Current vertical position of the agent.
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Current accumulated probability of the agent.
        /// </summary>
        public double Score { get; private set; }

        /// <summary>
        /// Map representing the probabilities of unvisited areas.
        /// </summary>
        public Map WorkingMap { get; private set; }

        /// <summary>
        /// Creates a new agent that follows the specified route
        /// across the given map.
        /// </summary>
        /// <param name="map">Map to traverse.</param>
        /// <param name="dirs">Route to follow.</param>
        public Agent(Map map, IEnumerable<Direction> dirs)
            : this(map, dirs.GetEnumerator()) { }
        
        /// <summary>
        /// Creates a new agent that follows the specified route
        /// across the given map.
        /// </summary>
        /// <param name="map">Map to traverse.</param>
        /// <param name="dirIter">Next direction selection enumerator.</param>
        public Agent(Map map, IEnumerator<Direction> dirIter)
        {
            _dirs = dirIter;
            WorkingMap = map.Clone();

            X = 0;
            Y = 0;

            Score = 0.0;
        }

        /// <summary>
        /// The direction the agent last moved in.
        /// </summary>
        public Direction Current
        {
            get { return _dirs.Current; }
        }

        object System.Collections.IEnumerator.Current
        {
            get { return _dirs.Current; }
        }

        private void OnMove()
        {
            var normal = Current.Normal();

            X += normal.X;
            Y += normal.Y;

            Score += WorkingMap[X, Y];

            WorkingMap[X, Y] = 0.0;
        }

        /// <summary>
        /// Advances the agent to the next step in its route.
        /// </summary>
        /// <returns>True if the agent has not reached the end of
        /// its route, false otherwise.</returns>
        public bool MoveNext()
        {
            if (_dirs.MoveNext()) {
                OnMove();
                return true;
            } else {
                return false;
            }
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public Direction GetDirection(Point pos)
        {
            int dx = pos.X - X;
            int dy = pos.Y - Y;

            if (Math.Abs(dx) >= Math.Abs(dy)) {
                if (dx > 0) return Direction.East;
                else return Direction.West;
            } else {
                if (dy > 0) return Direction.South;
                else return Direction.North;
            }
        }

        public IEnumerable<Point> GetNeighbours(int radius)
        {
            for (int i = 0; i < radius << 2; ++i) {
                int x = X - radius + (i + 1) / 2;
                int y = Y + (radius - Math.Abs(X - x)) * (((i & 1) << 1) - 1);

                if (x < 0 || y < 0 || x >= Map.Width || y >= Map.Height) continue;

                yield return new Point(x, y);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _dirs.Dispose();
        }
    }
}
