using OrderApi.Messages.Notification;
using OrderApi.Services.BusServices.Handlers;

namespace OrderApi.MessageHandlers.Notification;

public class SendPushMessageHandler : IMessageHandler<SendPushMessage>
{
	public SendPushMessageHandler()
	{
	}

	public Task HandleAsync(SendPushMessage message, CancellationToken ct = default)
	{
		throw new NotImplementedException();
	}
}
