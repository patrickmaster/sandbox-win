using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Polenter.Serialization;
using Sandbox.Contracts.MySql;
using Sandbox.Contracts.Types;
using Sandbox.Contracts.Types.Environment;

namespace Sandbox.Contracts.Queue
{
    class MySqlOperationsQueue : IOperationsQueue
    {
        readonly SandboxContext _context = SandboxContext.Create();

        public Guid Enqueue(Input input)
        {
            SqlTask task = new SqlTask();
            task.SyncGuid = Guid.NewGuid();
            task.Timestamp = DateTime.UtcNow.Ticks;
            task.Input = JsonConvert.SerializeObject(input);

            _context.Tasks.Add(task);
            _context.SaveChanges();
            
            return task.SyncGuid;
        }

        public OperationStatus TryGet(Guid id, out Output output)
        {
            SqlTask task = _context.Tasks.FirstOrDefault(x => x.SyncGuid == id);
            output = null;

            if (task != null)
            {
                if (task.Processed)
                {
                    if (task.Resolved)
                    {
                        EnvironmentOutput environmentOutput = JsonConvert.DeserializeObject<EnvironmentOutput>(task.Output);
                        output = new Output
                        {
                            Result = environmentOutput.Result,
                            Error = environmentOutput.Exception != null ? environmentOutput.Exception.Message : null
                        };

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
