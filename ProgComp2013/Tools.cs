using System;
using System.Collections.Generic;
using System.Drawing;

namespace ProgComp2013
{
    /// <summary>
    /// Assorted helper and extension methods.
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// Finds the position offset that would be the result of
        /// travelling in the specified direction.
        /// </summary>
        /// <param name="dir">Direction of the offset.</param>
        /// <returns>Position offset that a given direction represents.</returns>
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

        public static Point GetNeighbour(this Point pos, Direction dir)
        {
            var newPos = pos;
            pos.Offset(dir.Normal());
            return pos;
        }

        public static int Distance(this Point @this, Point @that)
        {
            return Math.Abs(@this.X - @that.X) + Math.Abs(@this.Y - @that.Y);
        }

        public static IEnumerable<Point> GetNeighbours(this Point pos, int radius)
        {
            for (int i = 0; i < radius << 2; ++i) {
                int x = pos.X - radius + (i + 1) / 2;
                int y = pos.Y + (radius - Math.Abs(pos.X - x)) * (((i & 1) << 1) - 1);

                if (x < 0 || y < 0 || x >= Map.Width || y >= Map.Height) continue;

                yield return new Point(x, y);
            }
        }
    }
}
