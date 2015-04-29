using System;
using CommandLine;
using Sandbox.Contracts;
using Sandbox.Contracts.Serialization;
using Sandbox.Contracts.Types;
using Sandbox.Environment.Compiler;
using Sandbox.Environment.Executor;
using Sandbox.Environment.Interface;
using Manager = Sandbox.Environment.Compiler.Manager;

namespace Sandbox.Environment
{
    class Program
    {
        private static Options _options;
        static void Main(string[] args)
        {
            try
            {
                _options = GetOptions(args);
                
                EnvironmentInput environmentInput = GetEnvironmentInput();
                ICompiler compiler = Manager.GetCompiler(environmentInput.Platform);
                IExecutor executor = Executor.Manager.GetExecutor(environmentInput.Platform);
                CompilerArgs compilerArgs = GetCompilerArgs(environmentInput);
                ExecutorArgs executorArgs = GetExecutorArgs(environmentInput);

                if (environmentInput.AttachDebugger)
                {
                    try
                    {
                        System.Diagnostics.Debugger.Launch();
                    }
                    catch
                    {
                    }
                }

                compiler.Compile(compilerArgs);

                string result = executor.Run(executorArgs);

                ReturnOutput(result);
            }
            catch (Exception e)
            {
                ReturnOutput(e);
            }
        }

        private static ExecutorArgs GetExecutorArgs(EnvironmentInput environmentInput)
        {
            return new ExecutorArgs
            {
                PackageName = environmentInput.PackageName,
                Platform = environmentInput.Platform
            };
        }

        private static CompilerArgs GetCompilerArgs(EnvironmentInput environmentInput)
        {
            return new CompilerArgs
            {
                PackageName = environmentInput.PackageName,
                Libraries = environmentInput.Libraries,
                InputArguments = environmentInput.InputArguments,
                ReturnType = environmentInput.ReturnType,
                Code = environmentInput.Code,
                Platform = environmentInput.Platform
            };
        }

        private static void ReturnOutput(object result)
        {
            EnvironmentOutput output = new EnvironmentOutput();

            if (result is Exception)
            {
                output.Exception = (Exception)result;
            }
            else
            {
                output.Result = result.ToString();
            }

            Contracts.Serialization.Manager.GetSerializer(_options.Format).Serialize(output, _options.OutputPath);
        }

        private static EnvironmentInput GetEnvironmentInput()
        {
            ISerializer serializer = Contracts.Serialization.Manager.GetSerializer(_options.Format);
            EnvironmentInput args = serializer.Deserialize<EnvironmentInput>(_options.InputPath);

            return args;
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
