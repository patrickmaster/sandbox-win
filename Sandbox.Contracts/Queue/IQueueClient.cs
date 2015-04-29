namespace Sandbox.Contracts.Queue
{
    public interface IQueueClient<T> : IQueue<T>
    {
        void Connect(string host, int port);
    }
}