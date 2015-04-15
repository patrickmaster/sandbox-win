using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Sandbox.Contracts;

namespace Sandbox.Environment.Executor
{
    class DotNetExecutor : IExecutor
    {
        public object Run(ExecutorArgs runArgs)
        {

            string dllContainerDir = GetDllContainer();
            string path = Path.Combine(dllContainerDir, runArgs.LibraryName);
            
            Assembly dll = Assembly.LoadFile(path);

            object instance = dll.CreateInstance(runArgs.TypeName);
            Type instanceType = dll.GetType(runArgs.TypeName);
            object result = instanceType.GetMethod(runArgs.MethodName, BindingFlags.Public | BindingFlags.Static)
                .Invoke(null, runArgs.InputArguments.Values.Select(x => (object)x).ToArray());

            return result; 
        }

        private static string GetDllContainer()
        {
            return Path.Combine(Program.AssemblyDirectory, "extensions");
        }
    }
}
