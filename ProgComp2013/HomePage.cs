﻿using System;
using WebServer;

namespace ProgComp2013
{
    [ServletURL("/")]
    public class HomePage : HTMLServlet
    {
        protected override void OnService()
        {
            Write(
                DocType("html"),
                Tag("html", lang => "en")(
                    Tag("head")(Dyn(Head)),
                    Tag("body")(Dyn(Body))
                )
            );
        }

        private void Head()
        {
            Write(
                Tag("title")("James & Max's ProgComp2013 Progress")
            );
        }

        private void Body()
        {
            for (int i = 0; i < Program.MapNames.Length; ++i) {
                Write(
                    Tag("h1")(Program.MapNames[i]),
                    EmptyTag("img", src => String.Format("res/{0}.png", Program.MapNames[i])),
                    Tag("p")(
                        "Last Update: ", Program.LastUpdates[i], Ln,
                        "Method: ", Program.Methods[i], Ln,
                        "Score: ", Program.BestScores[i]
                    )
                );
            }
        }
    }
}
