using System.Configuration;
using System.Diagnostics;
using System.IO;
using Sandbox.Environment.Configuration;
using Sandbox.Environment.Wrapper;

namespace Sandbox.Environment.Compiler
{
    class PythonCompiler : ScriptCompiler 
    {
        string MainFile { get { return Args.PackageName + ".py"; } }

        protected override string SourceExtension
        {
            get { return "py"; }
        }

        protected override string LibraryExtension
        {
            get { return "py"; }
        }

        protected override string ExecutableExtension
        {
            get { return "py"; }
        }

        protected override IWrapper GetCodeWrapper(CompilerArgs args)
        {
            return new PythonWrapper(args);
        }

        protected override void ValidateSource(string sourceFilePath)
        {
            Process process = new Process();
            MoveToTarget();
            string pythonArgs = string.Format(@"-m py_compile ""{0}""", sourceFilePath);
            process.StartInfo = new ProcessStartInfo
            {
                FileName = GetPythonPath(),
                Arguments = pythonArgs,
                WorkingDirectory = Path.GetDirectoryName(sourceFilePath),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            process.Start();
            string compilationResult = GetCompilationResult(process);
            process.WaitForExit();

            if (!string.IsNullOrWhiteSpace(compilationResult))
            {
                ThrowCompilationError(compilationResult);
            }
        }

        private string GetPythonPath()
        {
            string configPath = ConfigurationManager.AppSettings["PythonPath"];
            return string.IsNullOrEmpty(configPath) ? "python" : configPath;
        }

        private void MoveToTarget()
        {
            if (Args.Libraries != null)
            {
                foreach (string library in Args.Libraries)
                {
                    DirectoryInfo dir = new DirectoryInfo(Path.Combine(UseTemporaryDirectory ? TemporaryDirectory : PackageDirectory, library));
                    foreach (FileInfo fi in dir.GetFiles())
                    {
                        File.Copy(
                            Path.Combine(UseTemporaryDirectory ? TemporaryDirectory : PackageDirectory, library, fi.Name),
                            Path.Combine(UseTemporaryDirectory ? TemporaryDirectory : PackageDirectory, fi.Name));
                    }    
                }
            }
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
    }
}
