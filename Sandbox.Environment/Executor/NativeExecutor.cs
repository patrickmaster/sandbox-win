using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Sandbox.Contracts;
using Sandbox.Environment.Configuration;

namespace Sandbox.Environment.Executor
{
    class NativeExecutor : IExecutor
    {
        public string Run(ExecutorArgs args)
        {
            string result;
            Process process = new Process();
            string executablePath = Path.Combine(EnvironmentPath.GetPackageDirectory(args.Platform, args.PackageName), args.PackageName + ".exe");
            
            process.StartInfo = new ProcessStartInfo
            {
                FileName = executablePath,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            process.Start();
            result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return result;
        }
    }
}
