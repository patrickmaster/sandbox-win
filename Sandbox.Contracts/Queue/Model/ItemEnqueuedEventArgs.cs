using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Sandbox.Contracts.Queue
{
    public class ItemEnqueuedEventArgs<T> : EventArgs
    {
        public ItemEnqueuedEventArgs(T item)
        {
            Item = item;
        }

        public T Item { get; private set; } 
    }
}
