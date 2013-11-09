using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgComp2013
{
    public class Region
    {
        private static void Flood(Point point, List<Point> groupTiles, List<Point> group)
        {
            groupTiles.Remove(point);
            group.Add(point);

            foreach (var neighbour in point.GetNeighbours(1)) {
                if (groupTiles.Contains(neighbour)) {
                    Flood(neighbour, groupTiles, group);
                }
            }
        }

        public static IEnumerable<Region> FromMap(Map map)
        {
            var tiles = new SortedList<double, Point>();
            for (int x = 0; x < Map.Width; ++x) {
                for (int y = 0; y < Map.Height; ++y) {
                    tiles.Add(-map[x, y], new Point(x, y));
                }
            }

            var regions = new List<Region>();

            int regionGroups = 8;
            for (int i = 0; i < regionGroups; ++i) {
                int min = (int) (((i + 0.0) / regionGroups) * tiles.Count);
                int max = (int) (((i + 1.0) / regionGroups) * tiles.Count);

                var groupTiles = Enumerable.Range(min, max - min)
                    .Select(x => tiles[x]).ToList();

                while (groupTiles.Count > 0) {
                    var group = new List<Point>();
                    Flood(groupTiles.First(), groupTiles, group);
                    regions.Add(group);
                }
            }

            return regions;
        }

        private Point[] _tiles;

        public int Area { get { return _tiles.Length; } }

        public double Score { get; private set; }

        private Region(Map map, Point[] tiles)
        {
            _tiles = tiles;
            Score = _tiles.Sum(x => map[x.X, x.Y]);
        }
    }
}