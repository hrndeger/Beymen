using Beymen.NotificationService.Business;
using Beymen.Service.Message.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Beymen.NotificationService.API.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationBusiness _notificationBusiness;
        public NotificationController(INotificationBusiness notificationBusiness)
        {
            _notificationBusiness = notificationBusiness;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] OrderDto orderDto)
        {
            await _notificationBusiness.SendNotificationAsync(orderDto.CustomerId, "");
            return Ok(new { Message = "Mesaj gönderildi." });
        }
    }
}
