namespace Beymen.NotificationService.Business
{
    public interface INotificationBusiness
    {
        /// <summary>
        /// Sends the notification asynchronous.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        Task SendNotificationAsync(Guid customerId, string message);
    }
}
