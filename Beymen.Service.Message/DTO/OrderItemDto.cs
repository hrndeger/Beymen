using Newtonsoft.Json;

namespace Beymen.Service.Message.DTO
{
    public sealed class OrderItemDto
    {
        [JsonProperty("productId")]
        public Guid ProductId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}
