using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Sandbox
{
    public static class Manager
    {
        public static ISandbox GetSandbox()
        {
            return new BasicSandbox();
        }
    }
}
