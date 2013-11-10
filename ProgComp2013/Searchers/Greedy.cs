﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ProgComp2013.Searchers
{
    public class Greedy : IterativeSearcher
    {
        private Random _rand = new Random();
        private double _error;
        private Region _initialRegion;
        private bool _visitedInitialRegion;

        public Greedy(double error = 0.0)
        {
            _error = error;
        }

        private double GetScore(Agent agent, Direction dir)
        {
            var pos = new Point(agent.X, agent.Y);
            pos.Offset(dir.Normal());

            return agent.WorkingMap[pos.X, pos.Y];
        }

        protected override void OnBegin()
        {
            _initialRegion = null;
        }

        protected override Direction Next(Agent agent)
        {
            if (_initialRegion == null) {
                _initialRegion = Region.FromMap(agent.WorkingMap)
                    .Where(x => x.Tier == 0)
                    .OrderByDescending(x => x.Area * _rand.NextDouble())
                    .First();

                _visitedInitialRegion = _rand.NextDouble() < 0.25;
            }
            
            if (!_visitedInitialRegion) {
                _visitedInitialRegion = _initialRegion.Contains(agent.Pos);
            }

            if (!_visitedInitialRegion) {
                var nearest = _initialRegion
                    .OrderBy(x => x.Distance(agent.Pos))
                    .First();

                return agent.GetDirection(nearest);
            }

            var dirs = new List<Direction>();

            if (agent.X > 0) dirs.Add(Direction.West);
            if (agent.Y > 0) dirs.Add(Direction.North);
            if (agent.X < Map.Width - 1) dirs.Add(Direction.East);
            if (agent.Y < Map.Height - 1) dirs.Add(Direction.South);

            dirs = dirs
                .Where(x => GetScore(agent, x) > 0.0)
                .OrderBy(x => _rand.Next())
                .OrderByDescending(x => GetScore(agent, x))
                .ToList();

            if (dirs.Count == 0) {
                for (int r = 1; r < Map.Width; ++r) {
                    var scores = agent.Pos.GetNeighbours(r)
                        .Select(x => Tuple.Create(agent.GetDirection(x), agent.WorkingMap[x]))
                        .Where(x => x.Item2 > 0.0);

                    dirs = Tools.AllDirs
                        .Select(x => Tuple.Create(x, scores
                            .Where(y => y.Item1 == x)
                            .Sum(y => y.Item2)))
                        .OrderByDescending(x => x.Item2)
                        .Select(x => x.Item1).ToList();
                }
            }

            if (dirs.Count == 0) {
                return Direction.None;
            }
            
            while (dirs.Count > 1 && _rand.NextDouble() < _error) {
                dirs.RemoveAt(0);
            }

            return dirs.First();
        }

        public override string GetName()
        {
            return String.Format("{0}-{1}", base.GetName(), (int) (_error * 1000));
        }
    }
}
