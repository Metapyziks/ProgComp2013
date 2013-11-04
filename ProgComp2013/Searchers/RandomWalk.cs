using System;
using System.Collections.Generic;

namespace ProgComp2013.Searchers
{
    public class RandomWalk : IterativeSearcher
    {
        private Random _rand = new Random();

        protected override Direction Next(Agent agent)
        {
            var dirs = new List<Direction>();

            if (agent.X > 0) dirs.Add(Direction.West);
            if (agent.Y > 0) dirs.Add(Direction.North);
            if (agent.X < Map.Width - 1) dirs.Add(Direction.East);
            if (agent.Y < Map.Height - 1) dirs.Add(Direction.South);

            return dirs[_rand.Next(dirs.Count)];
        }
    }
}
