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
        private readonly string _queueName = "order.created";

        public NotificationConsumer(IServiceScopeFactory serviceScopeFactory, ILogger<NotificationConsumer> logger, IConnection connection)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _connection = connection;

            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    var notificationDto = JsonConvert.DeserializeObject<OrderDto>(message);

                    if (notificationDto == null || notificationDto.CustomerId == Guid.Empty)
                    {
                        _logger.LogWarning("Geçersiz bildirim mesajı alındı: {Message}", message);
                        _channel.BasicNack(ea.DeliveryTag, false, false); // Mesajı işlenmedi olarak işaretle
                        return;
                    }

                    using (var scope = _serviceScopeFactory.CreateScope()) // Scoped servis burada alınmalı
                    {
                        var notificationBusiness = scope.ServiceProvider.GetRequiredService<INotificationBusiness>();
                        await notificationBusiness.SendNotificationAsync(notificationDto.CustomerId, "Bildirim mesajı");
                    }

                    _channel.BasicAck(ea.DeliveryTag, false); // Mesaj başarıyla işlendi, kuyruktan kaldır
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Bildirim işlenirken hata oluştu: {Message}", message);
                    _channel.BasicNack(ea.DeliveryTag, false, true); // Hata oluştu, mesajı tekrar kuyruğa al
                }
            };

            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

            return Task.CompletedTask;
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
