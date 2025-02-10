using Beymen.StockService.Service.Repository;

namespace Beymen.StockService.Service
{
    public sealed class StockService : IStockService
    {
        private readonly StockRepository _stockRepository;

        public StockService(StockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }


        public async Task UpdateStockAsync(Guid productId, int quantity)
        {
            var productStock = await _stockRepository.GetByIdAsync(productId) ?? throw new KeyNotFoundException($"{productId} ürün bulunamadı.");

            if (productStock.Quantity < quantity)
                throw new InvalidOperationException("Stok sayısı yeterli değil.");

            productStock.Quantity -= quantity;
            productStock.LastUpdated = DateTime.UtcNow;

            await _stockRepository.UpdateAsync(productStock);
        }
    }
}
