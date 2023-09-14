namespace Audits.Infrastructure.Queues.Contracts
{
    public interface IProcessMessage<T>
    {
        void ProcessMessage(T message);
    }
}
