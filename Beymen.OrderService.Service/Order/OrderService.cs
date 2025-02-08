using Beymen.OrderService.Mapper;
using Beymen.OrderService.Model.Order.Request;
using Beymen.OrderService.Service.Repository;

namespace Beymen.OrderService.Service.Order
{
    public sealed class OrderService : IOrderService
    {
        private readonly OrderRepository _orderRepository;

        public OrderService(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Guid> InsertOrderAsync(CreateOrderRequestDto createOrderRequestDto)
        {
            var entity = createOrderRequestDto.MapToEntity();

            await _orderRepository.AddAsync(entity);

            return entity.Id;
        }
    }
}
