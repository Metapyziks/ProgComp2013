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

        public double Power { get; set; }

        public FlowMap()
        {
            Power = 2.0;
        }

        protected override Direction Next(Agent agent)
        {
            double[] scores = new double[4];
            for (int r = 1; r < Map.Width; ++r) {
                foreach (var pos in agent.Pos.GetNeighbours(r)) {                    
                    var dir = agent.GetDirection(pos);
                    scores[(int) dir - 1] += agent.WorkingMap[pos.X, pos.Y] / Math.Pow(r, Power);
                }
            }

            return (Direction) (Array.IndexOf(scores, scores.Max()) + 1);
        }

        public override string GetName()
        {
            return String.Format("{0}{1}", base.GetName(), Power);
        }
    }
}
