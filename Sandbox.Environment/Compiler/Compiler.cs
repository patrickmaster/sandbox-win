using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Sandbox.Contracts;
using Sandbox.Environment.Configuration;
using Sandbox.Environment.Wrapper;

namespace Sandbox.Environment.Compiler
{
    abstract class Compiler : ICompiler
    {
        protected bool Used;
        protected CompilerArgs Args;

        protected abstract string SourceExtension { get; }

        protected abstract string LibraryExtension { get; }

        protected abstract string ExecutableExtension { get; }

        protected abstract IWrapper GetCodeWrapper(CompilerArgs args);

        protected string ExecutableFile
        {
            get { return string.Format("{0}.{1}", Args.PackageName, ExecutableExtension); }
        }

        protected string SourceFile
        {
            get { return string.Format("{0}.{1}", Args.PackageName, SourceExtension); }
        }

        protected string ExtensionsDirectory
        {
            get { return EnvironmentPath.GetExtensionsDirectory(Args.Platform); }
        }

        protected string PackageDirectory
        {
            get { return EnvironmentPath.GetPackageDirectory(Args.Platform, Args.PackageName); }
        }

        public virtual void Compile(CompilerArgs args)
        {
            if (Used)
            {
                throw new InvalidOperationException("This compiler instance has already been used");
            }

            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            if (string.IsNullOrWhiteSpace(args.PackageName))
            {
                throw new ArgumentException("The PackageName property cannot be empty");
            }

            Args = args;

            Used = true;
        }

        public virtual void CompileLibrary(CompilerArgs args)
        {
        }

        protected void CreatePackageDirectory()
        {
            RemovePackageDirectoryIfExists();
            Directory.CreateDirectory(PackageDirectory);
        }

        protected void RemovePackageDirectoryIfExists()
        {
            if (Directory.Exists(PackageDirectory))
            {
                Directory.Delete(PackageDirectory, true);
            }
        }

        protected void ThrowCompilationError(string compilationResult)
        {
            throw new CompilerException(compilationResult);
        }

        protected void SaveToFile()
        {
            string filePath = Path.Combine(PackageDirectory, SourceFile);

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                if (Args.UseWrapper)
                {
                    IWrapper wrapper = GetCodeWrapper(Args);
                    wrapper.ToStream(stream);
                }
                else
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(Args.Code);
                    }
                }
            }
        }

        protected void ImportLibraryFile(string library)
        {
            DirectoryInfo dir = new DirectoryInfo(Path.Combine(ExtensionsDirectory, library));
            
            foreach (FileInfo fi in dir.GetFiles())
            {
                File.Copy(
                    Path.Combine(ExtensionsDirectory, library, fi.Name), 
                    Path.Combine(PackageDirectory, fi.Name));
            }    
        }
        protected static string GetCompilationResult(Process process)
        {
            string compilationResult = process.StandardError.ReadToEnd();
            if (string.IsNullOrWhiteSpace(compilationResult))
            {
                compilationResult = process.StandardOutput.ReadToEnd();
            }
            return compilationResult;
        }
    }
}
