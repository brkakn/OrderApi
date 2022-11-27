using Order.Services.BusServices.Messages;
using OrderApi.Constants;

namespace Order.Messages.Notification;

public record BaseNotification : PublishedMessage
{
	public BaseNotification(string routingKey) : base(MessageBrokerKeys.NotificationExchange, routingKey)
	{
	}

	public long UserId { get; set; }
	public string Content { get; set; }
}
