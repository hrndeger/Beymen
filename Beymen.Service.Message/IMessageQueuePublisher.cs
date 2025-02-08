namespace Beymen.Service.Message
{
    // Burada daha sonra farklı bir message broker kullanılabileceği için
    //RabbitMq.Client referans olarak eklenmedi.
    public interface IMessageQueuePublisher
    {
        void Publish(string queueName, object message);
    }
}
