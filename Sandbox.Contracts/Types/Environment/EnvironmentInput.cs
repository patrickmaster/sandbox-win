using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Contracts.Types.Environment
{
    public class EnvironmentInput
    {
        public Guid SyncGuid { get; set; }

        public PlatformType Platform { get; set; }

        public IEnumerable<string> Libraries { get; set; }

        public string Code { get; set; }
    }
}
