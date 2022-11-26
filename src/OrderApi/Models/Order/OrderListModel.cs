using Order.Entities;
using Order.Enums;
using Order.Services.AutoMapper;

namespace OrderApi.Models.Order;

public record OrderListModel : IMapFrom<OrderEntity>
{
	public DateTime OrderDate { get; set; }
	public decimal Amount { get; set; }
	public OrderStatuses OrderStatus { get; set; }
}
