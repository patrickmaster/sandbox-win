using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Contracts.Code;

namespace Sandbox.Contracts
{
    public class EnvironmentInput
    {
        public PlatformType Platform { get; set; }

        public string PackageName { get; set; }

        public IEnumerable<string> Libraries { get; set; }

        public IEnumerable<InputArgument> InputArguments { get; set; }

        public VariableType ReturnType { get; set; }

        public string Code { get; set; }

        public bool AttachDebugger { get; set; }
    }
}
