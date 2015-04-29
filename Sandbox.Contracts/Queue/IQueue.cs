using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Contracts.Queue;

namespace Sandbox.Contracts.Queue
{
    public interface IQueue<T>
    {
        void Enqueue(T item);

        T Dequeue();

        event EventHandler<ItemEnqueuedEventArgs<T>> ItemEnqueued;
    }
}
