using Microsoft.AspNetCore.Mvc;
using Order.Infrastructrues.Database;

namespace Order.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly OrderDbContext _orderDbContext;
    private readonly ILogger<UserController> _logger;

    public UserController(
        OrderDbContext orderDbContext,
        ILogger<UserController> logger)
    {
        _logger = logger;
        _orderDbContext = orderDbContext;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetList()
    {
        return Ok();
    }
}
