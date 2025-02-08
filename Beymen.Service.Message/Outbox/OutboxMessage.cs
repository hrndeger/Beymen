namespace Beymen.Service.Message.Outbox
{
    public sealed class OutboxMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string QueueName { get; set; }
        public string MessageBody { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsProcessed { get; set; } = false;
    }
}
