using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgComp2013.Searchers
{
    public class Greedy : IterativeSearcher
    {
        private Random _rand = new Random();

        private double GetScore(Agent agent, Direction dir)
        {
            var pos = new Point(agent.X, agent.Y);
            pos.Offset(dir.Normal());

            return agent.WorkingMap[pos.X, pos.Y];
        }

        protected override Direction Next(Agent agent)
        {
            var dirs = new List<Direction>();

            if (agent.X > 0) dirs.Add(Direction.West);
            if (agent.Y > 0) dirs.Add(Direction.North);
            if (agent.X < Map.Width - 1) dirs.Add(Direction.East);
            if (agent.Y < Map.Height - 1) dirs.Add(Direction.South);

            dirs = dirs.OrderBy(x => _rand.Next()).ToList();

            var bestDir = Direction.None;
            var bestScore = 0.0;
            
            foreach (var dir in dirs) {
                var score = GetScore(agent, dir);
                if (score > bestScore) {
                    bestDir = dir;
                    bestScore = score;
                }
            }

            if (bestDir == Direction.None) {
                for (int r = 1; r < Map.Width; ++r) {
                    foreach (var pos in agent.GetNeighbours(r)) {
                        var score = agent.WorkingMap[pos.X, pos.Y]
                            / (Math.Abs(pos.X - agent.X) + Math.Abs(pos.Y - agent.Y));
                        if (score > bestScore) {
                            bestDir = agent.GetDirection(pos);
                            bestScore = score;
                        }
                    }
                    if (bestScore > 0) break;
                }
            }

            return bestDir;
        }
    }
}
