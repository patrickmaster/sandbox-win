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

        public abstract string Run(string executablePath);

        protected void GenerateOptions(string inputPath, string inputFormat)
        {
            EnvironmentInput args = new EnvironmentInput
            {
                AttachDebugger = AttachDebugger,
                
                Platform = PlatformType.Native,
                PackageName = "exampel",
                ReturnType = VariableType.Integer,
                Libraries = new List<string>
                {
                    "mydll"
                },
                Code = "return add(123,6);"
                  

                //Platform = PlatformType.Python,
                //PackageName = "pythonexample",
                //ReturnType = VariableType.Integer,
                //Libraries = new List<string>
                //{
                //    "equation"
                //},
                //Code = "return equation()"
                /*
                Platform = PlatformType.Java,
                PackageName = "JavaApp",
                ReturnType = VariableType.Integer,
                Libraries = new List<string>
                {
                   // "mydll"
                },
                Code = "return 1+1;"*/
            };

            ISerializer serializer = Contracts.Serialization.Manager.GetSerializer(inputFormat);

            serializer.Serialize(args, inputPath);
        }

        protected string GetOutput(string outputPath)
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
            return @"sandbox\env_input.dat";
        }

    }
}
