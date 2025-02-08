using Beymen.Service.Message.DTO;
using Beymen.StockService.Business;
using Microsoft.AspNetCore.Mvc;

namespace Beymen.StockService.API.Controllers
{
    [ApiController]
    [Route("api/stocks")]
    public class StockController : ControllerBase
    {
        private readonly IStockBusiness _stockBusiness;
        public StockController(IStockBusiness stockBusiness)
        {
                _stockBusiness = stockBusiness;
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateStock([FromBody] OrderDto orderDto)
        {
            await _stockBusiness.UpdateStockAsync(orderDto);
            return Ok(new { Message = "Stok güncellendi." });
        }

    }
}
