using RabbitMQ.Client;
using Audits.Infrastructure.Queues.Contracts;

namespace Audits.Infrastructure.Queues.Listeners
{
    public class BusInitiator
    {
        private static ConnectionFactory _factory = null!;
        private static IConnection _connection = null!;
        private static IModel _channel = null!;

        public static IBus CreateBus(string uri)
        {
            _factory = new ConnectionFactory { Uri = new Uri(uri), DispatchConsumersAsync = true };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            return new RabbitBus(_channel);
        }

        public static IBus CreateBus(
            string hostName,
            ushort hostPort,
            string virtualHost,
            string username,
            string password)
        {
            _factory = new ConnectionFactory
            {
                HostName = hostName,
                Port = hostPort,
                VirtualHost = virtualHost,
                UserName = username,
                Password = password,
                DispatchConsumersAsync = true
            };

            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            return new RabbitBus(_channel);
        }

    }
}