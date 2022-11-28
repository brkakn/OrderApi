using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order.Entities;
using Order.Enums;
using Order.Infrastructures.Database;
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
			.AsNoTracking()
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
		var modelList = await _orderDbContext.Set<UserEntity>()
			.Where(e => e.Status == RecordStatuses.Active)
			.AsNoTracking()
			.ProjectTo<UserListModel>(_mapper.ConfigurationProvider)
			.ToListAsync(ct);

		return Ok(modelList);
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
		userEntity.Add();
		await _orderDbContext.AddAsync(userEntity, ct);
		await _orderDbContext.SaveChangesAsync(ct);

		_logger.LogInformation($"User added. User.Id: {userEntity.Id}");

		return CreatedAtAction(nameof(GetById), new { userEntity.Id }, userAddModel);
	}

	[HttpPut("{id:required}")]
	public async Task<IActionResult> Put(long id, [FromBody] UserAddModel model, CancellationToken ct)
	{
		var userEntity = await _orderDbContext.Set<UserEntity>().Where(e => e.Id == id && e.Status == RecordStatuses.Active).FirstOrDefaultAsync(ct);
		if (userEntity == null)
		{
			return NotFound();
		}

		userEntity = _mapper.Map(model, userEntity);
		userEntity.Update();

		await _orderDbContext.SaveChangesAsync(ct);

		return NoContent();
	}

	[HttpPatch("{id:required}")]
	public async Task<IActionResult> Patch(long id, [FromBody] JsonPatchDocument<UserEntity> patchDoc, CancellationToken ct)
	{
		if (patchDoc != null)
		{
			var existUser = await _orderDbContext.Set<UserEntity>().Where(e => e.Id == id && e.Status == RecordStatuses.Active).FirstOrDefaultAsync(ct);
			if (existUser == null)
			{
				return NotFound();
			}

			patchDoc.ApplyTo(existUser);
			existUser.Update();
			await _orderDbContext.SaveChangesAsync(ct);
			return new ObjectResult(existUser);
		}
		else
		{
			return BadRequest(ModelState);
		}
	}

	[HttpDelete("{id:required}")]
	public async Task<IActionResult> Delete(long id, CancellationToken ct)
	{
		var userEntity = await _orderDbContext.Set<UserEntity>().Where(e => e.Id == id && e.Status == RecordStatuses.Active).FirstOrDefaultAsync(ct);
		if (userEntity == null)
		{
			return NotFound();
		}

		userEntity.Delete();

		await _orderDbContext.SaveChangesAsync(ct);

		return Ok();
	}
}
