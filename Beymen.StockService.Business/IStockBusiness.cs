using Beymen.Service.Message.DTO;

namespace Beymen.StockService.Business
{
    public interface IStockBusiness
    {
        /// <summary>
        /// Updates the stock asynchronous.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns></returns>
        Task UpdateStockAsync(OrderDto dto);        

    }
}
