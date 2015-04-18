using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Environment.Compiler
{
    class CompilerException : Exception
    {
        public CompilerException()
            : base ("Compilation failed")
        {
        }

        public CompilerException(string compilationErrors)
            : this()
        {
            CompilationErrors = compilationErrors;
        }

        public string CompilationErrors { get; protected set; }
    }
}
