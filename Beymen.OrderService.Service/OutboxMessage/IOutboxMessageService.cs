namespace Beymen.OrderService.Service.OutboxMessage
{
    public interface IOutboxMessageService
    {
        Task<Guid> AddMessageAsync(string queueName, string message);

        Task<IList<Entity.OutboxMessage>> GetPendingMessagesAsync();

        Task SetProcessedAsync(Guid id, bool isProcessed);

        Task SetProcessingAsync(Guid id, bool isProcessing);

    }
}
