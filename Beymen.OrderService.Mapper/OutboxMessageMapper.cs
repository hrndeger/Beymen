using Beymen.OrderService.Entity;
using Beymen.Service.Message.DTO;
using Newtonsoft.Json;

namespace Beymen.OrderService.Mapper
{
    public static class OutboxMessageMapper
    {
        public static OutboxMessage MapToEntity(this OrderDto orderDto)
        {
            if (orderDto == null) return null;

            var message = JsonConvert.SerializeObject(orderDto);
            return new OutboxMessage
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                IsProcessed = false,
                QueueName = "order.created",
                MessageBody = message
            };
        }
    }
}
