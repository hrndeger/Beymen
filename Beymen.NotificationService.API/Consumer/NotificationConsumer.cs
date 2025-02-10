using Beymen.NotificationService.Business;
using Beymen.Service.Message.DTO;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Beymen.NotificationService.API.Consumer
{
    public class NotificationConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<NotificationConsumer> _logger;
       

        public NotificationConsumer(IServiceScopeFactory serviceScopeFactory, ILogger<NotificationConsumer> logger, IConnection connection)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _connection = connection;

            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "order-confirmed-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("RabbitMQ ile bağlantı kuruldu.");

            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body).Trim();

                try
                {
                    var order = JsonConvert.DeserializeObject<string>(message);
                    var notificationDto = JsonConvert.DeserializeObject<OrderDto>(order);

                    if (notificationDto == null)
                    {
                        _logger.LogWarning("Deserialization sonucu null döndü: {Message}", message);
                        _channel.BasicNack(ea.DeliveryTag, false, false);
                        return;
                    }

                    if (notificationDto.CustomerId == Guid.Empty)
                    {
                        _logger.LogWarning("Geçersiz müşteri ID'si: {Message}", message);
                        _channel.BasicNack(ea.DeliveryTag, false, false);
                        return;
                    }

                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var notificationBusiness = scope.ServiceProvider.GetRequiredService<INotificationBusiness>();
                        await notificationBusiness.SendNotificationAsync(notificationDto.CustomerId, "Bildirim mesajı");
                    }

                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (JsonSerializationException ex)
                {
                    _logger.LogError(ex, "JSON deserialization hatası: {Message}", message);

                    _channel.BasicNack(ea.DeliveryTag, false, true);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Bildirim işlenirken hata oluştu: {Message}", message);
                    _channel.BasicNack(ea.DeliveryTag, false, true); 
                }
            };

            _channel.BasicConsume(queue: "order-confirmed-queue", autoAck: false, consumer: consumer);
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("RabbitMQ ile bağlantı kapatıldı.");

            return base.StopAsync(cancellationToken);
        }
        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            _channel?.Dispose();
            _connection?.Dispose();
            base.Dispose();
        }
    }
}
