using Beymen.Service.Message.DTO;

namespace Beymen.StockService.Service
{
    public interface IStockService
    {
        Task UpdateStockAsync(Guid productId, int quantity);

    }
}
