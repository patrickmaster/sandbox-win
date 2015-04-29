using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Contracts;
using Sandbox.Contracts.Types;

namespace Sandbox.Sandbox
{
    public interface ISandbox
    {
        EnvironmentOutput Run(EnvironmentInput input);
    }
}
