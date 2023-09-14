using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Audits.Infrastructure.Queues.Contracts;

namespace Audits.Infrastructure.Queues.Listeners
{
    public class RabbitBus : IBus
    {
        private readonly IModel _channel;

        internal RabbitBus(IModel channel)
        {
            _channel = channel;
        }

        public async Task SendAsync<T>(string queue, T message, bool durable = true, bool exclusive = false, bool autoDelete = false)
        {
            await Task.Run(() =>
            {
                _channel.QueueDeclare(queue, durable, exclusive, autoDelete);

                var properties = _channel.CreateBasicProperties();
                properties.Persistent = false;

                var output = JsonConvert.SerializeObject(message);
                _channel.BasicPublish(string.Empty, queue, null, Encoding.UTF8.GetBytes(output));
            });
        }

        public async Task ReceiveAsync<T>(string queue, Action<T> onMessage, bool durable = true, bool exclusive = false, bool autoDelete = false)
        {
            _channel.QueueDeclare(queue, durable, exclusive, autoDelete);
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (s, e) =>
            {
                var jsonSpecified = Encoding.UTF8.GetString(e.Body.Span);
                var item = JsonConvert.DeserializeObject<T>(jsonSpecified);
                if (item != null)
                    onMessage(item);
                await Task.Yield();
            };
            _channel.BasicConsume(queue, true, consumer);
            await Task.Yield();
        }
        public async Task ReceiveAsyncFunctionAsync<T>(string queue, Func<T, Task> onMessage, bool durable = true, bool exclusive = false, bool autoDelete = false)
        {
            _channel.QueueDeclare(queue, durable, exclusive, autoDelete);
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (s, e) =>
            {
                var jsonSpecified = Encoding.UTF8.GetString(e.Body.Span);
                var item = JsonConvert.DeserializeObject<T>(jsonSpecified);
                if (item != null)
                    await onMessage(item);
                await Task.Yield();
            };
            _channel.BasicConsume(queue, true, consumer);
            await Task.Yield();
        }
    }
}