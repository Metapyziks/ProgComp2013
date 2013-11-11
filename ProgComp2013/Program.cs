using System;
using System.IO;
using System.Linq;
using ProgComp2013.Searchers;
using System.Drawing;
using WebServer;
using System.Reflection;
using System.Threading;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace ProgComp2013
{
    class Program
    {
        public static bool WebServer { get; private set; }
        public static int Exercise { get; private set; }

        public static String[] MapNames { get; private set; }
        public static DateTime[] LastUpdates { get; private set; }
        public static String[] Methods { get; private set; }
        public static double[] BestScores { get; private set; }

        static void Main(string[] args)
        {
            var options = args
                .Where(x => x.StartsWith("-"))
                .Select(x => x.Substring(1));

            WebServer = options.Contains("-server") || options.Contains("s");
            Exercise = options
                .Where(x => Regex.IsMatch(x, "^e[0-2]$"))
                .Select(x => int.Parse(x.Substring(1)))
                .FirstOrDefault();

            args = args
                .Where(x => !x.StartsWith("-"))
                .ToArray();

            if (args.Length == 0) {
                args = Enumerable.Range(1, 10).Select(x =>
                    Path.Combine("..", "..", "..", "maps",
                    String.Format("map{0:00}.map", x))).ToArray();
            }

            if (Exercise == 0) {
                Console.WriteLine("Searcher for main problem starting.");
            } else {
                Console.WriteLine("Searcher for extension {0} starting.", Exercise);
            }

            MapNames = args.Select(x => Path.GetFileNameWithoutExtension(x)).ToArray();

            var maps = args.ToDictionary(x => x, x => Map.FromFile(x));
            var bestRoutes = new Route[MapNames.Length];
            LastUpdates = new DateTime[MapNames.Length];
            Methods = new String[MapNames.Length];
            BestScores = new double[MapNames.Length];

            var resDir = Path.Combine("..", "..", "..", String.Format("res{0}", Exercise));

            Dictionary<String, Point> startPoints;
            if (Exercise == 1) {
                startPoints = new Dictionary<string, Point> {
                        { "map01", new Point(20, 35) },
                        { "map02", new Point(99, 5) },
                        { "map03", new Point(55, 55) },
                        { "map04", new Point(30, 60) },
                        { "map05", new Point(70, 15) },
                        { "map06", new Point(85, 5) },
                        { "map07", new Point(1, 70) },
                        { "map08", new Point(15, 51) },
                        { "map09", new Point(18, 12) },
                        { "map10", new Point(20, 13) }
                    };
            } else {
                startPoints = args.ToDictionary(x => Path.GetFileNameWithoutExtension(x), x => Point.Empty);
            }

            for (int i = 0; i < args.Length; ++i) {
                var name = MapNames[i];
                var path = Path.Combine(resDir, name);
                var bestName = String.Format("{0}.txt", path);
                if (File.Exists(bestName)) {
                    bestRoutes[i] = Route.FromFile(bestName, out Methods[i], out LastUpdates[i]);
                    BestScores[i] = bestRoutes[i].CalculateScore(maps[args[i]], startPoints[name]);
                } else {
                    bestRoutes[i] = null;
                    BestScores[i] = 0.0;
                }
            }
            
            if (!Directory.Exists(resDir)) {
                Directory.CreateDirectory(resDir);
            }

            Server server = WebServer ? new Server() : null;

            Action search = () => {
                var searchers = new[] { new Greedy(0.02), new Greedy(0.015), new Greedy(0.01), new Greedy(0.005) };
                
                while (server != null && !server.IsListening) Thread.Sleep(10);

                while (server == null || server.IsListening) {
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
                                var route = searcher.Search(map, startPoints[name], Map.Width * Map.Height);
                                var score = route.CalculateScore(map, startPoints[name]);

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
                                    route.ToImage(map, startPoints[name]).Save(bestName.Replace(".txt", ".png"));
                                }
                            }
                        }

                        ++r;
                    }
                }
            };

            if (WebServer) {
                new Thread(new ThreadStart(search)).Start();

                server.ResourceRootUrl = "/res";
                DefaultResourceServlet.ResourceDirectory = resDir;
                server.AddPrefix("http://+:80/");
                server.BindServletsInAssembly(Assembly.GetExecutingAssembly());
                server.Run();
            } else {
                search();
            }
        }
    }
}
