﻿using System;
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
    class NativeCompiler : BinaryCompiler
    {
        protected override string SourceExtension { get { return "cpp"; } }

        protected override string LibraryExtension { get { return "dll"; } }

        protected override string ExecutableExtension { get { return "exe"; } }


        protected override IWrapper GetCodeWrapper(CompilerArgs args)
        {
            return new NativeWrapper(args);
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

        protected override void CompileSource(string sourceFilePath, string targetFile)
        {
            Process process = new Process();
            string gccArgs = string.Format(@"""{0}"" -o ""{1}"" -static-libgcc -static-libstdc++", sourceFilePath, targetFile);

            if (Args.Libraries != null && Args.Libraries.Any())
            {
                gccArgs += " -L.";
                foreach (string header in NativeHelper.GetAllHeaders(Args.Libraries))
                {
                    gccArgs += " -l" + header;
                }
            }

            process.StartInfo = new ProcessStartInfo
            {
                FileName = GetCompilatorPath(),
                Arguments = gccArgs,
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
