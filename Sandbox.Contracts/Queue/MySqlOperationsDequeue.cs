using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Newtonsoft.Json;
using Sandbox.Contracts.MySql;
using Sandbox.Contracts.Types;
using Sandbox.Contracts.Types.Environment;

namespace Sandbox.Contracts.Queue
{
    class MySqlOperationsDequeue : IOperationsDequeue
    {
        readonly SandboxContext _context = SandboxContext.Instance;

        static MySqlOperationsDequeue()
        {
            Mapper.CreateMap<Input, EnvironmentInput>();
            Mapper.CreateMap<EnvironmentOutput, Output>();
        }

        public IEnumerable<EnvironmentInput> GetUnresolved()
        {
            return GetUnresolved(5);
        }

        public IEnumerable<EnvironmentInput> GetUnresolved(int count)
        {
            IEnumerable<Task> tasks = _context.Tasks
                .Where(x => !x.Resolved && !x.Processed)
                .OrderBy(x => x.Timestamp)
                .Take(count)
                .ToList();

            List<EnvironmentInput> result = new List<EnvironmentInput>();

            foreach (Task t in tasks)
            {
                t.Processed = true;
                EnvironmentInput input = JsonConvert.DeserializeObject<EnvironmentInput>(t.Input);
                input.SyncGuid = t.SyncGuid;
                result.Add(input);
            }

            _context.SaveChanges();

            return result;
        }

        public void Resolve(EnvironmentInput request, EnvironmentOutput result)
        {
            Output output = Mapper.Map<Output>(result);

            Task task = _context.Tasks
                .FirstOrDefault(x => x.SyncGuid == request.SyncGuid);

            task.Output = JsonConvert.SerializeObject(output);
            task.Resolved = true;
            _context.SaveChanges();
        }
    }
}
