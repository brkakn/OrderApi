namespace OrderApi.Constants;

public static partial class MessageBrokerKeys
{
	public const string AddOrderQueue = "add_order_queue";
	public const string CancelOrderQueue = "cancel_order_queue";
	public const string SendSmsQueue = "send_sms_queue";
	public const string SendEmailQueue = "send_email_queue";
	public const string SendPushQueue = "send_push_queue";
}
