using Microsoft.EntityFrameworkCore;
using Order.Entities;
using Order.Enums;
using Order.Infrastructures.Database;
using Order.Services.BusServices;
using OrderApi.Messages.Notification;
using OrderApi.Messages.Order;
using OrderApi.Services.BusServices.Handlers;

namespace OrderApi.MessageHandlers.Order;

public class CancelOrderMessageHandler : IMessageHandler<CancelOrderMessage>
{
	private readonly OrderDbContext _orderDbContext;
	private readonly IBusPublisher _busPublisher;

	public CancelOrderMessageHandler(
		OrderDbContext orderDbContext,
		IBusPublisher busPublisher)
	{
		_orderDbContext = orderDbContext;
		_busPublisher = busPublisher;
	}

	public async Task HandleAsync(CancelOrderMessage message, CancellationToken ct = default)
	{
		var orderEntity = await _orderDbContext.Set<OrderEntity>()
			.Where(e => e.UserId == message.UserId && e.Status == RecordStatuses.Active && e.OrderStatus != OrderStatuses.Done)
			.FirstOrDefaultAsync(ct);
		if (orderEntity == null)
		{
			return;
		}

		orderEntity.OrderStatus = OrderStatuses.Cancelled;
		orderEntity.Update();
		await _orderDbContext.SaveChangesAsync(ct);

		SendNotification(orderEntity, "Order has been cancelled");
	}

	private void SendNotification(OrderEntity order, string notificationContent)
	{
		if (order.EmailNotification)
		{
			_busPublisher.Publish(new SendEmailMessage { Content = notificationContent });
		}

		if (order.SmsNotification)
		{
			_busPublisher.Publish(new SendSmsMessage { Content = notificationContent });
		}

		if (order.PushNotification)
		{
			_busPublisher.Publish(new SendPushMessage { Content = notificationContent });
		}
	}
}
