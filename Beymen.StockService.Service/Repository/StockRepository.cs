using Beymen.StockService.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Beymen.StockService.Service.Repository
{
   
        public class StockRepository : Repository<ProductStock>
        {
            public StockRepository(StockDbContext context) : base(context) { }

            public async Task<IList<ProductStock>> GetFilteredAsync(Expression<Func<ProductStock, bool>> predicate)
            {
                return await _context.Set<ProductStock>().Where(predicate).ToListAsync();
            }
        }

}
