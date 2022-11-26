using Order.Entities.Common;
using Order.Enums;

namespace Order.Entities;

public class NotificationHistoryEntity : BaseEntity
{
	public long UserId { get; set; }
	public string Content { get; set; } = null!;
	public NotificationTypes Type { get; set; }
	public NotificationStatuses NotificationStatus { get; set; }
}
