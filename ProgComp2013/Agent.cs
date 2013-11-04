using System.Collections.Generic;

namespace ProgComp2013
{
    /// <summary>
    /// Represents an entity that follows a route across a given map,
    /// and keeps track of the combined probabilities of all visited tiles.
    /// </summary>
    public class Agent : IEnumerator<Direction>
    {
        private IEnumerator<Direction> _dirs;
        private Map _originalMap;

        private Map _workingMap;

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
            _originalMap = map;

            Reset();
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

            Score += _workingMap[X, Y];

            _workingMap[X, Y] = 0.0;
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

        /// <summary>
        /// Resets the agent to its initial state.
        /// </summary>
        public void Reset()
        {
            _dirs.Reset();

            _workingMap = _originalMap.Clone();

            Score = 0.0;
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
