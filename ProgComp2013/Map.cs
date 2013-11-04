using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgComp2013
{
    class Map
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
            var rawData = str.Split('\n').Select(x => x.Split(',').Select(y => double.Parse(y)).ToArray()).ToArray();
            double[,] data = new double[Width, Height];

            for (int x = 0; x < Width; ++x) {
                for (int y = 0; y < Height; ++y) {
                    data[x, y] = rawData[x][y];
                }
            }

            return new Map(data);
        }

        public const int Width = 100;
        public const int Height = 100;

        private double[,] _data;

        public double this[int x, int y]
        {
            get { return _data[x, y]; }
        }

        private Map(double[,] data)
        {
            _data = data;
        }
    }
}
