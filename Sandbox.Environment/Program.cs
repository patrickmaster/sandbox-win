using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Cache;
using System.Threading;
using Sandbox.Contracts;
using Sandbox.Contracts.Queue;
using Sandbox.Contracts.Serialization;
using Sandbox.Contracts.Types;
using Sandbox.Contracts.Types.Environment;
using Sandbox.Environment.Compiler;
using Sandbox.Environment.Executor;
using Manager = Sandbox.Environment.Compiler.Manager;

namespace Sandbox.Environment
{
    class Program
    {
        private static readonly IOperationsDequeue Dequeue = Contracts.Manager.GetDequeue();

        static void Main(string[] args)
        {
            while (true)
            {
                IEnumerable<EnvironmentInput> requests = Dequeue.GetUnresolved();
                int sleepingTimeout = GetTimeout();

                if (requests != null && requests.Any())
                {
                    Console.WriteLine("Resolving requests...");

                    foreach (EnvironmentInput request in requests)
                    {
                        EnvironmentOutput result = Runner.RunTask(request);

                        Dequeue.Resolve(request, result);
                    }

                    Console.WriteLine("Requests resolved");
                }
                else
                {
                    Console.WriteLine("No tasks in queue. Sleeping...");
                    Thread.Sleep(sleepingTimeout);
                }
            }
        }

        private static int GetTimeout()
        {
            int value;
            if (!int.TryParse(ConfigurationManager.AppSettings["QueueSleepTime"], out value))
            {
                value = 500;
            }
            return value;
        }
    }
}
