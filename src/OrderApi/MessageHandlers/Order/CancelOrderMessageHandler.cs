using OrderApi.Messages.Order;
using OrderApi.Services.BusServices.Handlers;

namespace OrderApi.MessageHandlers.Order;

public class CancelOrderMessageHandler : IMessageHandler<CancelOrderMessage>
{
	public CancelOrderMessageHandler()
	{
	}

	public Task HandleAsync(CancelOrderMessage message, CancellationToken ct = default)
	{
		throw new NotImplementedException();
	}
}
