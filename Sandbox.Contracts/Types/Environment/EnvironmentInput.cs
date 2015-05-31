using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Contracts.MySql;
using Sandbox.Contracts.Types.Code;

namespace Sandbox.Contracts.Types.Environment
{
    public class EnvironmentInput
    {
        public Guid SyncGuid { get; set; }

        public PlatformType Platform { get; set; }

        public IEnumerable<Library> Libraries { get; set; }

        public string Code { get; set; }

        public VariableType ReturnType { get; set; }

        public bool UseWrapper { get; set; }
    }
}
