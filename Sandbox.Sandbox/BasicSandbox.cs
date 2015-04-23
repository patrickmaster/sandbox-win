using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Sandbox.Contracts;
using Sandbox.Contracts.Code;
using Sandbox.Contracts.Serialization;

namespace Sandbox.Sandbox
{
    class BasicSandbox : Sandbox
    {
        public override string Run(string executablePath)
        {
            Process sandboxieProcess = new Process();
            string inputPath = GetInputPath();
            string inputFormat = GetInputFormat();
            string outputPath = GetOutputPath();

            GenerateOptions(inputPath, inputFormat);

            string args = string.Format("/wait cmd.exe /c \"\"{0}\" -i \"{1}\" -f {2} -o \"{3}\"\"", executablePath, inputPath, inputFormat, outputPath);

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = GetSandboxiePath(),
                Arguments = args
                
            };
            
            sandboxieProcess.StartInfo = startInfo;
            sandboxieProcess.Start();
            sandboxieProcess.WaitForExit();

            Process clearSandboxieProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = GetSandboxiePath(),
                    Arguments = "delete_sandbox_silent"
                }
            };

            clearSandboxieProcess.Start();
            clearSandboxieProcess.WaitForExit();

            return GetOutput(outputPath);
        }

        private string GetSandboxiePath()
        {
            string path = ConfigurationManager.AppSettings["SandboxiePath"];

            if (string.IsNullOrEmpty(path))
            {
                path = @"C:\Program Files\Sandboxie\Start.exe";
            }

            return path;
        }
    }
}