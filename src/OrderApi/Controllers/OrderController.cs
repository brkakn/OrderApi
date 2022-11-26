using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order.Entities;
using Order.Enums;
using Order.Infrastructures.Database;
using OrderApi.Models.Order;

namespace Order.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
	private readonly OrderDbContext _orderDbContext;
	private readonly IMapper _mapper;
	private readonly ILogger<OrderController> _logger;

	public OrderController(
		OrderDbContext orderDbContext,
		IMapper mapper,
		ILogger<OrderController> logger)
	{
		_orderDbContext = orderDbContext;
		_mapper = mapper;
		_logger = logger;
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(long id, CancellationToken ct)
	{
		var orderModel = await _orderDbContext.Set<OrderEntity>()
			.Where(e => e.Id == id && e.Status == RecordStatuses.Active)
			.AsNoTracking()
			.ProjectTo<OrderModel>(_mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(ct);

		if (orderModel == null)
		{
			return NotFound();
		}

		return Ok(orderModel);
	}

	[HttpGet("{userId:required}/list")]
	public async Task<IActionResult> GetList(long userId, CancellationToken ct)
	{
		var modelList = await _orderDbContext.Set<OrderEntity>()
			.Where(e => e.UserId == userId && e.Status == RecordStatuses.Active)
			.AsNoTracking()
			.ProjectTo<OrderListModel>(_mapper.ConfigurationProvider)
			.ToListAsync(ct);

		return Ok(modelList);
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] OrderAddModel orderAddModel, CancellationToken ct)
	{

		return Accepted();
	}
}
