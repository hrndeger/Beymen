using Beymen.StockService.Entity;

namespace Beymen.StockService.Service.Repository
{
    public class StockRepository : Repository<ProductStock>
    {
        public StockRepository(StockDbContext context) : base(context) { }

    }
}
