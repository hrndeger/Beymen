using Beymen.Service.Message.DTO;
using Beymen.StockService.Service;

namespace Beymen.StockService.Business
{
    public sealed class StockBusiness : IStockBusiness
    {
        private readonly IStockService _stockService;

        public StockBusiness(IStockService stockService)
        {
            _stockService = stockService;
        }

      

        public async Task UpdateStockAsync(OrderDto orderDto)
        {
            // içerisine liste alan bir model ile gönderilip
            // repositoryde AddRange ile kaydedilebilir.
            foreach (var item in orderDto.OrderItems)
            {
                await _stockService.UpdateStockAsync(item.ProductId, item.Quantity);
            }
        }
    }
}
