using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NARCFileReadingDLL;

namespace PokemonCTR
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
            {
                switch (o.Game)
                {
                    case "Pt":
                        break;
                    default:
                        break;
                }
                CharTable charTable = new CharTable(o.ChartablePath);
                Narc msg = new Narc(o.MessagePath);
                Narc font = new Narc(o.FontPath);
                Text text = new Text(msg, charTable);
            });
        }
    }
}
