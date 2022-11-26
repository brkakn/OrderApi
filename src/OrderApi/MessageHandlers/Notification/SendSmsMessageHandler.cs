using OrderApi.Messages.Notification;
using OrderApi.Services.BusServices.Handlers;

namespace OrderApi.MessageHandlers.Notification;

public class SendSmsMessageHandler : IMessageHandler<SendSmsMessage>
{
	public SendSmsMessageHandler()
	{
	}

	public Task HandleAsync(SendSmsMessage message, CancellationToken ct = default)
	{
		throw new NotImplementedException();
	}
}
