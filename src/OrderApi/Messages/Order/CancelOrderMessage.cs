using Order.Services.BusServices.Messages;
using OrderApi.Constants;

namespace OrderApi.Messages.Order;

public record CancelOrderMessage() : PublishedMessage(MessageBrokerKeys.OrderExchange, MessageBrokerKeys.CancelOrderKey), IMessage
{
	public long Id { get; set; }
	public long UserId { get; set; }
}
