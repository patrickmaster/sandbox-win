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
            if (Args.Libraries != null)
            {
                foreach (string library in Args.Libraries)
                {
                    ImportLibraryFile(library);
                }
            }
        }

        protected override void MoveToTargetDirectory()
        {
            File.Move(Path.Combine(TemporaryDirectory, ExecutableFile),
                Path.Combine(PackageDirectory, ExecutableFile));

            if (Args.Libraries != null)
            {
                foreach (string library in Args.Libraries)
                {
                    DirectoryInfo dir = new DirectoryInfo(Path.Combine(TemporaryDirectory, library));
                    DirectoryInfo di = Directory.CreateDirectory(Path.Combine(PackageDirectory, library));
                    foreach (FileInfo fi in dir.GetFiles())
                    {
                        File.Move(Path.Combine(TemporaryDirectory, library, fi.Name),
                            Path.Combine(PackageDirectory, fi.Name));
                    } 
                }
            }
        }

        protected override void CompileSource(string sourceFilePath, string targetFile)
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            //parameters.CompilerOptions = "/target:library";
            parameters.GenerateExecutable = true;
            parameters.OutputAssembly = targetFile;

            if (Args.Libraries != null)
            {
                foreach (string library in Args.Libraries)
                {
                    DirectoryInfo dir = new DirectoryInfo(Path.Combine(TemporaryDirectory, library));
                    foreach (FileInfo fi in dir.GetFiles())
                    {
                        parameters.ReferencedAssemblies.Add(Path.Combine(TemporaryDirectory, library, fi.Name));
                    } 
                }
            }

            CompilerResults cr = codeProvider.CompileAssemblyFromFile(parameters, sourceFilePath);
            if (cr.Errors.Count > 0)
            {
                // THROW compilation errors.
                StringBuilder sb = new StringBuilder();

                sb.AppendFormat("Errors building {0} into {1}",
                    "exampleDll.cs", cr.PathToAssembly);
                foreach (CompilerError ce in cr.Errors)
                {
                    sb.AppendFormat("  {0}", ce);
                }

                throw new CompilerException(sb.ToString());
            }
        }

        public override void CompileLibrary(CompilerArgs args)
        {
            throw new NotImplementedException();
        }
    }
}

