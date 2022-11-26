using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order.Entities;
using Order.Infrastructures.Database;
using OrderApi.Models.NotificationHistory;

namespace Order.Controllers;

[ApiController]
[Route("api/notification-history")]
public class NotificationHistoryController : ControllerBase
{
	private readonly OrderDbContext _orderDbContext;
	private readonly IMapper _mapper;
	private readonly ILogger<NotificationHistoryController> _logger;

	public NotificationHistoryController(
		OrderDbContext orderDbContext,
		IMapper mapper,
		ILogger<NotificationHistoryController> logger)
	{
		_orderDbContext = orderDbContext;
		_mapper = mapper;
		_logger = logger;
	}

	[HttpGet("list")]
	public async Task<IActionResult> GetList(CancellationToken ct)
	{
		var modelList = await _orderDbContext.Set<NotificationHistoryEntity>()
			.Where(e => e.Status == Enums.RecordStatuses.Active)
			.AsNoTracking()
			.ProjectTo<NotificationHistoryListModel>(_mapper.ConfigurationProvider)
			.ToListAsync(ct);

		return Ok(modelList);
	}
}
