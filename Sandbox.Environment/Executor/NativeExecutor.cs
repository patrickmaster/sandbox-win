using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Sandbox.Contracts;
using Sandbox.Contracts.Types;
using Sandbox.Environment.Configuration;

namespace Sandbox.Environment.Executor
{
    class NativeExecutor : Executor, IExecutor
    {
        public string Run(ExecutorArgs args)
        {
            Process process = new Process();
            string executablePath = Path.Combine(EnvironmentPath.GetPackageDirectory(args.Platform, args.PackageName), args.PackageName + ".exe");
            
            process.StartInfo = new ProcessStartInfo
            {
                FileName = executablePath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            process.StartInfo.EnvironmentVariables["Path"] += 
                ";" + Path.GetDirectoryName(ConfigurationManager.AppSettings["NativeCompilerPath"]);

            process.Start();
            process.WaitForExit();
            return ReadExecutionResult(process);
        }
    }
}
