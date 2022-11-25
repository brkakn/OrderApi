using Order.Entities.Common;
using Order.Enums;

namespace Order.Entities;

public class OrderEntity : BaseEntity
{
    public long UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal Amount { get; set; }
    public OrderStatuses Status { get; set; }

    public virtual UserEntity User { get; set; } = null!;
}
