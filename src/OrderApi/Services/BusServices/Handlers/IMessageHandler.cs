using Order.Services.BusServices.Messages;

namespace OrderApi.Services.BusServices.Handlers;

public interface IMessageHandler<in TMessage> where TMessage : IMessage
{
	Task HandleAsync(TMessage message, CancellationToken ct = default);
}
