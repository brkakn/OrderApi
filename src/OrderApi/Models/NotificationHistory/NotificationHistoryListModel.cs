using Order.Entities;
using Order.Enums;
using Order.Services.AutoMapper;

namespace OrderApi.Models.NotificationHistory;

public record NotificationHistoryListModel : IMapFrom<NotificationHistoryEntity>
{
	public long Id { get; set; }
	public string Content { get; set; } = null!;
	public NotificationTypes Type { get; set; }
	public NotificationStatuses NotificationStatus { get; set; }
}
