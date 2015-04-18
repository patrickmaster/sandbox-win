using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Contracts;
using Sandbox.Environment.Configuration;

namespace Sandbox.Environment.Executor
{
    class PythonExecutor : IExecutor
    {
        public string Run(ExecutorArgs args)
        {
            Process process = new Process();
            string scriptPath = Path.Combine(EnvironmentPath.GetPackageDirectory(args.Platform, args.PackageName), args.PackageName + ".py");

            process.StartInfo = new ProcessStartInfo
            {
                FileName = GetPythonPath(),
                Arguments = "\"" + scriptPath + "\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = Path.GetDirectoryName(scriptPath)
            };

            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return result.Trim();
        }

        private string GetPythonPath()
        {
            string configPath = ConfigurationManager.AppSettings["PythonPath"];
            return string.IsNullOrEmpty(configPath) ? "python" : configPath;
        }
    }
}
