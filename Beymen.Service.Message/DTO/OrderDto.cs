namespace Beymen.Service.Message.DTO
{
    public sealed class OrderDto
    {
        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>

        public Guid CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the order items.
        /// </summary>
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
