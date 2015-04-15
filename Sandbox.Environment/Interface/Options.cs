using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace Sandbox.Environment.Interface
{
    internal class Options
    {
        [Option('i', "input", Required = true, HelpText = "Input file path")]
        public string InputPath { get; set; }

        [Option('f', "format", Required = true, HelpText = "Input file format, either JSON or XML")]
        public string InputFormat { get; set; }
    }
}
