using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgComp2013.Searchers
{
    public class RegionSearch : IterativeSearcher
    {
        private static readonly IComparer<Region> _sRegionComp =
            Comparer<Region>.Create((a, b) => Math.Sign(b.Score - a.Score));

        private List<Region> _regions;

        protected override void OnBegin()
        {
            _regions = null;
        }

        protected override Direction Next(Agent agent)
        {
            if (_regions == null) {
                _regions = Region.FromMap(agent.WorkingMap).ToList();
            }

            // If there are no regions left, entire map is explored
            // and we can stop
            if (_regions.Count == 0) return Direction.None;

            //_regions.Sort(_sRegionComp);

            // Find highest scoring region from when they were originally
            // calculated
            var best = _regions.First();

            // Find nearest tile in the best region
            var nearest = best.OrderBy(x => x.Distance(agent.Pos))
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
