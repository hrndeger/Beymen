using Beymen.OrderService.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Beymen.OrderService.Service.Repository
{
    public class OutboxMessageRepository : Repository<Entity.OutboxMessage>
    {
        public OutboxMessageRepository(OrderDbContext context) : base(context) { }


        public async Task<IList<Entity.OutboxMessage>> GetFilteredAsync(Expression<Func<Entity.OutboxMessage, bool>> predicate)
        {
            return await _context.Set<Entity.OutboxMessage>().Where(predicate).ToListAsync();
        }
    }

}
