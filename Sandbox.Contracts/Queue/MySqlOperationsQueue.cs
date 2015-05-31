using System;
using System.Linq;
using Newtonsoft.Json;
using Sandbox.Contracts.MySql;
using Sandbox.Contracts.Types;

namespace Sandbox.Contracts.Queue
{
    class MySqlOperationsQueue : IOperationsQueue
    {
        readonly SandboxContext _context = SandboxContext.Create();

        public Guid Enqueue(Input input)
        {
            Task task = new Task();
            task.SyncGuid = Guid.NewGuid();
            task.Timestamp = DateTime.UtcNow.Ticks;
            task.Input = JsonConvert.SerializeObject(input);

            _context.Tasks.Add(task);
            _context.SaveChanges();
            
            return task.SyncGuid;
        }

        public OperationStatus TryGet(Guid id, out Output output)
        {
            Task task = _context.Tasks.FirstOrDefault(x => x.SyncGuid == id);
            output = null;

            if (task != null)
            {
                if (task.Processed)
                {
                    if (task.Resolved)
                    {
                        output = JsonConvert.DeserializeObject<Output>(task.Output);
                        return OperationStatus.Resolved;
                    }
                    else
                    {
                        return OperationStatus.Processing;
                    }
                }
                else
                {
                    return OperationStatus.Queued;
                }
            }
            else
            {
                return OperationStatus.NotFound;
            }
        }
    }
}
