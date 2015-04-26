using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Contracts.Code;
using Sandbox.Environment.Compiler;


namespace Sandbox.Environment.Wrapper
{
    class JavaWrapper : IWrapper
    {
        private CompilerArgs _args;

        public JavaWrapper(CompilerArgs args)
        {
            _args = args;
        }

        public void ToStream(Stream stream)
        {
            using (TextWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine(@"class JavaApp{");
    /*
                foreach (string library in _args.Libraries)
                {
                    writer.WriteLine(@"#include ""{0}.h""", library);
                }
    */
                writer.WriteLine(@"public static  {0} resolve()", GetTypeRepresentation(_args.ReturnType));
                writer.WriteLine(@"{");
                writer.WriteLine(_args.Code);
                writer.WriteLine(@"}");

                writer.WriteLine(@"public static void main(String[] args)");
                writer.WriteLine(@"{");
                writer.WriteLine(@"{0} result = resolve();", GetTypeRepresentation(_args.ReturnType));
                writer.WriteLine(@"System.out.println(result);");
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
