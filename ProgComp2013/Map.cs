using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgComp2013
{
    public class Map
    {
        public static Map FromFile(String path)
        {
            return FromString(File.ReadAllText(path));
        }

        public static Map FromStream(Stream stream)
        {
            var reader = new StreamReader(stream);
            return FromString(reader.ReadToEnd());
        }

        public static Map FromString(String str)
        {
            var lines = str.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var rawData = lines.Select(x => x.Split(',').Select(y => double.Parse(y)).ToArray()).ToArray();
            double[,] data = new double[Width, Height];

            double sum = rawData.Sum(x => x.Sum());
            for (int x = 0; x < Width; ++x) {
                for (int y = 0; y < Height; ++y) {
                    data[x, y] = rawData[y][x] / sum;
                }
            }

            return new Map(data);
        }

        public const int Width = 100;
        public const int Height = 100;

        private readonly double[,] _data;

        private readonly double _min;
        private readonly double _max;

        public double this[int x, int y]
        {
            get { return _data[x, y]; }
        }

        private Map(double[,] data)
        {
            _data = data;

            _min = double.MaxValue;
            _max = double.MinValue;

            foreach (var val in _data) {
                if (val < _min) _min = val;
                if (val > _max) _max = val;
            }
        }

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
