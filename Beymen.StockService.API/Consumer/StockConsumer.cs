﻿using Beymen.Service.Message.DTO;
using Beymen.StockService.Business;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Beymen.StockService.API.Consumer
{
    public class StockConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<StockConsumer> _logger;
        private readonly string _queueName = "order.created";

        public StockConsumer(IServiceScopeFactory serviceScopeFactory, ILogger<StockConsumer> logger, IConnection connection)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
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
                    var orderDto = JsonConvert.DeserializeObject<OrderDto>(message);

                    if (orderDto == null)
                    {
                        _logger.LogWarning("Geçersiz mesaj formatı: {Message}", message);
                        _channel.BasicNack(ea.DeliveryTag, false, false); // Mesajı işlenmedi olarak işaretle, tekrar kuyruğa ekleme
                        return;
                    }

                    using (var scope = _serviceScopeFactory.CreateScope()) // Scoped servis burada alınmalı
                    {
                        var stockBusiness = scope.ServiceProvider.GetRequiredService<IStockBusiness>();
                        await stockBusiness.UpdateStockAsync(orderDto);
                    }

                    _logger.LogInformation("Stok güncellendi: {OrderId}", orderDto.OrderId);

                    _channel.BasicAck(ea.DeliveryTag, false); // Mesaj başarıyla işlendi, onay ver
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Stok güncellenirken hata oluştu: {Message}", message);
                    _channel.BasicNack(ea.DeliveryTag, false, true); // Hata oluştu, mesajı tekrar kuyruğa ekle
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
