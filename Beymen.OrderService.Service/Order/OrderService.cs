using Beymen.OrderService.Mapper;
using Beymen.OrderService.Model.Order.Request;
using Beymen.OrderService.Service.Repository;
using Microsoft.Extensions.Logging;

namespace Beymen.OrderService.Service.Order
{
    public sealed class OrderService : IOrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(OrderRepository orderRepository, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<Guid> InsertOrderAsync(CreateOrderRequestDto createOrderRequestDto)
        {
            try
            {
                var entity = createOrderRequestDto.MapToEntity();

                await _orderRepository.AddAsync(entity);

                return entity.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "InsertOrderAsync işlemi sırasında hata oluştu.");
                throw;
            }
            
        }
    }
}
