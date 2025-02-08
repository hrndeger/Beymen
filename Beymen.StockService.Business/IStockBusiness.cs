using Beymen.Service.Message.DTO;

namespace Beymen.StockService.Business
{
    public interface IStockBusiness
    {
        Task UpdateStockAsync(OrderDto dto);

    }
}
