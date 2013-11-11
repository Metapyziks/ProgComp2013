using System.Collections.Generic;
using System.Drawing;

namespace ProgComp2013
{
    /// <summary>
    /// Abstract base class for searchers that iteratively moves an agent
    /// to create a route.
    /// </summary>
    public abstract class IterativeSearcher : ISearcher
    {
        private Agent _agent;

        /// <summary>
        /// Method called when a new route is about to be searched.
        /// </summary>
        protected virtual void OnBegin() { }

        /// <summary>
        /// Method to decide what direction an agent should travel in order
        /// to produce a route.
        /// </summary>
        /// <param name="agent">Agent to instruct.</param>
        /// <returns>The direction the agent should move next step.</returns>
        protected abstract Direction Next(Agent agent);

        private IEnumerator<Direction> Enumerator(Map map)
        {
            for (;;) {
                var dir = Next(_agent);
                if (dir == Direction.None) yield break;
                yield return dir;
            }
        }

        /// <summary>
        /// Generates a route that traverses the given map within the specified
        /// number of steps.
        /// </summary>
        /// <param name="map">Map to traverse.</param>
        /// <param name="maxLength">Maximum number of steps to take.</param>
        /// <returns>A route that traverses the given map.</returns>
        public Route Search(Map map, Point start, int maxLength = Map.Width * Map.Height)
        {
            _agent = new Agent(map, Enumerator(map), start);

            OnBegin();

            var route = new Route();
            for (int i = 0; i < maxLength && _agent.MoveNext(); ++i) {
                route.Add(_agent.Current);
            }

            return route;
        }

        public virtual string GetName()
        {
            return GetType().Name;
        }
    }
}
