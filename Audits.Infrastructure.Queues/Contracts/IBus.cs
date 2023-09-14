namespace Audits.Infrastructure.Queues.Contracts
{
    public interface IBus
    {
        Task SendAsync<T>(string queue, T message, bool durable = true, bool exclusive = false, bool autoDelete = false);

        Task ReceiveAsync<T>(string queue, Action<T> onMessage, bool durable = true, bool exclusive = false, bool autoDelete = false);
        Task ReceiveAsyncFunctionAsync<T>(string queue, Func<T, Task> onMessage, bool durable = true, bool exclusive = false, bool autoDelete = false);
    }
}