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

        protected override string LibraryExtension { get { return "dll"; } }

        protected override string ExecutableExtension { get { return "class"; } }


        protected override IWrapper GetCodeWrapper(CompilerArgs args)
        {
            return new JavaWrapper(args);
        }

        protected override void ImportLibraries()
        {
            /*
            foreach (string library in Args.Libraries)
            {
                ImportLibraryFile(library, library + ".dll");
                ImportLibraryFile(library, library + ".h");
            }
             */
        }

        protected override void MoveToTargetDirectory()
        {
            File.Move(Path.Combine(TemporaryDirectory, ExecutableFile),
                Path.Combine(PackageDirectory, ExecutableFile));
            /*
            foreach (string library in Args.Libraries)
            {
                File.Move(Path.Combine(TemporaryDirectory, library + ".dll"),
                    Path.Combine(PackageDirectory, library + ".dll"));
            }
             */
        }

        protected override void CompileSource(string sourceFilePath, string targetFilePath)
        {
            Process process = new Process();
            string javaArgs = string.Format(@"""{0}""", sourceFilePath);
            /*
            foreach (string library in Args.Libraries)
            {
                javaArgs += " -l" + library;
            }
            */
            process.StartInfo = new ProcessStartInfo
            {
                FileName = GetCompilatorPath(),
                Arguments = javaArgs,
                WorkingDirectory = Path.GetDirectoryName(sourceFilePath),
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            process.Start();
            string compilationResult = process.StandardOutput.ReadToEnd();
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