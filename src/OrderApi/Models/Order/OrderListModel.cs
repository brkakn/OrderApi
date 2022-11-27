using Order.Entities;
using Order.Enums;
using Order.Services.AutoMapper;

namespace OrderApi.Models.Order;

public record OrderListModel : IMapFrom<OrderEntity>
{
	public long Id { get; set; }
	public DateTime OrderDate { get; set; }
	public decimal Amount { get; set; }
	public OrderStatuses OrderStatus { get; set; }
	public bool SmsNotification { get; set; }
	public bool EmailNotification { get; set; }
	public bool PushNotification { get; set; }
}
