using Sandbox.Contracts;
using Sandbox.Contracts.Types;

namespace Sandbox.Environment.Executor
{
    interface IExecutor
    {
        string Run(ExecutorArgs args);
    }
}