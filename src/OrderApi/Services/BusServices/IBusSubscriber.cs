using Order.Services.BusServices.Messages;
using OrderApi.Services.BusServices.Enums;

namespace OrderApi.Services.BusServices;

public interface IBusSubscriber
{
	Task SubscribeAsync<TMessage>(string exchangeName, string queueName, string routingKey, ExchangeTypes exchangeType = ExchangeTypes.Unknown, ushort prefetchCount = 1, int retry = 0, CancellationToken ct = default) where TMessage : IMessage;
}
