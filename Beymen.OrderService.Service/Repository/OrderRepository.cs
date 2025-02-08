using Beymen.OrderService.Entity;

namespace Beymen.OrderService.Service.Repository
{
    public class OrderRepository : Repository<Entity.Order>
    {
        public OrderRepository(OrderDbContext context) : base(context) { }
    }
}
