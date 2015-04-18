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
                foreach (string library in _args.Libraries)
                {
                    writer.WriteLine(@"from {0} import *", library);
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
