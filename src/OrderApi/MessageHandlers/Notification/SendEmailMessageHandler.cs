using OrderApi.Messages.Notification;
using OrderApi.Services.BusServices.Handlers;

namespace OrderApi.MessageHandlers.Notification;

public class SendEmailMessageHandler : IMessageHandler<SendEmailMessage>
{
	public SendEmailMessageHandler()
	{
	}

	public Task HandleAsync(SendEmailMessage message, CancellationToken ct = default)
	{
		throw new NotImplementedException();
	}
}
