using Beymen.OrderService.Model.Order.Request;

namespace Beymen.OrderService.Service.Order
{
    public interface IOrderService
    {
        Task<Guid> InsertOrderAsync(CreateOrderRequestDto order);

    }
}
