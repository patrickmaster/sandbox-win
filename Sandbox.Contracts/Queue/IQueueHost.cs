using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Contracts.Queue
{
    public interface IQueueHost<T> : IQueue<T>
    {
        void Create(string host, int port);
    }
}
