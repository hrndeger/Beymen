using Beymen.Service.Core.Entities;

namespace Beymen.OrderService.Entity
{
    public sealed class OrderItem : BaseEntity
    {
        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public Order Order { get; set; }
    }
}
