using System.ComponentModel.DataAnnotations;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order.Constants;
using Order.Entities;
using Order.Enums;
using Order.Infrastructures.Database;
using Order.Services.BusServices;
using OrderApi.Messages.Order;
using OrderApi.Models.Order;

namespace Order.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
	private readonly OrderDbContext _orderDbContext;
	private readonly IMapper _mapper;
	private readonly IBusPublisher _busPublisher;

	public OrderController(
		OrderDbContext orderDbContext,
		IMapper mapper,
		IBusPublisher busPublisher)
	{
		_orderDbContext = orderDbContext;
		_mapper = mapper;
		_busPublisher = busPublisher;
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(
		long id,
		[FromHeader(Name = HttpRequestHeaderKeys.UserId)][Required] long userId,
		CancellationToken ct)
	{
		var orderModel = await _orderDbContext.Set<OrderEntity>()
			.Where(e => e.Id == id && e.UserId == userId && e.Status == RecordStatuses.Active)
			.AsNoTracking()
			.ProjectTo<OrderModel>(_mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(ct);

		if (orderModel == null)
		{
			return NotFound();
		}

		return Ok(orderModel);
	}

	[HttpGet("list")]
	public async Task<IActionResult> GetList([FromHeader(Name = HttpRequestHeaderKeys.UserId)][Required] long userId, CancellationToken ct)
	{
		var modelList = await _orderDbContext.Set<OrderEntity>()
			.Where(e => e.UserId == userId && e.Status == RecordStatuses.Active)
			.AsNoTracking()
			.ProjectTo<OrderListModel>(_mapper.ConfigurationProvider)
			.ToListAsync(ct);

		return Ok(modelList);
	}

	[HttpPost]
	public async Task<IActionResult> Post(
		[FromHeader(Name = HttpRequestHeaderKeys.UserId)][Required] long userId,
		[FromBody] OrderAddModel orderAddModel,
		CancellationToken ct)
	{
		var userEntity = await _orderDbContext.Set<UserEntity>().Where(e => e.Id == userId && e.Status == RecordStatuses.Active).FirstOrDefaultAsync(ct);
		if (userEntity == null)
		{
			return NotFound("User not found");
		}

		var message = _mapper.Map<AddOrderMessage>(orderAddModel);
		message = _mapper.Map(userEntity, message);
		message.UserId = userId;

		_busPublisher.Publish(message, Guid.NewGuid());
		return Accepted();
	}

	[HttpPatch("{id:required}/cancel")]
	public async Task<IActionResult> Cancel(long id, [FromHeader(Name = HttpRequestHeaderKeys.UserId)][Required] long userId, CancellationToken ct)
	{
		var orderEntity = await _orderDbContext.Set<OrderEntity>().Where(e => e.Id == id && e.UserId == userId && e.Status == RecordStatuses.Active).FirstOrDefaultAsync(ct);
		if (orderEntity == null)
		{
			return NotFound();
		}

		switch (orderEntity.OrderStatus)
		{
			case OrderStatuses.Done:
				return BadRequest("Order has been done, can not cancel");
			case OrderStatuses.Cancelled:
				return BadRequest("Order already cancelled");
			default:
				break;
		}

		_busPublisher.Publish(new CancelOrderMessage { Id = id, UserId = userId });

		return Accepted();
	}
}
