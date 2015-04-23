using System.Configuration;
using System.Diagnostics;
using System.IO;
using Sandbox.Environment.Configuration;
using Sandbox.Environment.Wrapper;

namespace Sandbox.Environment.Compiler
{
    class PythonCompiler : Compiler
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

        protected override bool UseTemporaryDirectory
        {
            get { return false; }
        }

        protected override IWrapper GetCodeWrapper(CompilerArgs args)
        {
            return new PythonWrapper(args);
        }

        public override void Compile(CompilerArgs args)
        {
            base.Compile(args);

            CreatePackageDirectory();
            SaveToFile();
            ImportLibraries();
            ValidateSource();
        }

        private void ValidateSource()
        {
            Process process = new Process();
            string sourcePath = Path.Combine(PackageDirectory, Args.PackageName + ".py");
            string pythonArgs = string.Format(@"-m py_compile ""{0}""", sourcePath);
            process.StartInfo = new ProcessStartInfo
            {
                FileName = GetPythonPath(),
                Arguments = pythonArgs,
                WorkingDirectory = Path.GetDirectoryName(sourcePath),
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            process.Start();
            string validationResult = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            if (!string.IsNullOrWhiteSpace(validationResult))
            {
                ThrowCompilationError(validationResult);
            }
        }

        private string GetPythonPath()
        {
            string configPath = ConfigurationManager.AppSettings["PythonPath"];
            return string.IsNullOrEmpty(configPath) ? "python" : configPath;
        }

        private void ImportLibraries()
        {
            foreach (string library in Args.Libraries)
            {
                File.Copy(
                    Path.Combine(ExtensionsDirectory, library, library + ".py"),
                    Path.Combine(PackageDirectory, library + ".py"));
            }
        }
    }
}
