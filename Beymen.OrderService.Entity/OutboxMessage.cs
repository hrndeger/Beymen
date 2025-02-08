using Beymen.Service.Core.Entities;

namespace Beymen.OrderService.Entity
{
    public sealed class OutboxMessage : BaseEntity
    {
        public string QueueName { get; set; }
        public string MessageBody { get; set; }
        public bool IsProcessed { get; set; } = false;
        public bool IsProcessing { get; set; } = false;
    }
}
