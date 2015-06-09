using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Environment.Compiler;
using System.IO;
using Sandbox.Contracts.Types.Code;

namespace Sandbox.Environment.Wrapper
{
    class DotNetWrapper : IWrapper
    {
        private CompilerArgs _args;

        public DotNetWrapper(CompilerArgs args)
        {
            _args = args;
        }

        public void ToStream(Stream stream)
        {
            using (TextWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine(@"using System;");
                writer.WriteLine(@"using System.Collections.Generic;");
                writer.WriteLine(@"using System.Text;");
                writer.WriteLine(@"using System.Threading.Tasks;");

                if (_args.Libraries != null)
                {
                    foreach (string library in _args.Libraries)
                    {
                        DirectoryInfo dir = new DirectoryInfo(Path.Combine(
                            Directory.GetCurrentDirectory(), 
                            @"extensions\dotnet\", 
                            library));
                        
                        foreach (FileInfo fi in dir.GetFiles())
                        {
                            writer.WriteLine(@"using {0};", fi.Name.Remove(fi.Name.Length - 4));
                        }
                    }
                }

                writer.WriteLine(@"namespace {0}", _args.PackageName);
                writer.WriteLine(@"{");
                writer.WriteLine(@"class Program");
                writer.WriteLine(@"{");
                writer.WriteLine(@"static public {0} resolve()", GetTypeRepresentation(_args.ReturnType));
                writer.WriteLine(@"{");
                writer.WriteLine(_args.Code);
                writer.WriteLine(@"}");

                writer.WriteLine(@"static void Main(string[] args)");
                writer.WriteLine(@"{");
                writer.WriteLine(@"{0} result = resolve();", GetTypeRepresentation(_args.ReturnType));
                writer.WriteLine(@"Console.WriteLine(result);");
                writer.WriteLine(@"}");

                writer.WriteLine(@"}");
                writer.WriteLine(@"}");
            }
        }

        private string GetTypeRepresentation(VariableType type)
        {
            switch (_args.ReturnType)
            {
                case VariableType.Void:
                    throw new InvalidOperationException("The type Void cannot be returned");
                case VariableType.Integer:
                    return "int";
                case VariableType.Double:
                    return "double";
            }

            throw new InvalidOperationException(string.Format("No suitable type representation for {0}", type));
        }
    }
}
