using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sandbox.Contracts
{
    public class ExecutorArgs
    {
        public string LibraryName { get; set; }

        public IDictionary<string, double> InputArguments { get; set; }

        public string TypeName { get; set; }

        public string MethodName { get; set; }

        public ExtensionType Type { get; set; }
    }

    public enum ExtensionType
    {
        DotNet = 1,
        Native = 2
    }
}
