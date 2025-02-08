using Beymen.OrderService.Entity;
using Beymen.OrderService.Model.Order.Request;

namespace Beymen.OrderService.Mapper
{
    public static class OrderDtoMapper
    {
        public static Order MapToEntity(this CreateOrderRequestDto dto)
        {
            if (dto == null) return null;


            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = dto.CustomerId,
                Total = dto.Total,
                IsDeleted = false,
                CreatedDate = DateTime.Now
            };

            order.OrderItems = dto.OrderItems.Select(x => new OrderItem
            {
                ProductId = x.ProductId,
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                OrderId = order.Id,
                Price = x.Price,
                Quantity = x.Quantity,
                IsDeleted = false
            }).ToList();

            return order;
        }
    }
}
