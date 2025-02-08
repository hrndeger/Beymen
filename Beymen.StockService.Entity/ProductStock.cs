using Beymen.Service.Core.Entities;

namespace Beymen.StockService.Entity
{
    public sealed class ProductStock : BaseEntity
    {
        public int Quantity { get; set; }
        public DateTime LastUpdated { get;  set; }
    }
}
