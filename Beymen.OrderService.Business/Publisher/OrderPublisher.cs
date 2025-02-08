using Beymen.Service.Message;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Beymen.OrderService.Business.Publisher
{
    //singleton pattern kullanıldı
    public sealed class OrderPublisher : IMessageQueuePublisher, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public OrderPublisher()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                DispatchConsumersAsync = true
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Publish(string queueName, object message)
        {
            if (!_connection.IsOpen)
            {
                throw new InvalidOperationException("RabbitMQ bağlantısı kapalı");
            }

            //herhangi bir filtreleme yapmadan kendisine bağlı kaç tane queue varsa hepsine aynı mesajı yollar.
            _channel.ExchangeDeclare("order-fanout", durable: true, type: ExchangeType.Fanout);

            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: properties, body: body);
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
