using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Sandbox
{
    public static class Manager
    {
        public static ISandbox GetSandbox(bool useSandbox)
        {
            if (useSandbox)
            {
                return new BasicSandbox();
            }
            else
            {
                return new NoSandbox(false);
            }
        }
    }
}
