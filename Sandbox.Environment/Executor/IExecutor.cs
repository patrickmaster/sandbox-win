using Sandbox.Contracts;

namespace Sandbox.Environment.Executor
{
    interface IExecutor
    {
        object Run(ExecutorArgs args);
    }
}