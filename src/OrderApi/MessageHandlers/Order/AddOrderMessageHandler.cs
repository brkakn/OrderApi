using OrderApi.Messages.Order;
using OrderApi.Services.BusServices.Handlers;

namespace OrderApi.MessageHandlers.Order;

public class AddOrderMessageHandler : IMessageHandler<AddOrderMessage>
{
	public AddOrderMessageHandler()
	{
	}

	public Task HandleAsync(AddOrderMessage message, CancellationToken ct = default)
	{
		throw new NotImplementedException();
	}
}
