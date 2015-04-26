using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Sandbox.Contracts;
using Sandbox.Environment.Configuration;
using System.Threading;

namespace Sandbox.Environment.Executor
{
    class JavaExecutor : IExecutor
    {
        public string Run(ExecutorArgs args)
        {
            string result;
            Process process = new Process();
            string executablePath = EnvironmentPath.GetPackageDirectory(args.Platform, args.PackageName);

            process.StartInfo = new ProcessStartInfo
            {
                FileName = "java",
                Arguments = string.Format("-cp .;{0}/*; {1}",executablePath, args.PackageName), //"\"" + executablePath + "\"",
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

