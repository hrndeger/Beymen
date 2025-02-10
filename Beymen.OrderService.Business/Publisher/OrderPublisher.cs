using Beymen.Service.Message;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Beymen.OrderService.Business.Publisher
{
    public sealed class OrderPublisher : IMessageQueuePublisher, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string _exchangeName = "order-direct";

        public OrderPublisher(IConnection connection, IConfiguration configuration)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _channel = _connection.CreateModel();
            
            _channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct, durable: true);
        }

        public void Publish(string queueName, object message)
        {
            if (!_connection.IsOpen)
            {
                throw new InvalidOperationException("RabbitMQ bağlantısı kapalı");
            }

            _channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct, durable: true);

            _channel.QueueDeclare(queue: "stock-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: "stock-queue", exchange: _exchangeName, routingKey: "stock-queue");

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish(exchange: _exchangeName, routingKey: "stock-queue", basicProperties: properties, body: body);
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            _channel?.Dispose();
            _connection?.Dispose();
        }

    }

}
