using Microsoft.EntityFrameworkCore;

namespace Beymen.StockService.Entity
{
    public class StockDbContext : DbContext
    {
        public StockDbContext(DbContextOptions<StockDbContext> options) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<ProductStock> ProductStocks { get; set; }
    }
}
