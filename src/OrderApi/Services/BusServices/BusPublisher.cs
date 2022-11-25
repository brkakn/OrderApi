using Microsoft.Extensions.ObjectPool;
using Order.Services.BusServices.Messages;
using RabbitMQ.Client;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Order.Services.BusServices;

public class BusPublisher : IBusPublisher
{
    private readonly DefaultObjectPool<IModel> _channelPool;

    public BusPublisher(IPooledObjectPolicy<IModel> objectPolicy)
    {
        _channelPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount * 2);
    }

    public void Publish<T>(T message, Guid? messageId = null, IBasicProperties? properties = null) where T : PublishedMessage
    {
        if (message == null)
        {
            return;
        }

        var channel = _channelPool.Get();

        try
        {
            if (properties == null)
            {
                properties = channel.CreateBasicProperties();
                properties.Persistent = true;
            }
            if (messageId.HasValue && messageId.Value != Guid.Empty)
            {
                properties.MessageId = messageId.ToString();
            }

            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message,
            options: new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }));

            channel.BasicPublish(message.ExchangeName, message.RoutingKey, properties, bytes);
        }
        finally
        {
            _channelPool.Return(channel);
        }
    }
}
