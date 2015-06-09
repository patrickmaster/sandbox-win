using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Environment.Compiler
{
    internal abstract class BinaryCompiler : Compiler
    {
        public sealed override void Compile(CompilerArgs args)
        {
            base.Compile(args);

            try
            {
                CreatePackageDirectory();
                SaveToFile();
                ImportLibraries();
                CompileSource(
                    Path.Combine(PackageDirectory, SourceFile),
                    Path.Combine(PackageDirectory, ExecutableFile));
            }
            catch (CompilerException e)
            {
                //RemovePackageDirectoryIfExists();
                throw;
            }
        }

        protected abstract void CompileSource(string sourceFilePath, string destinationFilePath);

        protected abstract void ImportLibraries();


    }
}
