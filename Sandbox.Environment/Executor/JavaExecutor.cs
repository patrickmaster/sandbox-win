using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Sandbox.Contracts;
using Sandbox.Environment.Configuration;

namespace Sandbox.Environment.Executor
{
    class JavaExecutor : IExecutor
    {
        public string Run(ExecutorArgs args)
        {
            string result;
            Process process = new Process();
            string executablePath = Path.Combine(EnvironmentPath.GetPackageDirectory(args.Platform, args.PackageName), args.PackageName);

            process.StartInfo = new ProcessStartInfo
            {
                FileName = "java",
                Arguments = string.Format("-cp . {0}", args.PackageName), //"\"" + executablePath + "\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = EnvironmentPath.GetPackageDirectory(args.Platform, args.PackageName)
            };

            process.Start();
            result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return result.Trim();
        }
    }
}

