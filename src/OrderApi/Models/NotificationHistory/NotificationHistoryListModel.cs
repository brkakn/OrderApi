using AutoMapper;
using Order.Entities;
using Order.Services.AutoMapper;

namespace OrderApi.Models.NotificationHistory;

public record NotificationHistoryListModel : IMapFromCustomMapping
{
	public long Id { get; set; }
	public string Content { get; set; } = null!;
	public string Type { get; set; }
	public string NotificationStatus { get; set; }
	public DateTime SentDate { get; set; }

	public void CreateMappings(IProfileExpression profileExpression)
	{
		profileExpression.CreateMap<NotificationHistoryEntity, NotificationHistoryListModel>()
			.ForMember(dest => dest.SentDate, opt => opt.MapFrom(src => src.CreatedDate));
	}
}
