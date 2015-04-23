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
        protected override bool UseTemporaryDirectory
        {
            get { return true; }
        }

        public sealed override void Compile(CompilerArgs args)
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

        protected abstract void MoveToTargetDirectory();

        protected abstract void CompileSource(string sourceFilePath, string destinationFilePath);

        protected abstract void ImportLibraries();


    }
}
