using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Contracts.Queue
{
    class NetworkQueueClient<T> : IQueueClient<T>, IDisposable
    {
        public NetworkQueueClient(string host, int port)
        {
            throw new NotImplementedException();
        }

        public void Enqueue(T item)
        {
            throw new NotImplementedException();
        }

        public T Dequeue()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<ItemEnqueuedEventArgs<T>> ItemEnqueued;

        public void Connect(string host, int port)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
