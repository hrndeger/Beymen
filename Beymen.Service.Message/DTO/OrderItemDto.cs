using Newtonsoft.Json;

namespace Beymen.Service.Message.DTO
{
    public sealed class OrderItemDto
    {
        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        [JsonProperty("productId")]        
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}
