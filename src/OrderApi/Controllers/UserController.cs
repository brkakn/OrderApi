using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order.Entities;
using Order.Enums;
using Order.Infrastructrues.Database;
using Order.Models.User;

namespace Order.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
	private readonly OrderDbContext _orderDbContext;
	private readonly IMapper _mapper;
	private readonly ILogger<UserController> _logger;

	public UserController(
		OrderDbContext orderDbContext,
		IMapper mapper,
		ILogger<UserController> logger)
	{
		_orderDbContext = orderDbContext;
		_mapper = mapper;
		_logger = logger;
	}

	[HttpGet("{id:required}")]
	public async Task<IActionResult> GetById(long id, CancellationToken ct)
	{
		var userModel = await _orderDbContext.Set<UserEntity>()
				.Where(e => e.Id == id && e.Status == RecordStatuses.Active)
				.ProjectTo<UserListModel>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(ct);

		if (userModel == null)
		{
			return NotFound();
		}

		return Ok(userModel);
	}

	[HttpGet("list")]
	public async Task<IActionResult> GetList(CancellationToken ct)
	{
		var query = _orderDbContext.Set<UserEntity>().Where(e => e.Status == RecordStatuses.Active);
		var model = await query.ProjectTo<UserListModel>(_mapper.ConfigurationProvider).ToListAsync(ct);
		return Ok(model);
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] UserAddModel userAddModel, CancellationToken ct)
	{
		bool isExist = await _orderDbContext.Set<UserEntity>().Where(e => e.Email == userAddModel.Email && e.Status == RecordStatuses.Active).AnyAsync(ct);
		if (isExist)
		{
			return BadRequest("Exist user");
		}

		var userEntity = _mapper.Map<UserEntity>(userAddModel);
		await _orderDbContext.AddAsync(userEntity, ct);
		await _orderDbContext.SaveChangesAsync(ct);

		_logger.LogInformation($"User added. User.Id: {userEntity.Id}");

		return CreatedAtAction(nameof(GetById), new { userEntity.Id }, userAddModel);
	}
}
