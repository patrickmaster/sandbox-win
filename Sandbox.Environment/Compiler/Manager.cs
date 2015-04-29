using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Contracts;
using Sandbox.Contracts.Types;

namespace Sandbox.Environment.Compiler
{
    class Manager
    {
        public static ICompiler GetCompiler(PlatformType type)
        {
            switch (type)
            {
                case PlatformType.Native:
                    return new NativeCompiler();
                case PlatformType.Python:
                    return new PythonCompiler();
                case PlatformType.Java:
                    return new JavaCompiler();
                case PlatformType.DotNet:
                    return new DotNetCompiler();
            }

            throw new NotImplementedException("No compiler implementation for " + type);
        }
    }
}
