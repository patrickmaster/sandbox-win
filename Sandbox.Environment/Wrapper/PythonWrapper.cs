using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Sandbox.Environment.Compiler;

namespace Sandbox.Environment.Wrapper
{
    class PythonWrapper : IWrapper
    {
        private CompilerArgs _args;

        public PythonWrapper(CompilerArgs args)
        {
            this._args = args;
        }

        public void ToStream(Stream stream)
        {
            using (TextWriter writer = new StreamWriter(stream))
            {
                if (_args.Libraries != null)
                {
                    foreach (string library in _args.Libraries)
                    {
                        DirectoryInfo dir = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), @"extensions\python\", library));
                        foreach (FileInfo fi in dir.GetFiles())
                        {
                            writer.WriteLine(@"from {0} import *", fi.Name.Remove(fi.Name.Length - 3));
                        }
                    }
                }

                writer.WriteLine();
                writer.WriteLine(@"def resolve():");
                WriteIndentedUserCode(writer);
                writer.WriteLine(@"");
                writer.WriteLine();
                writer.WriteLine(@"result = resolve()");
                writer.WriteLine(@"print(result)");
            }
        }

        private void WriteIndentedUserCode(TextWriter writer)
        {
            IEnumerable<string> lines = Regex.Split(_args.Code, "\r\n|\r|\n");

            foreach (string line in lines)
            {
                writer.WriteLine("    " + line);
            }
        }
    }
}
