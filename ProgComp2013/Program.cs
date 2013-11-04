using System;
using System.IO;

namespace ProgComp2013
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0) {
                args = new String[] { Path.Combine("..", "..", "..", "maps", "map01.map") };
            }

            foreach (var arg in args) {
                var name = Path.GetFileNameWithoutExtension(arg);
                Console.WriteLine("Loading {0}", name);
                Map.FromFile(arg).ToImage().Save(name + ".png");
            }
        }
    }
}
