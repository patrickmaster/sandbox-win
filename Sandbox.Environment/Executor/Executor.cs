using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Environment.Compiler;

namespace Sandbox.Environment.Executor
{
    abstract class Executor
    {
        protected string ReadExecutionResult(Process process)
        {
            string error = process.StandardError.ReadToEnd();
            if (!string.IsNullOrWhiteSpace(error))
            {
                throw new CompilerException(error);
            }

            return process.StandardOutput.ReadToEnd().Trim();
        }
    }
}
