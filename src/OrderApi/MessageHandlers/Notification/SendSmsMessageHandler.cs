using AutoMapper;
using Order.Entities;
using Order.Enums;
using Order.Infrastructures.Database;
using OrderApi.Messages.Notification;
using OrderApi.Services.BusServices.Handlers;

namespace OrderApi.MessageHandlers.Notification;

public class SendSmsMessageHandler : IMessageHandler<SendSmsMessage>
{
	private readonly OrderDbContext _orderDbContext;
	private readonly IMapper _mapper;

	public SendSmsMessageHandler(
		OrderDbContext orderDbContext,
		IMapper mapper)
	{
		_orderDbContext = orderDbContext;
		_mapper = mapper;
	}

	public async Task HandleAsync(SendSmsMessage message, CancellationToken ct = default)
	{
		var notificationHistoryEntity = _mapper.Map<NotificationHistoryEntity>(message);
		notificationHistoryEntity.Add();
		notificationHistoryEntity.Type = NotificationTypes.Sms;

		try
		{
			Thread.Sleep(100); // fake call
			notificationHistoryEntity.NotificationStatus = NotificationStatuses.Sent;
		}
		catch (Exception)
		{
			notificationHistoryEntity.NotificationStatus = NotificationStatuses.Error;
		}

		await _orderDbContext.Set<NotificationHistoryEntity>().AddAsync(notificationHistoryEntity, ct);
		await _orderDbContext.SaveChangesAsync(ct);
	}
}
