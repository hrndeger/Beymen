namespace Beymen.Service.Message.DTO
{
    public sealed class OrderItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
