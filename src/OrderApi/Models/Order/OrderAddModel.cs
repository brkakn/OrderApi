using Order.Entities;
using Order.Services.AutoMapper;

namespace OrderApi.Models.Order;

public record OrderAddModel : IMapFrom<OrderEntity>
{
	public DateTime OrderDate { get; set; }
	public decimal Amount { get; set; }
}
