namespace Beymen.OrderService.Model.Order.Request
{
    public sealed class CreateOrderItemRequestDto
    {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
