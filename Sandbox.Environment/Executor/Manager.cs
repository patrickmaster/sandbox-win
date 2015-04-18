using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Contracts;

namespace Sandbox.Environment.Executor
{
    class Manager
    {
        public static IExecutor GetExecutor(PlatformType type)
        {
            switch (type)
            {
                case PlatformType.Native:
                    return new NativeExecutor();
                case PlatformType.DotNet:
                    return new DotNetExecutor();
                case PlatformType.Python:
                    return new PythonExecutor();
            }

            throw new NotImplementedException("No executor implementation for " + type);
        }
    }
}
