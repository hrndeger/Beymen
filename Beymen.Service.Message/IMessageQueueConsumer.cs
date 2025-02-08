namespace Beymen.Service.Message
{
    public interface IMessageQueueConsumer
    {
        void Consume(string queueName, object message);
    }
}
