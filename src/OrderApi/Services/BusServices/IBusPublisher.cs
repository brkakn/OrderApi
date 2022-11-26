using Order.Services.BusServices.Messages;
using RabbitMQ.Client;

namespace Order.Services.BusServices
{
	public interface IBusPublisher
	{
		void Publish<T>(T message, Guid? messageId = null, IBasicProperties? properties = null) where T : PublishedMessage;
	}
}
