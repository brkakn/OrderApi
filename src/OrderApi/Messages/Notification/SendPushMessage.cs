using Order.Entities;
using Order.Services.AutoMapper;
using Order.Services.BusServices.Messages;
using OrderApi.Constants;

namespace OrderApi.Messages.Notification;

public record SendPushMessage() : PublishedMessage(MessageBrokerKeys.NotificationExchange, MessageBrokerKeys.SendPushKey), IMessage, IMapFrom<NotificationHistoryEntity>
{
	public string NotificationContent { get; set; }
}
