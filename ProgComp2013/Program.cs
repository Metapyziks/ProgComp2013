using System;
using System.IO;
using ProgComp2013.Searchers;

namespace ProgComp2013
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0) {
                args = new String[] { Path.Combine("..", "..", "..", "maps", "map01.map") };
            }

            var searcher = new RowScan();

            foreach (var arg in args) {
                var name = Path.GetFileNameWithoutExtension(arg);
                var map = Map.FromFile(arg);

                Console.WriteLine("Running {0} on {1}", searcher.GetType().Name, name);
                var route = searcher.Search(map);

                Console.WriteLine("Score: {0}", route.CalculateScore(map));

                File.WriteAllText(name + ".route.txt", route.ToString());
                route.ToImage(map).Save(name + ".route.png");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
