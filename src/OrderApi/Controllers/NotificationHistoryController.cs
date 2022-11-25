using Microsoft.AspNetCore.Mvc;
using Order.Infrastructrues.Database;

namespace Order.Controllers;

[ApiController]
[Route("api/notification-history")]
public class NotificationHistoryController : ControllerBase
{
    private readonly OrderDbContext _orderDbContext;
    private readonly ILogger<NotificationHistoryController> _logger;

    public NotificationHistoryController(
        OrderDbContext orderDbContext,
        ILogger<NotificationHistoryController> logger)
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
