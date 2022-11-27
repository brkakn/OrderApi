using Order.Entities;
using Order.Enums;
using Order.Services.AutoMapper;

namespace OrderApi.Models.NotificationHistory;

public record NotificationHistoryListModel : IMapFrom<NotificationHistoryEntity>
{
	public long Id { get; set; }
	public string Content { get; set; } = null!;
	public string Type { get; set; }
	public string NotificationStatus { get; set; }
}
