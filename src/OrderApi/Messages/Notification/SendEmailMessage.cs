using Order.Entities;
using Order.Services.AutoMapper;
using Order.Services.BusServices.Messages;
using OrderApi.Constants;

namespace OrderApi.Messages.Notification;

public record SendEmailMessage() : PublishedMessage(MessageBrokerKeys.NotificationExchange, MessageBrokerKeys.SendEmailKey), IMessage, IMapFrom<NotificationHistoryEntity>
{

}
