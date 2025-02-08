
namespace Beymen.OrderService.Model.Order.Request
{
    public sealed class CreateOrderRequestDto
    {
        public Guid CustomerId { get; set; }

        public decimal Total { get; set; }

        public List<CreateOrderItemRequestDto> OrderItems { get; set; }
    }
}
