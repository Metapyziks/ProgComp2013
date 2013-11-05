using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgComp2013.Searchers
{
    public class FlowMap : IterativeSearcher
    {
        private Random _rand = new Random();

        protected override Direction Next(Agent agent)
        {
            var bestScore = 0.0;
            var bestDir = Direction.None;
            for (int r = 1; r < Map.Width; ++r) {
                foreach (var pos in agent.Pos.GetNeighbours(r)) {
                    var score = agent.WorkingMap[pos.X, pos.Y]
                        / (Math.Abs(pos.X - agent.X) + Math.Abs(pos.Y - agent.Y));
                    if (score > bestScore) {
                        bestDir = agent.GetDirection(pos);
                        bestScore = score;
                    }
                }
                if (bestScore > 0) break;
            }

            return bestDir;
        }
    }
}
