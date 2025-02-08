using Beymen.OrderService.Entity;

namespace Beymen.OrderService.Service.Repository
{
    public class OutboxMessageRepository : Repository<Entity.OutboxMessage>
    {
        public OutboxMessageRepository(OrderDbContext context) : base(context) { }
    }
}
