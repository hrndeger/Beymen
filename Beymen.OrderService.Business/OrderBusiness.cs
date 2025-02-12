using Beymen.OrderService.Entity;
using Beymen.OrderService.Model.Order.Request;
using Beymen.OrderService.Service.Order;
using Beymen.OrderService.Service.OutboxMessage;
using Beymen.Service.Message.DTO;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Beymen.OrderService.Business
{
    public sealed class OrderBusiness : IOrderBusiness
    {
        private readonly IOrderService _orderService;
        private readonly IOutboxMessageService _outboxMessageService;
        private readonly OrderDbContext _dbContext;
        private readonly ILogger<OrderBusiness> _logger;


        public OrderBusiness(IOrderService orderService, IOutboxMessageService outboxMessageService, OrderDbContext dbContext, ILogger<OrderBusiness> logger)
        {
            _orderService = orderService;
            _outboxMessageService = outboxMessageService;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task CreateOrderAsync(CreateOrderRequestDto createOrderRequestDto)
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
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

                await _outboxMessageService.AddMessageAsync("order.created", message);

                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "CreateOrderAsync işlemi sırasında hata oluştu.");
                await transaction.RollbackAsync();
                throw new Exception("Sipariş işlemi sırasında beklenmeyen bir hata oluştu.");
            }


        }
    }
}
