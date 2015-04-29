using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sandbox.Contracts.Queue
{
    class NetworkQueueHost<T> : IQueueHost<T>, IDisposable
    {
        private int _port;
        private string _host;

        private TcpListener _listener;
        private bool _used;

        private Thread _receivingThread;

        private ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();

        private void Bind()
        {
            IPAddress address;

            if (!IPAddress.TryParse(_host, out address))
            {
                address = IPAddress.Loopback;
            }

            _listener = new TcpListener(address, _port);

            _receivingThread = new Thread(Receive);
            _receivingThread.Start();
        }

        private void Receive()
        {
            while (true)
            {
                TcpClient client = _listener.AcceptTcpClient();
                NetworkStream stream = client.GetStream();
                byte[] lengthBytes = new byte[4];
                stream.Read(lengthBytes, 0, 4);
                int contentLength = BitConverter.ToInt32(lengthBytes, 0);
                byte[] contentBytes = new byte[contentLength];

                stream.Read(contentBytes, 4, contentLength);
                MemoryStream objectStream = new MemoryStream(contentLength);

                BinaryFormatter formatter = new BinaryFormatter();

                T item = (T) formatter.Deserialize(objectStream);

                _queue.Enqueue(item);

                EventHandler<ItemEnqueuedEventArgs<T>> handler = ItemEnqueued;

                if (handler != null)
                {
                    
                    handler(this, new ItemEnqueuedEventArgs<T>(item));
                }
            } 
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

        public void Create(string host, int port)
        {
            if (_used)
            {
                throw new InvalidOperationException("This instance of QueueHost is already in use");
            }

            _used = true;
            _host = host;
            _port = port;

            Bind();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
