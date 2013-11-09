using System;
using System.IO;
using System.Linq;
using ProgComp2013.Searchers;
using System.Drawing;

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

            var searchers = new [] { new RegionSearch() };
                //Enumerable.Range(1, 10).Select(x => new FlowMap { Power = x });

            foreach (var arg in args) {
                var name = Path.GetFileNameWithoutExtension(arg);
                var map = Map.FromFile(arg);

                var regions = Region.FromMap(map);
                var regionMap = new Bitmap(Map.Width, Map.Height);

                var clrs = new[] {
                    Color.Blue,
                    Color.CornflowerBlue,
                    Color.Salmon,
                    Color.SeaShell,
                    Color.Goldenrod,
                    Color.Fuchsia,
                    Color.Lavender,
                    Color.Green
                };

                using (var ctx = Graphics.FromImage(regionMap)) {
                    int i = 0;
                    foreach (var region in regions) {
                        var img = region.ToImage(clrs[(i++) & 7]);
                        ctx.DrawImageUnscaled(img, Point.Empty);
                    }
                }

                regionMap.Save(String.Format("region{0}.png", name));
                
                var bestName = String.Format("{0}.txt", name);
                var bestScore = 0.0;
                Route best;

                if (File.Exists(bestName)) {
                    best = Route.FromFile(bestName);
                    bestScore = best.CalculateScore(map);
                    Console.WriteLine("Current best score: {0}", bestScore);
                }

                foreach (var searcher in searchers) {
                    Console.WriteLine("Running {0} on {1}", searcher.GetName(), name);
                    var route = searcher.Search(map, Map.Width * Map.Height);
                    var fileName = String.Format("{0}.{1}", name, searcher.GetName().ToLower());
                    var score = route.CalculateScore(map);
                    Console.WriteLine("Score: {0}", score);

                    File.WriteAllText(String.Format("{0}.txt", fileName), route.ToString());
                    route.ToImage(map).Save(String.Format("{0}.png", fileName));

                    if (score > bestScore) {
                        bestScore = score;

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("New best score!");
                        Console.ResetColor();

                        File.WriteAllText(bestName, route.ToString());
                        route.ToImage(map).Save(bestName.Replace(".txt", ".png"));
                    }
                }

                Console.WriteLine("================");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
