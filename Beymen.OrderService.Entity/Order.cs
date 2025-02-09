using Beymen.Service.Core.Entities;

namespace Beymen.OrderService.Entity
{
    public sealed class Order : BaseEntity
    {    
        public Guid CustomerId { get; set; }

        public decimal Total { get; set; }

        public string  Descriptiom { get; set; }
        public ICollection<OrderItem> OrderItems { get;  set; }
    }
}
