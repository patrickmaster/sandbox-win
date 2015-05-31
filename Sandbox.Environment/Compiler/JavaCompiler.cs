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

namespace Sandbox.Environment.Compiler
{
    class JavaCompiler : BinaryCompiler
    {
        protected override string SourceExtension { get { return "java"; } }

        protected override string LibraryExtension { get { return "jar"; } }

        protected override string ExecutableExtension { get { return "class"; } }


        protected override IWrapper GetCodeWrapper(CompilerArgs args)
        {
            return new JavaWrapper(args);
        }

        protected override void ImportLibraries()
        {
            if (Args.Libraries != null)
            {
                foreach (string library in Args.Libraries)
                {
                    ImportLibraryFile(library, library + ".jar");
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
                    File.Move(Path.Combine(TemporaryDirectory, library + ".jar"),
                        Path.Combine(PackageDirectory, library + ".jar"));
                }
            }
        }

        protected override void CompileSource(string sourceFilePath, string targetFilePath)
        {
            Process process = new Process();
            string javaArgs = string.Format(@"-cp .;""{0}\*""; ""{1}""", TemporaryDirectory, sourceFilePath);
          
            process.StartInfo = new ProcessStartInfo
            {
                FileName = GetCompilatorPath(),
                Arguments = javaArgs,
                WorkingDirectory = Path.GetDirectoryName(sourceFilePath),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            process.Start();
            string compilationResult = process.StandardError.ReadToEnd();
            if (string.IsNullOrWhiteSpace(compilationResult))
            {
                compilationResult = process.StandardOutput.ReadToEnd();
            }
            process.WaitForExit();

            if (!string.IsNullOrWhiteSpace(compilationResult))
            {
                ThrowCompilationError(compilationResult);
            }
        }


        string GetCompilatorPath()
        {
            string configPath = ConfigurationManager.AppSettings["JavaCompilerPath"];
            return string.IsNullOrEmpty(configPath) ? "javac" : configPath;
        }

        public override void CompileLibrary(CompilerArgs args)
        {
            throw new NotImplementedException();
        }
    }
}