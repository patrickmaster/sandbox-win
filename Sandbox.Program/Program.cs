using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Contracts;
using Sandbox.Contracts.Types;
using Sandbox.Contracts.Types.Code;

namespace Sandbox.Program
{
    class Program
    {
        static void Main(string[] args)
        {
            ISandbox sandbox = Manager.GetSandbox(false);

            EnvironmentOutput output = sandbox.Run(GetEnvironmentInput());

            Console.WriteLine("result is: " + output.Result);
            Console.WriteLine("exception is: " + (output.Exception == null ? "nothing" : output.Exception.Message));
            Console.WriteLine("press enter to exit");
            Console.ReadLine();
        }

        static EnvironmentInput GetEnvironmentInput()
        {

            EnvironmentInput args = new EnvironmentInput
            {
                AttachDebugger = false,
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

                //Platform = PlatformType.DotNet,
                //PackageName = "DotNetTest",
                //ReturnType = VariableType.Double,
                //Libraries = new List<string>
                //{
                //    "exampleDll"
                //},
                //Code = "exampleDllClass A = new exampleDllClass(); return A.Equation();"

            };

            return args;
        }
    }
}
