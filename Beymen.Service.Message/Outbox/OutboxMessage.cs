namespace Beymen.Service.Message.Outbox
{
    public sealed class OutboxMessage
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the name of the queue.
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// Gets or sets the message body.
        /// </summary>
        public string MessageBody { get; set; }

        /// <summary>
        /// Gets or sets the created at.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is processed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is processed; otherwise, <c>false</c>.
        /// </value>
        public bool IsProcessed { get; set; } = false;
    }
}
