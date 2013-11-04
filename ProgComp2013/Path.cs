using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgComp2013
{
    public enum Direction
    {
        North = 1,
        East = 2,
        South = 3,
        West = 4
    }

    public class Path : IEnumerable<Direction>
    {
        private List<Direction> _dirs;

        public Path()
        {
            _dirs = new List<Direction>();
        }

        public Path(IEnumerable<Direction> dirs)
        {
            _dirs = dirs.ToList();
        }

        public void Add(Direction dir)
        {
            _dirs.Add(dir);
        }

        public IEnumerator<Direction> GetEnumerator()
        {
            return _dirs.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _dirs.GetEnumerator();
        }
    }
}
