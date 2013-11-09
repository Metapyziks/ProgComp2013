using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgComp2013
{
    public class Region : IEnumerable<Point>
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
            var tiles = new List<Point>();
            for (int x = 0; x < Map.Width; ++x) {
                for (int y = 0; y < Map.Height; ++y) {
                    tiles.Add(new Point(x, y));
                }
            }

            tiles.Sort(Comparer<Point>.Create((a, b) => Math.Sign(map[b.X, b.Y] - map[a.X, a.Y])));

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
                    regions.Add(new Region(map, group.ToList()));
                }
            }

            return regions.OrderByDescending(x => x.Score);
        }

        private Map _map;
        private List<Point> _tiles;

        public int Area { get { return _tiles.Length; } }

        public double Score { get; private set; }

        private Region(Map map, List<Point> tiles)
        {
            _map = map;
            _tiles = tiles;
            Score = _tiles.Sum(x => map[x.X, x.Y]);
        }

        public void Remove(Point pos)
        {
            _tiles.Remove(pos);
            Score -= _map[pos.X, pos.Y];
        }
        
        public Image ToImage()
        {
            return ToImage(Color.Blue);
        }

        public Image ToImage(Color clr)
        {
            var bmp = new Bitmap(Map.Width, Map.Height);
            using (var ctx = Graphics.FromImage(bmp)) {
                ctx.Clear(Color.Transparent);
            }

            foreach (var point in _tiles) {
                bmp.SetPixel(point.X, point.Y, clr);
            }

            return bmp;
        }

        public IEnumerator<Point> GetEnumerator()
        {
            return _tiles.AsEnumerable().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _tiles.GetEnumerator();
        }
    }
}