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
    class NativeCompiler : Compiler
    {
        protected override string SourceExtension { get { return "cpp"; } }
        
        protected override string LibraryExtension { get { return "dll"; } }

        protected override string ExecutableExtension { get { return "exe"; } }

        protected override bool UseTemporaryDirectory
        {
            get { return true; }
        }

        protected override IWrapper GetCodeWrapper(CompilerArgs args)
        {
            return new NativeExecutableCodeWrapper(args);
        }

        public override void Compile(CompilerArgs args)
        {
            base.Compile(args);

            try
            {
                CreatePackageDirectory();
                CreateTemporaryDirectory();
                SaveToFile();
                ImportLibraries();
                CompileSource(
                    Path.Combine(TemporaryDirectory, SourceFile),
                    Path.Combine(TemporaryDirectory, ExecutableFile));
                MoveToTargetDirectory();
            }
            catch (CompilerException e)
            {
                RemovePackageDirectoryIfExists();
                throw;
            }
            finally
            {
                RemoveTemporaryDirectoryIfExists();
            }
        }

        private void ImportLibraries()
        {
            foreach (string library in Args.Libraries)
            {
                File.Copy(
                    Path.Combine(ExtensionsDirectory, library, library + ".dll"),
                    Path.Combine(TemporaryDirectory, library + ".dll"));

                File.Copy(
                    Path.Combine(ExtensionsDirectory, library, library + ".h"),
                    Path.Combine(TemporaryDirectory, library + ".h"));
            }
        }

        protected void MoveToTargetDirectory()
        {
            File.Move(Path.Combine(TemporaryDirectory, ExecutableFile),
                Path.Combine(PackageDirectory, ExecutableFile));

            foreach (string library in Args.Libraries)
            {
                File.Move(Path.Combine(TemporaryDirectory, library + ".dll"),
                    Path.Combine(PackageDirectory, library + ".dll"));
            }
        }

        private void CompileSource(string sourceFilePath, string targetFile)
        {
            Process process = new Process();
            string gccArgs = string.Format(@"""{0}"" -o ""{1}"" -L./", sourceFilePath, targetFile);
            string compilationResult;

            foreach (string library in Args.Libraries)
            {
                gccArgs += " -l" + library;
            }

            process.StartInfo = new ProcessStartInfo
            {
                FileName = GetCompilatorPath(),
                Arguments = gccArgs,
                WorkingDirectory = Path.GetDirectoryName(sourceFilePath),
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            process.Start();
            compilationResult = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            if (!string.IsNullOrWhiteSpace(compilationResult))
            {
                ThrowCompilationError(compilationResult);
            }
        }


        string GetCompilatorPath()
        {
            string configPath = ConfigurationManager.AppSettings["NativeCompilerPath"];
            return string.IsNullOrEmpty(configPath) ? "g++" : configPath;
        }

        public override void CompileLibrary(CompilerArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
