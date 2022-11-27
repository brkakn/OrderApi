using Order.Services.BusServices.Extensions;
using OrderApi.Constants;
using OrderApi.Messages.Notification;
using OrderApi.Messages.Order;
using OrderApi.Services.BusServices.Enums;

namespace OrderApi.Infrastructures.MessageBroker;

public static class SubscribeQueuesExtension
{
	public static async Task SubscribeQueues(this IApplicationBuilder app)
	{
		var bus = app.UseRabbitMq();
		await bus.SubscribeAsync<AddOrderMessage>(MessageBrokerKeys.OrderExchange, MessageBrokerKeys.AddOrderQueue, MessageBrokerKeys.AddOrderKey, ExchangeTypes.Topic, 4);
		await bus.SubscribeAsync<CancelOrderMessage>(MessageBrokerKeys.OrderExchange, MessageBrokerKeys.CancelOrderQueue, MessageBrokerKeys.CancelOrderKey, ExchangeTypes.Topic);
		await bus.SubscribeAsync<SendEmailMessage>(MessageBrokerKeys.NotificationExchange, MessageBrokerKeys.SendEmailQueue, MessageBrokerKeys.SendEmailKey, ExchangeTypes.Topic);
		await bus.SubscribeAsync<SendSmsMessage>(MessageBrokerKeys.NotificationExchange, MessageBrokerKeys.SendSmsQueue, MessageBrokerKeys.SendSmsKey, ExchangeTypes.Topic);
		await bus.SubscribeAsync<SendPushMessage>(MessageBrokerKeys.NotificationExchange, MessageBrokerKeys.SendPushQueue, MessageBrokerKeys.SendPushKey, ExchangeTypes.Topic);
	}
}
