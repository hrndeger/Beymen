using Beymen.OrderService.Service.OutboxMessage;
using Beymen.Service.Message;

namespace Beymen.OrderService.API.Processor
{
    public class OutboxProcessor : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMessageQueuePublisher _orderPublisher;
        private readonly ILogger<OutboxProcessor> _logger;
       
        public OutboxProcessor(IServiceScopeFactory serviceScopeFactory, IMessageQueuePublisher orderPublisher, ILogger<OutboxProcessor> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _orderPublisher = orderPublisher;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var outboxMessageService = scope.ServiceProvider.GetRequiredService<IOutboxMessageService>();

                    var messages = await outboxMessageService.GetPendingMessagesAsync();

                    if (messages.Count > 0)
                    {
                        foreach (var message in messages)
                        {
                            try
                            {
                                await outboxMessageService.SetProcessingAsync(message.Id, true);

                                _orderPublisher.Publish(message.QueueName, message.MessageBody);

                                await outboxMessageService.SetProcessedAsync(message.Id, true);

                            }
                            catch (Exception ex)
                            {
                                await outboxMessageService.SetProcessingAsync(message.Id, false);

                                _logger.LogError(ex, $"Mesaj iletilirken hata oluştu :{message.Id}");
                            }
                        }
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}