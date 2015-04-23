using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Environment.Compiler
{
    abstract class ScriptCompiler : Compiler
    {

        protected override bool UseTemporaryDirectory
        {
            get { return false; }
        }

        public sealed override void Compile(CompilerArgs args)
        {
            base.Compile(args);

            CreatePackageDirectory();
            SaveToFile();
            ImportLibraries();
            ValidateSource(Path.Combine(PackageDirectory, Args.PackageName + ".py"));
        }

        protected abstract void ValidateSource(string sourceFilePath);

        protected abstract void ImportLibraries();
    }
}
