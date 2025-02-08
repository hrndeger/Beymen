using Beymen.OrderService.Service.Repository;

namespace Beymen.OrderService.Service.OutboxMessage
{
    public sealed class OutboxMessageService : IOutboxMessageService
    {
        private readonly OutboxMessageRepository _outboxMessageRepository;

        public OutboxMessageService(OutboxMessageRepository outboxMessageRepository)
        {
            _outboxMessageRepository = outboxMessageRepository;
        }

        public async Task<Guid> AddMessageAsync(string queueName, string message)
        {
            var entity = new Entity.OutboxMessage
            {
                QueueName = queueName,
                CreatedDate = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                IsProcessed = false,
                IsDeleted = false,
                MessageBody = message
            };

            await _outboxMessageRepository.AddAsync(entity);

            return entity.Id;
        }

        public async Task<IList<Entity.OutboxMessage>> GetPendingMessagesAsync()
        {
            return await _outboxMessageRepository.GetFilteredAsync(m => !m.IsProcessed);
        }

        public async Task SetProcessedAsync(Guid id, bool isProcessed)
        {
            var message = await _outboxMessageRepository.GetByIdAsync(id);

            if (message != null)
            {
                message.IsProcessed = isProcessed;
                await _outboxMessageRepository.UpdateAsync(message);
            }
        }

        public async Task SetProcessingAsync(Guid id, bool isProcessing)
        {
            var message = await _outboxMessageRepository.GetByIdAsync(id);

            if (message != null)
            {
                message.IsProcessing = isProcessing;
                await _outboxMessageRepository.UpdateAsync(message);
            }
        }
    }
}
