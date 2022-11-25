using Order.Entities.Common;

namespace Order.Entities;

public class UserEntity : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public decimal Balance { get; set; }
    public bool SmsNotification { get; set; }
    public bool EmailNotification { get; set; }
    public bool PushNotification { get; set; }

    public ICollection<OrderEntity>? Orders { get; set; }
}
