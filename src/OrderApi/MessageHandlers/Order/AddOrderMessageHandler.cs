using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Order.Entities;
using Order.Enums;
using Order.Infrastructures.Cache.Helpers;
using Order.Infrastructures.Cache.Redis;
using Order.Infrastructures.Database;
using Order.Services.BusServices;
using OrderApi.Messages.Notification;
using OrderApi.Messages.Order;
using OrderApi.Services.BusServices.Handlers;

namespace OrderApi.MessageHandlers.Order;

public class AddOrderMessageHandler : IMessageHandler<AddOrderMessage>
{

	private readonly IRedLockService _redLockService;
	private readonly OrderDbContext _orderDbContext;
	private readonly IBusPublisher _busPublisher;
	private readonly IMapper _mapper;
	private readonly ILogger<AddOrderMessageHandler> _logger;

	public AddOrderMessageHandler(
		IRedLockService redLockService,
		OrderDbContext orderDbContext,
		IBusPublisher busPublisher,
		IMapper mapper,
		ILogger<AddOrderMessageHandler> logger)
	{
		_redLockService = redLockService;
		_orderDbContext = orderDbContext;
		_busPublisher = busPublisher;
		_mapper = mapper;
		_logger = logger;
	}

	public async Task HandleAsync(AddOrderMessage message, CancellationToken ct = default)
	{
		await using (var redLock = await _redLockService.GetRedLockFactory().CreateLockAsync(CacheHelper.GetUserOrderKey(message.UserId), TimeSpan.FromMinutes(2), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(5), ct))
		{
			if (redLock.IsAcquired)
			{
				bool isOrderExist = await _orderDbContext.Set<OrderEntity>()
					.Where(e => e.UserId == message.UserId && e.Status == RecordStatuses.Active && e.OrderStatus == OrderStatuses.Awaiting)
					.AnyAsync(ct);
				if (isOrderExist)
				{
					SendNotification(message, "Exist active order");
					return;
				}

				var orderEntity = _mapper.Map<OrderEntity>(message);
				orderEntity.Add();
				orderEntity.OrderStatus = OrderStatuses.Awaiting;
				orderEntity.UserId = message.UserId;

				await _orderDbContext.Set<OrderEntity>().AddAsync(orderEntity, ct);
				await _orderDbContext.SaveChangesAsync(ct);

				SendNotification(message, "Your order has been received");

				_logger.LogInformation($"UserId: {message.UserId} OrderAmount: {message.Amount}");
			}
		}
	}

	private void SendNotification(AddOrderMessage message, string notificationContent)
	{
		if (message.EmailNotification)
		{
			_busPublisher.Publish(new SendEmailMessage { Content = notificationContent, UserId = message.UserId });
		}

		if (message.SmsNotification)
		{
			_busPublisher.Publish(new SendSmsMessage { Content = notificationContent, UserId = message.UserId });
		}

		if (message.PushNotification)
		{
			_busPublisher.Publish(new SendPushMessage { Content = notificationContent, UserId = message.UserId });
		}
	}
}
