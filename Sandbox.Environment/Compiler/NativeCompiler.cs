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
        private readonly string _sourceFileName = "source.c";

        private string ExecutableFile
        {
            get { return Args.PackageName + ".exe"; }
        }
        
        public override void Compile(CompilerArgs args)
        {
            base.Compile(args);

            try
            {
                CreatePackageDirectory();
                SaveToFile();
                ImportLibraries();
                CompileSource(
                    Path.Combine(EnvironmentPath.GetTemporaryDirectory(args.Platform, args.PackageName), _sourceFileName),
                    Path.Combine(EnvironmentPath.GetTemporaryDirectory(args.Platform, args.PackageName), ExecutableFile),
                    args);
                MoveToTargetDirectory();
            }
            finally
            {
                RemoveTemporaryDirectory();
            }
        }

        private void ImportLibraries()
        {
            foreach (string library in Args.Libraries)
            {
                File.Copy(
                    Path.Combine(EnvironmentPath.GetExtensionsDirectory(Args.Platform), library, library + ".dll"),
                    Path.Combine(EnvironmentPath.GetTemporaryDirectory(Args.Platform, Args.PackageName), library + ".dll"));

                File.Copy(
                    Path.Combine(EnvironmentPath.GetExtensionsDirectory(Args.Platform), library, library + ".h"),
                    Path.Combine(EnvironmentPath.GetTemporaryDirectory(Args.Platform, Args.PackageName), library + ".h"));
            }
        }

        private void RemoveTemporaryDirectory()
        {
            Directory.Delete(EnvironmentPath.GetTemporaryDirectory(Args.Platform, Args.PackageName), true);
        }

        private void MoveToTargetDirectory()
        {
            File.Move(Path.Combine(EnvironmentPath.GetTemporaryDirectory(Args.Platform, Args.PackageName), ExecutableFile),
                Path.Combine(EnvironmentPath.GetPackageDirectory(Args.Platform, Args.PackageName), ExecutableFile));

            foreach (string library in Args.Libraries)
            {
                File.Move(Path.Combine(EnvironmentPath.GetTemporaryDirectory(Args.Platform, Args.PackageName), library + ".dll"),
                    Path.Combine(EnvironmentPath.GetPackageDirectory(Args.Platform, Args.PackageName), library + ".dll"));
            }
        }


        private void CompileSource(string sourceFilePath, string targetFile, CompilerArgs args)
        {
            Process process = new Process();
            string gccArgs = string.Format(@"""{0}"" -o ""{1}"" -L./", sourceFilePath, targetFile);
            string compilationResult;

            foreach (string library in args.Libraries)
            {
                gccArgs += " -l" + library;
            }

            process.StartInfo = new ProcessStartInfo
            {
                // DEBUG 
                //FileName = "cmd.exe",
                //Arguments = @"/k C:\MinGW\bin\g++.exe " + gccArgs,
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
                ThrowError(compilationResult);
            }
        }

        private void ThrowError(string compilationResult)
        {
            throw new CompilerException(compilationResult);
        }

        string GetCompilatorPath()
        {
            string configPath = ConfigurationManager.AppSettings["NativeCompilerPath"];
            return string.IsNullOrEmpty(configPath) ? "g++" : configPath;
        }

        private void SaveToFile()
        {
            string filePath = Path.Combine(EnvironmentPath.GetTemporaryDirectory(Args.Platform, Args.PackageName), _sourceFileName);

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                NativeExecutableWrapper wrapper = new NativeExecutableWrapper(Args);
                wrapper.ToStream(stream);
            }
        }

        private void CreatePackageDirectory()
        {
            Directory.CreateDirectory(EnvironmentPath.GetPackageDirectory(Args.Platform, Args.PackageName));
            Directory.CreateDirectory(EnvironmentPath.GetTemporaryDirectory(Args.Platform, Args.PackageName));
        }

        public override void CompileLibrary(CompilerArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
