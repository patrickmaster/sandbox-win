using System.Collections.Generic;
using Sandbox.Contracts;
using Sandbox.Contracts.Types;
using Sandbox.Contracts.Types.Code;

namespace Sandbox.Environment.Compiler
{
    class CompilerArgs
    {
        public PlatformType Platform { get; set; }

        public string PackageName { get; set; }

        public IEnumerable<string> Libraries { get; set; }

        public VariableType ReturnType { get; set; } // to be removed

        public string Code { get; set; }

        public bool UseWrapper { get; set; }
    }
}
