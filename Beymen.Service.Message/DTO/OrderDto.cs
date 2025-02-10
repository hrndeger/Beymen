using Newtonsoft.Json;

namespace Beymen.Service.Message.DTO
{
    public sealed class OrderDto
    {
        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        [JsonProperty("orderId")]
        public Guid OrderId { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        [JsonProperty("customerId")]
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the order items.
        /// </summary>
        [JsonProperty("orderItems")]
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
