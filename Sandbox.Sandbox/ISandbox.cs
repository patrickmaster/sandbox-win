using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Sandbox
{
    public interface ISandbox
    {
        void Run(string executablePath, string outputPath);
    }
}
