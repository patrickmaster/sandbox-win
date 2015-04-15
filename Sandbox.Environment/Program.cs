using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using Sandbox.Contracts;
using Sandbox.Contracts.Serialization;
using Sandbox.Environment.Executor;
using Sandbox.Environment.Interface;

namespace Sandbox.Environment
{
    class Program
    {
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        static void Main(string[] args)
        {
            Options options = GetOptions(args);
            ExecutorArgs runArgs = GetRunArgs(options);
            IExecutor executor = GetExecutor(runArgs);
            
            object result = executor.Run(runArgs);
            
            Console.WriteLine(string.Format("got dll: {0}, method name: {1}, result: {2}", runArgs.LibraryName, runArgs.MethodName, result));
        }

        private static IExecutor GetExecutor(ExecutorArgs runArgs)
        {
            switch (runArgs.Type)
            {
                case ExtensionType.DotNet:
                    return new DotNetExecutor();
                case ExtensionType.Native:
                    return new NativeExecutor();
            }

            throw new NotImplementedException();
        }
        
        private static ExecutorArgs GetRunArgs(Options options)
        {
            ISerializer serializer = Manager.GetSerializer(options.InputFormat);

            return serializer.Deserialize<ExecutorArgs>(options.InputPath);
        }

        private static Options GetOptions(string[] args)
        {
            Options options = new Options();
            using (Parser parser = new Parser())
            {
                parser.ParseArguments(args, options);
            }

            return options;
        }
    }
}
