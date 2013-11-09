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

            _regions.Sort(_sRegionComp);

            var best = _regions.First();

            var nearest = best.OrderBy(x => x.Distance(agent.Pos))
                .Where(x => x != agent.Pos).First();
            return agent.GetDirection(nearest);
        }
    }
}
