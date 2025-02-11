using Beymen.OrderService.Model.Order.Request;
using Beymen.OrderService.Service.Order;
using Beymen.OrderService.Service.OutboxMessage;
using Beymen.Service.Message;
using Beymen.Service.Message.DTO;
using Newtonsoft.Json;

namespace Beymen.OrderService.Business
{
    public sealed class OrderBusiness : IOrderBusiness
    {
        private readonly IOrderService _orderService;
        private readonly IOutboxMessageService _outboxMessageService;
        private readonly IMessageQueuePublisher _messageQueuePublisher;

        public OrderBusiness(IOrderService orderService, IOutboxMessageService outboxMessageService, IMessageQueuePublisher messageQueuePublisher)
        {
            _orderService = orderService;
            _outboxMessageService = outboxMessageService;
            _messageQueuePublisher = messageQueuePublisher;
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

            var message = JsonConvert.SerializeObject(orderDto);

            var outboxMessageResult = await _outboxMessageService.AddMessageAsync("stock-queue", message);

            if (outboxMessageResult != Guid.Empty)
            {
                _messageQueuePublisher.Publish("stock-queue", message);
            }

        }
    }
}
