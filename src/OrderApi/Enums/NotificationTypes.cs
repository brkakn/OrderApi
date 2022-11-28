using System.ComponentModel;

namespace Order.Enums;

public enum NotificationTypes : byte
{
	Unknown = 0,
	Sms = 1,
	Email = 2,
	Push = 3
}
