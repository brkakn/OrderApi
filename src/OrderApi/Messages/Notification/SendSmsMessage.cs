using Order.Entities;
using Order.Services.AutoMapper;
using Order.Services.BusServices.Messages;
using OrderApi.Constants;

namespace OrderApi.Messages.Notification;

public record SendSmsMessage() : PublishedMessage(MessageBrokerKeys.NotificationExchange, MessageBrokerKeys.SendSmsKey), IMessage, IMapFrom<NotificationHistoryEntity>
{

}
