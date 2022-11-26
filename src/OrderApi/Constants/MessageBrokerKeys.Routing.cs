namespace OrderApi.Constants;

public static partial class MessageBrokerKeys
{
	public const string AddOrderKey = "order.add";
	public const string SendSmsKey = "notification.sms.send";
	public const string SendEmailKey = "notification.email.send";
	public const string SendPushKey = "notification.push.send";
}
