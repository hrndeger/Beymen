using Beymen.Service.Message.DTO;
using Beymen.StockService.Entity;
using Beymen.StockService.Service;
using Microsoft.Extensions.Logging;

namespace Beymen.StockService.Business
{
    public sealed class StockBusiness : IStockBusiness
    {
        private readonly IStockService _stockService;
        private readonly StockDbContext _dbContext;
        private readonly ILogger<StockBusiness> _logger;

        public StockBusiness(IStockService stockService, StockDbContext dbContext, ILogger<StockBusiness> logger)
        {
            _stockService = stockService;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task UpdateStockAsync(OrderDto orderDto)
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                foreach (var item in orderDto.OrderItems)
                {
                    await _stockService.UpdateStockAsync(item.ProductId, item.Quantity);
                }

                await transaction.CommitAsync(); 
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); 
                _logger.LogError(ex, "Stok güncelleme işlemi sırasında hata oluştu.");
                throw;
            }
        }
    }

}
