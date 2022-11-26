using Microsoft.AspNetCore.Mvc;
using Order.Infrastructrues.Database;

namespace Order.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
	private readonly OrderDbContext _orderDbContext;
	private readonly ILogger<OrderController> _logger;

	public OrderController(
		OrderDbContext orderDbContext,
		ILogger<OrderController> logger)
	{
		_orderDbContext = orderDbContext;
		_logger = logger;
	}

	[HttpGet("list")]
	public async Task<IActionResult> GetList()
	{
		return Ok();
	}
}
