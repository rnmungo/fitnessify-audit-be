using Microsoft.Extensions.Hosting;

namespace Audits.Infrastructure.Queues.Contracts
{
    public interface IBasicWorker<T> : IHostedService
    {
    }
}
