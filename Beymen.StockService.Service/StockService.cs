using Beymen.StockService.Service.Repository;
using Microsoft.Extensions.Logging;

namespace Beymen.StockService.Service
{
    public sealed class StockService : IStockService
    {
        private readonly StockRepository _stockRepository;
        private readonly ILogger<StockService> _logger;

        public StockService(StockRepository stockRepository, ILogger<StockService> logger)
        {
            _stockRepository = stockRepository;
            _logger = logger;
        }

        public async Task UpdateStockAsync(Guid productId, int quantity)
        {
            try
            {
                var productStock = await _stockRepository.GetByIdAsync(productId)
                                   ?? throw new KeyNotFoundException($"{productId} ürün bulunamadı.");

                if (productStock.Quantity < quantity)
                    throw new InvalidOperationException("Stok sayısı yeterli değil.");

                productStock.Quantity -= quantity;
                productStock.LastUpdated = DateTime.UtcNow;

                await _stockRepository.UpdateAsync(productStock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateStockAsync işlemi sırasında hata oluştu.");
                throw;
            }
        }
    }

}
