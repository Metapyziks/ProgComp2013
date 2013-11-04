using System.Collections.Generic;
using System.Linq;

namespace ProgComp2013
{
    public enum Direction
    {
        North = 1,
        East = 2,
        South = 3,
        West = 4
    }

    public class Route : IEnumerable<Direction>
    {
        private List<Direction> _dirs;

        public Route()
        {
            _dirs = new List<Direction>();
        }

        public Route(IEnumerable<Direction> dirs)
        {
            _dirs = dirs.ToList();
        }

        public void Add(Direction dir)
        {
            _dirs.Add(dir);
        }

        public IEnumerator<Direction> GetEnumerator()
        {
            return _dirs.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _dirs.GetEnumerator();
        }

        public double CalculateScore(Map map)
        {
            var agent = new Agent(map, this);
            double score = 0.0;

            while (agent.MoveNext()) {
                score += agent.Score;
            }

            return score;
        }
    }
}
