using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Contracts;
using Sandbox.Environment.Configuration;
using Sandbox.Environment.Wrapper;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;

namespace Sandbox.Environment.Compiler
{
    class DotNetCompiler : BinaryCompiler
    {
        protected override string SourceExtension { get { return "cs"; } }

        protected override string LibraryExtension { get { return "dll"; } }

        protected override string ExecutableExtension { get { return "exe"; } }


        protected override IWrapper GetCodeWrapper(CompilerArgs args)
        {
            return new DotNetWrapper(args);
        }

        protected override void ImportLibraries()
        {
            foreach (string library in Args.Libraries)
            {
                ImportLibraryFile(library, library + ".dll");
            }
        }

        protected override void MoveToTargetDirectory()
        {
            File.Move(Path.Combine(TemporaryDirectory, ExecutableFile),
                Path.Combine(PackageDirectory, ExecutableFile));

            foreach (string library in Args.Libraries)
            {
                File.Move(Path.Combine(TemporaryDirectory, library + ".dll"),
                    Path.Combine(PackageDirectory, library + ".dll"));
            }
        }

        protected override void CompileSource(string sourceFilePath, string targetFile)
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            //parameters.CompilerOptions = "/target:library";
            parameters.GenerateExecutable = true;
            parameters.OutputAssembly = targetFile;
            foreach (string library in Args.Libraries)
            {
                parameters.ReferencedAssemblies.Add(Path.Combine(TemporaryDirectory, library + ".dll"));
            }
            CompilerResults cr = codeProvider.CompileAssemblyFromFile(parameters, sourceFilePath);
            if (cr.Errors.Count > 0)
            {
                // Display compilation errors.
                Console.WriteLine("Errors building {0} into {1}",
                    "exampleDll.cs", cr.PathToAssembly);
                foreach (CompilerError ce in cr.Errors)
                {
                    Console.WriteLine("  {0}", ce.ToString());
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Source {0} built into {1} successfully.",
                    sourceFilePath, cr.PathToAssembly);
                Console.WriteLine("{0} temporary files created during the compilation.",
                    parameters.TempFiles.Count.ToString());
            }
        }

        public override void CompileLibrary(CompilerArgs args)
        {
            throw new NotImplementedException();
        }
    }
}

