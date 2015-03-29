using System;
using System.Configuration;
using System.Diagnostics;

namespace Sandbox.Sandbox
{
    class BasicSandbox : ISandbox
    {

        public void Run(string executablePath, string outputPath)
        {
            Process process = new Process();
            string args = string.Format("/wait cmd.exe /c \"\"{0}\" > \"{1}\"\"", executablePath, outputPath);
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = GetSandboxiePath(),
                Arguments = args
            };

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }

        private string GetSandboxiePath()
        {
            string path = ConfigurationManager.AppSettings["SandboxiePath"];

            if (string.IsNullOrEmpty(path))
            {
                path = @"C:\Program Files\Sandboxie\Start.exe";
            }

            CheckIfExists(path);

            return path;
        }

        private void CheckIfExists(string path)
        {
        }
    }
}