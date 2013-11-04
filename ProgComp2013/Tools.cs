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
    }
}
