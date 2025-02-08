using Beymen.OrderService.Business;
using Beymen.OrderService.Model.Order.Request;
using Microsoft.AspNetCore.Mvc;

namespace Beymen.OrderService.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderBusiness _orderBusiness;

        public OrderController(IOrderBusiness orderBusiness)
        {
            _orderBusiness = orderBusiness;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequestDto createOrderRequestDto)
        {
            await _orderBusiness.CreateOrderAsync(createOrderRequestDto);
            return Ok(new { Message = "Sipariş oluşturuldu." });
        }
    }
}
