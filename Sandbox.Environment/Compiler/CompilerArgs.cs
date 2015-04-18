using System.Collections.Generic;
using Sandbox.Contracts;
using Sandbox.Contracts.Code;

namespace Sandbox.Environment.Compiler
{
    class CompilerArgs
    {
        public PlatformType Platform { get; set; }

        public string PackageName { get; set; }

        public IEnumerable<string> Libraries { get; set; }

        public IEnumerable<InputArgument> InputArguments { get; set; } 

        public VariableType ReturnType { get; set; }

        public string Code { get; set; }
    }
}
