using System;
using System.IO;
using System.Linq;
using ProgComp2013.Searchers;
using System.Drawing;
using WebServer;
using System.Reflection;
using System.Threading;

namespace ProgComp2013
{
    class Program
    {
        public static String[] MapNames { get; private set; }
        public static DateTime[] LastUpdates { get; private set; }
        public static String[] Methods { get; private set; }
        public static double[] BestScores { get; private set; }

        static void Main(string[] args)
        {
            if (args.Length == 0) {
                args = Enumerable.Range(1, 10).Select(x =>
                    Path.Combine("..", "..", "..", "maps",
                    String.Format("map{0:00}.map", x))).ToArray();
            }

            MapNames = args.Select(x => Path.GetFileNameWithoutExtension(x)).ToArray();

            var maps = args.ToDictionary(x => x, x => Map.FromFile(x));
            var bestRoutes = new Route[MapNames.Length];
            LastUpdates = new DateTime[MapNames.Length];
            Methods = new String[MapNames.Length];
            BestScores = new double[MapNames.Length];

            var resDir = Path.Combine("..", "..", "..", "res");

            for (int i = 0; i < args.Length; ++i) {
                var name = MapNames[i];
                var path = Path.Combine(resDir, name);
                var bestName = String.Format("{0}.txt", path);
                if (File.Exists(bestName)) {
                    bestRoutes[i] = Route.FromFile(bestName, out Methods[i], out LastUpdates[i]);
                    BestScores[i] = bestRoutes[i].CalculateScore(maps[args[i]]);
                } else {
                    bestRoutes[i] = null;
                    BestScores[i] = 0.0;
                }
            }
            
            if (!Directory.Exists(resDir)) {
                Directory.CreateDirectory(resDir);
            }

            var server = new Server();

            new Thread(() => {
                var searchers = new[] { new Greedy(0.015), new Greedy(0.01), new Greedy(0.005), new Greedy(0.002) };

                while (!server.IsListening) Thread.Sleep(10);
                
                while (server.IsListening) {
                    int r = 0;
                    foreach (var arg in args) {
                        var name = Path.GetFileNameWithoutExtension(arg);
                        var path = Path.Combine(resDir, name);
                        var map = maps[arg];

                        var bestName = String.Format("{0}.txt", path);
                        var bestScore = BestScores[r];

                        int passes = 10;
                        foreach (var searcher in searchers) {
                            for (int i = 0; i < passes; ++i) {
                                var route = searcher.Search(map, Map.Width * Map.Height);
                                var score = route.CalculateScore(map);

                                if (score > bestScore) {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("New best score for {0} at {1}!", name, DateTime.Now);
                                    Console.WriteLine("+{0}", score - bestScore);
                                    Console.ResetColor();
                                    
                                    bestScore = score;
                                    bestRoutes[r] = route;
                                    BestScores[r] = score;
                                    Methods[r] = searcher.GetName();
                                    LastUpdates[r] = DateTime.Now;

                                    File.WriteAllText(bestName, String.Concat(searcher.GetName(), ", ",
                                        DateTime.Now.ToString(), Environment.NewLine, route.ToString()));
                                    route.ToImage(map).Save(bestName.Replace(".txt", ".png"));
                                }
                            }
                        }

                        ++r;
                    }
                }
            }).Start();

            server.ResourceRootUrl = "/res";
            DefaultResourceServlet.ResourceDirectory = resDir;
            server.AddPrefix("http://+:80/");
            server.BindServletsInAssembly(Assembly.GetExecutingAssembly());
            server.Run();
        }
    }
}
