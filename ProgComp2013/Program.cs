using System;
using System.IO;
using System.Linq;
using ProgComp2013.Searchers;

namespace ProgComp2013
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0) {
                args = Enumerable.Range(1, 10).Select(x =>
                    Path.Combine("..", "..", "..", "maps",
                    String.Format("map{0:00}.map", x))).ToArray();
            }

            var searchers = new ISearcher[] {
                new RowScan(),
                new Greedy()
            };

            foreach (var arg in args) {
                var name = Path.GetFileNameWithoutExtension(arg);
                var map = Map.FromFile(arg);

                foreach (var searcher in searchers) {
                    Console.WriteLine("Running {0} on {1}", searcher.GetType().Name, name);
                    var route = searcher.Search(map, Map.Width * Map.Height);
                    var fileName = String.Format("{0}.{1}", name, searcher.GetType().Name.ToLower());

                    Console.WriteLine("Score: {0}", route.CalculateScore(map));

                    File.WriteAllText(String.Format("{0}.txt", fileName), route.ToString());
                    route.ToImage(map).Save(String.Format("{0}.png", fileName));
                }

                Console.WriteLine("================");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
