using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Contracts.Queue
{
    public static class QueueManager
    {
        private static readonly string _host = "127.0.0.1";
        private static readonly int _port = 54346;

        private static object _queueHost;
        private static object _queueClient;

        public static IQueue<T> GetQueueHost<T>()
        {
            if (_queueHost == null)
            {
                _queueHost = new NetworkQueueHost<T>();
                ((NetworkQueueHost<T>)_queueHost).Create(_host, _port);
            }

            return (NetworkQueueHost<T>) _queueHost;
        }

        public static IQueue<T> GetQueueClient<T>()
        {
            if (_queueClient == null)
            {
                _queueClient = new NetworkQueueClient<T>(_host, _port);
                ((NetworkQueueClient<T>)_queueHost).Connect(_host, _port);
            }

            return (NetworkQueueClient<T>) _queueClient;
        }
    }
}
