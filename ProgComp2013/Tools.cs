using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgComp2013
{
    public static class Tools
    {
        public static Point Normal(this Direction dir)
        {
            switch (dir) {
                case Direction.North:
                    return new Point(0, -1);
                case Direction.East:
                    return new Point(1, 0);
                case Direction.South:
                    return new Point(0, 1);
                case Direction.West:
                    return new Point(-1, 0);
                default:
                    return new Point();
            }
        }
    }
}
