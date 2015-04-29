using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Contracts;
using Sandbox.Contracts.Types;

namespace Sandbox.Sandbox
{
    class NoSandbox : Sandbox
    {
        public NoSandbox(bool attachDebugger)
        {
            AttachDebugger = attachDebugger;
        }

        public override EnvironmentOutput Run(EnvironmentInput input)
        {
            Process environmentProcess = new Process();
            string environmentPath = GetEnvironmentExecutable();
            string inputPath = GetInputPath();
            string format = GetInputFormat();
            string outputPath = GetOutputPath();

            GenerateOptions(inputPath, format, input);

            string args = string.Format("-i \"{0}\" -f {1} -o \"{2}\"", inputPath, format, outputPath);

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = environmentPath,
                Arguments = args
            };

            environmentProcess.StartInfo = startInfo;
            environmentProcess.Start();
            environmentProcess.WaitForExit();

            return GetOutput(outputPath, format);
        }
    }
}
