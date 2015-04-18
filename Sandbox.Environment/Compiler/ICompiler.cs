using Sandbox.Contracts;

namespace Sandbox.Environment.Compiler
{
    interface ICompiler
    {
        void Compile(CompilerArgs args);
    
        void CompileLibrary(CompilerArgs args);
    }
}