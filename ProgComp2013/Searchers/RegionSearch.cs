using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgComp2013.Searchers
{
    public class RegionSearch : IterativeSearcher
    {
        private List<Region> _regions;
        private Region _curRegion;

        protected override void OnBegin()
        {   
            _regions = null;
            _curRegion = null;
        }

        protected override Direction Next(Agent agent)
        {
            if (_regions == null) {
                _regions = Region.FromMap(agent.WorkingMap).ToList();
            }

            // If there are no regions left, entire map is explored
            // and we can stop
            if (_regions.Count == 0) return Direction.None;

            if (_curRegion == null || _curRegion.Count() == 0) {
                var scoreDict = _regions.ToDictionary(x => x, x =>
                x.Score - Math.Pow(x.Min(y => y.Distance(agent.Pos)), 0.4284172) / (Map.Width * Map.Height));

                _regions.Sort(Comparer<Region>.Create((a, b) => Math.Sign(scoreDict[b] - scoreDict[a])));

                // Find highest scoring region from when they were originally
                // calculated
                _curRegion = _regions.First();
            }

            // Find nearest tile in the best region
            var nearest = _curRegion.OrderBy(x => x.Distance(agent.Pos))
                .Where(x => x != agent.Pos).First();

            var dir = agent.GetDirection(nearest);
            var nextPos = agent.Pos.GetNeighbour(dir);
            var nextRegion = _regions.FirstOrDefault(x => x.Contains(nextPos));

            if (nextRegion != null) {
                nextRegion.Remove(nextPos);

                if (nextRegion.Score == 0.0) {
                    _regions.Remove(nextRegion);
                }
            }

            return dir;
        }
    }
}
