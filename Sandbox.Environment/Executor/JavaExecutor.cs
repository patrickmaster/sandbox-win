using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Sandbox.Contracts;
using Sandbox.Environment.Configuration;
using System.Threading;
using Sandbox.Contracts.Types;

namespace Sandbox.Environment.Executor
{
    class JavaExecutor : Executor, IExecutor
    {
        public string Run(ExecutorArgs args)
        {
            string result;
            Process process = new Process();
            string executablePath = EnvironmentPath.GetPackageDirectory(args.Platform, args.PackageName);

            process.StartInfo = new ProcessStartInfo
            {
                FileName = ConfigurationManager.AppSettings["JavaExecutorPath"],
                Arguments = string.Format(@"-cp .;""{0}/*""; ""{1}""", executablePath, args.PackageName),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                WorkingDirectory = EnvironmentPath.GetPackageDirectory(args.Platform, args.PackageName)
            };

            process.Start();
            process.WaitForExit();
            return ReadExecutionResult(process);
        }
    }
}

