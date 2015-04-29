using System.Collections.Generic;
using Sandbox.Contracts.Code;

namespace Sandbox.Contracts.Types
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
