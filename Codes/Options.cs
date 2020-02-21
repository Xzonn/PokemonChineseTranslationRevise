using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace PokemonCTR
{
    class Options
    {
        [Option('g', "game", HelpText = "游戏种类", MetaValue = "D|P|Pt|HG|SS", Required = true)]
        public string Game { get; set; }

        [Option('c', "chartable", HelpText = "码表文件", MetaValue = "FILE", Required = true)]
        public string ChartablePath { get; set; }

        [Option('m', "msg", HelpText = "文本（message）文件", MetaValue = "FILE", Required = true)]
        public string MessagePath { get; set; }

        [Option('f', "font", HelpText = "字体（font）文件", MetaValue = "FILE", Required = true)]
        public string FontPath { get; set; }
    }
}
