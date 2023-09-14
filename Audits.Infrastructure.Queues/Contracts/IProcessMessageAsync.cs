namespace Audits.Infrastructure.Queues.Contracts
{
    public interface IProcessMessageAsync<T>
    {
        Task ProcessMessage(T message);
    }
}
