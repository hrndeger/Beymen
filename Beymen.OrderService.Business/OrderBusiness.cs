using Beymen.OrderService.Model.Order.Request;
using Beymen.OrderService.Service.Order;
using Beymen.OrderService.Service.OutboxMessage;
using Beymen.Service.Message.DTO;
using Newtonsoft.Json;
using System.Text;

namespace Beymen.OrderService.Business
{
    public sealed class OrderBusiness : IOrderBusiness
    {
        private readonly IOrderService _orderService;
        private readonly IOutboxMessageService _outboxMessageService;

        public OrderBusiness(IOrderService orderService, IOutboxMessageService outboxMessageService)
        {
            _orderService = orderService;
            _outboxMessageService = outboxMessageService;
        }

        public async Task CreateOrderAsync(CreateOrderRequestDto createOrderRequestDto)
        {
            var orderGuid = await _orderService.InsertOrderAsync(createOrderRequestDto);

            if (orderGuid == Guid.Empty)
            {
                throw new Exception("Sipariş oluşturulması sırasında hata oluştu.");
            }

            OrderDto orderDto = new()
            {
                OrderId = orderGuid,
                CustomerId = createOrderRequestDto.CustomerId,
                OrderItems = createOrderRequestDto.OrderItems.Select(x => new OrderItemDto
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                }).ToList()
            };

            var message= JsonConvert.SerializeObject(orderDto);

            await _outboxMessageService.AddMessageAsync("order.created", message);

        }
    }
}
