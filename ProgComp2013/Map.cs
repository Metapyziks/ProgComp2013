using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ProgComp2013
{
    /// <summary>
    /// Represents a two dimensional probability distribution.
    /// </summary>
    public class Map
    {
        /// <summary>
        /// Load a map from a text file.
        /// </summary>
        /// <param name="path">Path to the file to load from.</param>
        /// <returns>A map loaded from the given file.</returns>
        public static Map FromFile(String path)
        {
            return FromString(File.ReadAllText(path));
        }

        /// <summary>
        /// Load a map from a stream.
        /// </summary>
        /// <param name="stream">Stream to load from.</param>
        /// <returns>A map loaded from the given stream.</returns>
        public static Map FromStream(Stream stream)
        {
            var reader = new StreamReader(stream);
            return FromString(reader.ReadToEnd());
        }

        /// <summary>
        /// Load a map from a string.
        /// </summary>
        /// <param name="str">String to load from.</param>
        /// <returns>A map loaded from the given string.</returns>
        public static Map FromString(String str)
        {
            var lines = str.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var rawData = lines.Select(x => x.Split(',').Select(y => double.Parse(y)).ToArray()).ToArray();

            // Find the sum so we can normalize the map
            double sum = rawData.Sum(x => x.Sum());

            double[,] data = new double[Width, Height];

            for (int x = 0; x < Width; ++x) {
                for (int y = 0; y < Height; ++y) {
                    data[x, y] = rawData[y][x] / sum;
                }
            }

            return new Map(data);
        }

        /// <summary>
        /// Horizontal resolution of the map.
        /// </summary>
        public const int Width = 100;

        /// <summary>
        /// Vertical resolution of the map.
        /// </summary>
        public const int Height = 100;

        private readonly double[,] _data;

        private readonly double _min;
        private readonly double _max;

        /// <summary>
        /// Select a probability from a certain coordinate in the map.
        /// </summary>
        /// <param name="x">Horizontal coordinate.</param>
        /// <param name="y">Vertical coordinate.</param>
        /// <returns>The probability at the specified coordinate.</returns>
        public double this[int x, int y]
        {
            get { return _data[x, y]; }
            set { _data[x, y] = value; }
        }

        public double this[Point pos]
        {
            get { return _data[pos.X, pos.Y]; }
            set { _data[pos.X, pos.Y] = value; }
        }

        private Map(double[,] data)
        {
            _data = data;

            _min = double.MaxValue;
            _max = double.MinValue;

            // Find min and max for when generating an image
            foreach (var val in _data) {
                if (val < _min) _min = val;
                if (val > _max) _max = val;
            }
        }

        /// <summary>
        /// Creates an exact copy of this map.
        /// </summary>
        /// <returns>An exact copy of this map.</returns>
        public Map Clone()
        {
            return new Map((double[,]) _data.Clone());
        }

        /// <summary>
        /// Generates a grayscale image from this map.
        /// </summary>
        /// <returns>A grayscale image generated from this map.</returns>
        public Image ToImage()
        {
            var bmp = new Bitmap(Width, Height);
            
            for (var x = 0; x < Width; ++x) {
                for (var y = 0; y < Height; ++y) {
                    byte clr = (byte) Math.Round(((this[x, y] - _min) * 255.0) / (_max - _min));
                    bmp.SetPixel(x, y, Color.FromArgb(clr, clr, clr));
                }
            }

            return bmp;
        }
    }
}
