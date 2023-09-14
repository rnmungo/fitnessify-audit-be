using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Audits.Infrastructure.Queues.Contracts;
using Audits.Infrastructure.Queues.Listeners;

namespace Audits.Infrastructure.Queues.Workers
{
    public class BasicWorker<T> : BackgroundService, IBasicWorker<T>
    {

        private readonly IBus _busControl;
        private readonly IProcessMessage<T> _processMessage;
        private readonly string _queueName;
        public BasicWorker(IConfiguration configuration, IProcessMessage<T> processMessage)
        {
            _busControl = BusInitiator.CreateBus(configuration["RabbitMQ:ConnectionString"] ?? "localhost");
            _queueName = configuration["RabbitMQ:QueueName"] ?? "NonInitialized";
            _processMessage = processMessage;
        }
        public BasicWorker(IConfiguration configuration, IProcessMessage<T> processMessage, string hostName)
        {
            _busControl = BusInitiator.CreateBus(hostName);
            _queueName = configuration["RabbitMQ:QueueName"] ?? "NonInitialized";
            _processMessage = processMessage;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _busControl.ReceiveAsync<T>(_queueName, x =>
            {
                Task.Run(() => { _processMessage.ProcessMessage(x); }, stoppingToken);
            });
        }
    }
}
