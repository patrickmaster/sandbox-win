using System;
using System.Linq;
using Newtonsoft.Json;
using Sandbox.Contracts.MySql;
using Sandbox.Contracts.Types;

namespace Sandbox.Contracts.Queue
{
    class MySqlOperationsQueue : IOperationsQueue
    {
        readonly SandboxContext _context = SandboxContext.Instance;

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

        public bool TryGet(Guid id, out Output output)
        {
            Task task = _context.Tasks.FirstOrDefault(x => x.SyncGuid == id);

            if (task.Resolved)
            {
                output = JsonConvert.DeserializeObject<Output>(task.Output);
                return true;
            }
            else
            {
                output = null;
                return false;
            }
        }
    }
}
