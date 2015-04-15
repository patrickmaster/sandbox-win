using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Sandbox.Contracts;
using Sandbox.Contracts.Serialization;

namespace Sandbox.Sandbox
{
    class BasicSandbox : ISandbox
    {

        public string Run(string executablePath)
        {
            Process sandboxieProcess = new Process();
            string inputPath = GetInputPath();
            string inputFormat = GetInputFormat();
            string outputPath = GetOutputPath();

            GenerateOptions(inputPath, inputFormat);

            string args = string.Format("/wait cmd.exe /c \"\"{0}\" -i {1} -f {2} > \"{3}\"\"", executablePath, inputPath, inputFormat, outputPath);

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
            //File.Delete(inputPath);

            return GetOutput(outputPath);
        }

        private void GenerateOptions(string inputPath, string inputFormat)
        {
            ExecutorArgs args = new ExecutorArgs
            {
                LibraryName = "ExampleExtension.dll",
                InputArguments = new Dictionary<string, double> { { "a", 15D }, { "b", 13D } },
                MethodName = "Add",
                TypeName = "ExampleExtension.Calculator",
                Type = ExtensionType.DotNet
            };

            ISerializer serializer = Contracts.Serialization.Manager.GetSerializer(inputFormat);

            serializer.Serialize(args, inputPath);
        }

        private string GetInputFormat()
        {
            return "json";
        }

        private string GetInputPath()
        {
            return @"sandbox\input.dat";
        }

        private string GetOutput(string outputPath)
        {
            string result;
            using (FileStream fileStream = new FileStream(outputPath, FileMode.Open))
            {
                StreamReader streamReader = new StreamReader(fileStream);
                result = streamReader.ReadToEnd();
            }

            File.Delete(outputPath);
            
            return result;
        }

        private string GetOutputPath()
        {
            string path = ConfigurationManager.AppSettings["OutputPath"];

            if (string.IsNullOrEmpty(path))
            {
                path = @"sandbox\output.txt";
            }

            return path;
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