using Beymen.Service.Message.DTO;
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

        public StockConsumer(IServiceScopeFactory serviceScopeFactory, ILogger<StockConsumer> logger, IConnection connection)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _connection = connection;

            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "stock-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
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
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    var order = JsonConvert.DeserializeObject<string>(message);
                    var orderDto = JsonConvert.DeserializeObject<OrderDto>(order);


                    if (orderDto == null)
                    {
                        _logger.LogWarning("Geçersiz mesaj formatı: {Message}", message);
                        _channel.BasicNack(ea.DeliveryTag, false, false); 
                        return;
                    }

                    using (var scope = _serviceScopeFactory.CreateScope()) 
                    {
                        var stockBusiness = scope.ServiceProvider.GetRequiredService<IStockBusiness>();
                        await stockBusiness.UpdateStockAsync(orderDto);
                    }

                    _logger.LogInformation("Stok güncellendi: {OrderId}", orderDto.OrderId);

                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _channel.BasicNack(ea.DeliveryTag, false, true); 
                    _logger.LogError(ex, "Stok güncellenirken hata oluştu: {Message}", message);
                }
            };

            _channel.BasicConsume(queue: "stock-queue", autoAck: false, consumer: consumer);

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
