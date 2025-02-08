namespace Beymen.NotificationService.Business
{
    public interface INotificationBusiness
    {
        Task SendNotificationAsync(Guid customerId, string message);
    }
}
