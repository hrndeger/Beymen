using Beymen.Service.Message.DTO;

namespace Beymen.StockService.Service
{
    public interface IStockService
    {
        /// <summary>
        /// Updates the stock asynchronous.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="quantity">The quantity.</param>
        /// <returns></returns>
        Task UpdateStockAsync(Guid productId, int quantity);

    }
}
