using Sandbox.Contracts;

namespace Sandbox.Environment.Executor
{
    interface IExecutor
    {
        string Run(ExecutorArgs args);
    }
}