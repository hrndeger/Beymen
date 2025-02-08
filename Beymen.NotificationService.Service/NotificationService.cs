
using Microsoft.Extensions.Logging;

namespace Beymen.NotificationService.Service
{
    public sealed class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger;
        }

        public async Task SendNotificationAsync(Guid customerId, string message)
        {
            // Burada Push Notification, Email veya SMS entegrasyonu olabilir.
            _logger.LogInformation("Customer {CustomerId} için bildirim gönderildi: {Message}", customerId, message);
            await Task.CompletedTask;
        }
    }
}
