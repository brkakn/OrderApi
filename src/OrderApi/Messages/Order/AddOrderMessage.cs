using Order.Entities;
using Order.Enums;
using Order.Services.AutoMapper;
using Order.Services.BusServices.Messages;
using OrderApi.Constants;
using OrderApi.Models.Order;

namespace OrderApi.Messages.Order;

public record AddOrderMessage() : PublishedMessage(MessageBrokerKeys.OrderExchange, MessageBrokerKeys.AddOrderKey), IMessage, IMapFrom<OrderEntity>, IMapFrom<OrderAddModel>, IMapFrom<UserEntity>
{
	public long UserId { get; set; }
	public DateTime OrderDate { get; set; }
	public decimal Amount { get; set; }
	public OrderStatuses OrderStatus { get; set; }
	public bool SmsNotification { get; set; }
	public bool EmailNotification { get; set; }
	public bool PushNotification { get; set; }
}
