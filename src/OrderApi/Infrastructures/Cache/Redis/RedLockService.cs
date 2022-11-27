using Microsoft.Extensions.Options;
using OrderApi.Infrastructures.Cache.Redis;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

namespace Order.Infrastructures.Cache.Redis;

public class RedLockService : IRedLockService
{
	private readonly RedLockFactory _redLockFactory;

	public RedLockService(IOptions<RedisConfigModel> cacheConfigModel)
	{
		if (cacheConfigModel?.Value == null)
		{
			throw new ArgumentNullException(nameof(IOptions<RedisConfigModel>));
		}

		var configurationString = $"{cacheConfigModel.Value.Host}:{cacheConfigModel.Value.Port}";
		var existingConnectionMultiplexer = ConnectionMultiplexer.Connect(configurationString);

		var multiplexers = new List<RedLockMultiplexer>
		{
			existingConnectionMultiplexer,
		};
		_redLockFactory = RedLockFactory.Create(multiplexers);
	}

	public RedLockFactory GetRedLockFactory()
	{
		return _redLockFactory;
	}
}
