using Beymen.OrderService.Model.Order.Request;

namespace Beymen.OrderService.Business
{
    public interface IOrderBusiness
    {
        /// <summary>
        /// Creates the order asynchronous.
        /// </summary>
        /// <param name="createOrderRequestDto">The create order request dto.</param>
        /// <returns></returns>
        Task CreateOrderAsync(CreateOrderRequestDto createOrderRequestDto);
    }
}
