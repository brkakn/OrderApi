using Order.Entities;
using Order.Messages.Notification;
using Order.Services.AutoMapper;
using Order.Services.BusServices.Messages;
using OrderApi.Constants;

namespace OrderApi.Messages.Notification;

public record SendSmsMessage() : BaseNotification(MessageBrokerKeys.SendSmsKey), IMessage, IMapFrom<NotificationHistoryEntity>
{
}
