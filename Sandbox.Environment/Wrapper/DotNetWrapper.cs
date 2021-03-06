﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Contracts.Code;
using Sandbox.Environment.Compiler;
using System.IO;

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

                foreach (string library in _args.Libraries)
                {
                    writer.WriteLine(@"using {0};", library);
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
