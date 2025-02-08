namespace Beymen.NotificationService.Service
{
    public interface INotificationService
    {
        Task SendNotificationAsync(Guid customerId, string message);
    }
}
