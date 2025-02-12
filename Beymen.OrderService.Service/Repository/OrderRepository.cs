using Beymen.OrderService.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Beymen.OrderService.Service.Repository
{
    public class OrderRepository : Repository<Entity.Order>
    {
        public OrderRepository(OrderDbContext context) : base(context) { }

        public async Task<IEnumerable<Entity.Order>> GetByCustomerIdAsync(Guid customerId)
        {
            return await _context.Set<Entity.Order>()
                                 .Where(o => o.CustomerId == customerId)
                                 .ToListAsync();
        }


        public async Task<IList<Entity.Order>> GetFilteredAsync(Expression<Func<Entity.Order, bool>> predicate)
        {
            return await _context.Set<Entity.Order>().Where(predicate).ToListAsync();
        }

    }

}
