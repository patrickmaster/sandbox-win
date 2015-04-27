using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Contracts;
using Sandbox.Contracts.Code;
using Sandbox.Contracts.Serialization;

namespace Sandbox.Sandbox
{
    abstract class Sandbox : ISandbox
    {
        protected bool AttachDebugger;

        public abstract EnvironmentOutput Run(EnvironmentInput input);

        protected void GenerateOptions(string inputPath, string inputFormat, EnvironmentInput input)
        {

            ISerializer serializer = Contracts.Serialization.Manager.GetSerializer(inputFormat);

            serializer.Serialize(input, inputPath);
        }

        protected string GetEnvironmentExecutable()
                {
            string path = ConfigurationManager.AppSettings["EnvironmentPath"];

            if (string.IsNullOrEmpty(path))
            {
                path = @"sandbox\Sandbox.Environment.exe";
            }

            return path;
        }

        protected EnvironmentOutput GetOutput(string outputPath, string format)
        {
            string result;

            ISerializer serializer = Contracts.Serialization.Manager.GetSerializer(format);
            EnvironmentOutput output = serializer.Deserialize<EnvironmentOutput>(outputPath);

            File.Delete(outputPath);

            return output;
        }

        protected string GetOutputPath()
        {
            string path = ConfigurationManager.AppSettings["OutputPath"];

            if (string.IsNullOrEmpty(path))
            {
                path = @"sandbox\env_output.txt";
            }

            return path;
        }

        protected string GetInputFormat()
        {
            return "json";
        }

        protected string GetInputPath()
        {
            string path = ConfigurationManager.AppSettings["InputPath"];

            if (string.IsNullOrEmpty(path))
            {
                path = @"sandbox\env_input.dat";
            }

            return path;
        }

    }
}
