using Order.Entities;
using Order.Messages.Notification;
using Order.Services.AutoMapper;
using Order.Services.BusServices.Messages;
using OrderApi.Constants;

namespace OrderApi.Messages.Notification;

public record SendPushMessage() : BaseNotification(MessageBrokerKeys.SendPushKey), IMessage, IMapFrom<NotificationHistoryEntity>
{
}
