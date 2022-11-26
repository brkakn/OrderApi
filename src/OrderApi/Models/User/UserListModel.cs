using Order.Entities;
using Order.Services.AutoMapper;

namespace Order.Models.User;

public record UserListModel : IMapFrom<UserEntity>
{
	public string Name { get; set; } = null!;
	public string Surname { get; set; } = null!;
	public string Email { get; set; } = null!;
	public string PhoneNumber { get; set; } = null!;
	public decimal Balance { get; set; }
	public bool SmsNotification { get; set; }
	public bool EmailNotification { get; set; }
	public bool PushNotification { get; set; }
}
