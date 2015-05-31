using System;
using Sandbox.Contracts.Types;
using Sandbox.Contracts.Types.Code;
using Sandbox.Contracts.Types.Environment;
using Sandbox.Environment.Compiler;
using Sandbox.Environment.Executor;
using Manager = Sandbox.Environment.Compiler.Manager;

namespace Sandbox.Environment
{
    class Runner
    {
        public static EnvironmentOutput RunTask(EnvironmentInput input)
        {
            try
            {
                string name = "_" + input.SyncGuid.ToString().Replace('-', '_');
                ICompiler compiler = Manager.GetCompiler(input.Platform);
                IExecutor executor = Executor.Manager.GetExecutor(input.Platform);
                CompilerArgs compilerArgs = GetCompilerArgs(input, name);
                ExecutorArgs executorArgs = GetExecutorArgs(input, name);

                compiler.Compile(compilerArgs);
                string result = executor.Run(executorArgs);

                return ReturnOutput(result);
            }
            catch (Exception e)
            {
                return ReturnOutput(e);
            }
        }

        private static ExecutorArgs GetExecutorArgs(EnvironmentInput input, string name)
        {
            return new ExecutorArgs
            {
                PackageName = name,
                Platform = input.Platform
            };
        }

        private static CompilerArgs GetCompilerArgs(EnvironmentInput input, string name)
        {
            return new CompilerArgs
            {
                PackageName = name,
                Libraries = input.Libraries,
                ReturnType = VariableType.Integer,
                Code = input.Code,
                Platform = input.Platform
            };
        }

        private static EnvironmentOutput ReturnOutput(object result)
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

            return output;
        }
    }
}
