using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Sandbox.Contracts.Types.Code;
using Sandbox.Environment.Compiler;

namespace Sandbox.Environment.Wrapper
{
    class NativeWrapper : IWrapper
    {
        private readonly CompilerArgs _args;

        public NativeWrapper(CompilerArgs args)
        {
            _args = args;
        }

        public void ToStream(Stream stream)
        {
            using (TextWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine(@"#include <stdio.h>");
                writer.WriteLine(@"#include <iostream>");

                foreach (string lib in NativeHelper.GetAllHeaders(_args.Libraries))
                {
                    writer.WriteLine(@"#include ""{0}.h""", lib);
                }

                writer.WriteLine(@"{0} resolve()", GetTypeRepresentation(_args.ReturnType));
                writer.WriteLine(@"{");
                writer.WriteLine(_args.Code);
                writer.WriteLine(@"}");

                writer.WriteLine(@"int main(int argc, char* argv[])");
                writer.WriteLine(@"{");
                writer.WriteLine(@"{0} result = resolve();", GetTypeRepresentation(_args.ReturnType));
                writer.WriteLine(@"std::cout << result;");
                writer.WriteLine(@"return 0;");
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
