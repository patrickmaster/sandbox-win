namespace Sandbox.Contracts.Queue.Model
{
    class NetworkQueueDto<T>
    {
        public NetworkQueueOperation Operation { get; set; }

        public T Argument { get; set; }
    }

    enum NetworkQueueOperation
    {
        Enqueue = 1,
        Dequeue = 2,
        Notify = 3
    }
}
