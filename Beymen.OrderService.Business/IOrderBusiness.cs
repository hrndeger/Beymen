using Beymen.OrderService.Model.Order.Request;

namespace Beymen.OrderService.Business
{
    public interface IOrderBusiness
    {
        Task CreateOrderAsync(CreateOrderRequestDto createOrderRequestDto);
    }
}
