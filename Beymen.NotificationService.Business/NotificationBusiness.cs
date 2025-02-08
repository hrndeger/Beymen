
using Beymen.NotificationService.Service;

namespace Beymen.NotificationService.Business
{
    public class NotificationBusiness : INotificationBusiness
    {

        private readonly INotificationService _notificationService;
        public NotificationBusiness(INotificationService notificationService)
        {
            _notificationService = notificationService;

        }
        public async Task SendNotificationAsync(Guid customerId,string message)
        {           
           await _notificationService.SendNotificationAsync(customerId,message);
        }
    }
}
