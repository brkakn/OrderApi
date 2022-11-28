using System.ComponentModel.DataAnnotations;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order.Constants;
using Order.Entities;
using Order.Enums;
using Order.Infrastructures.Database;
using OrderApi.Models.NotificationHistory;

namespace Order.Controllers;

[ApiController]
[Route("api/notification-histories")]
public class NotificationHistoryController : ControllerBase
{
	private readonly OrderDbContext _orderDbContext;
	private readonly IMapper _mapper;

	public NotificationHistoryController(
		OrderDbContext orderDbContext,
		IMapper mapper)
	{
		_orderDbContext = orderDbContext;
		_mapper = mapper;
	}

	[HttpGet("list")]
	public async Task<IActionResult> GetList(
		[FromHeader(Name = HttpRequestHeaderKeys.UserId)][Required] long userId,
		[FromQuery(Name ="Type")] NotificationTypes? type,
		CancellationToken ct)
	{
		var modelList = await _orderDbContext.Set<NotificationHistoryEntity>()
			.Where(
				e => e.UserId == userId 
				&& (type == null || e.Type == type)
				&& e.Status == Enums.RecordStatuses.Active)
			.AsNoTracking()
			.ProjectTo<NotificationHistoryListModel>(_mapper.ConfigurationProvider)
			.ToListAsync(ct);

		return Ok(modelList);
	}
}
