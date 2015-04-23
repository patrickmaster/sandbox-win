using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Sandbox
{
    class NoSandbox : Sandbox
    {
        public NoSandbox(bool attachDebugger)
        {
            AttachDebugger = attachDebugger;
        }

        public override string Run(string executablePath)
        {
            Process environmentProcess = new Process();

            string inputPath = GetInputPath();
            string inputFormat = GetInputFormat();
            string outputPath = GetOutputPath();

            GenerateOptions(inputPath, inputFormat);

            string args = string.Format("-i \"{0}\" -f {1} -o \"{2}\"", inputPath, inputFormat, outputPath);

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = executablePath,
                Arguments = args
            };

            environmentProcess.StartInfo = startInfo;
            environmentProcess.Start();
            environmentProcess.WaitForExit();

            return GetOutput(outputPath);
        }
    }
}
