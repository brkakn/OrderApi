using Order.Infrastructures.Cache.Redis;
using OrderApi.Constants;

namespace OrderApi.Infrastructures.Cache.Redis;

public static class ServiceCollectionExtension
{
	public static IServiceCollection AddRedisManager(this IServiceCollection services, IConfiguration configuration)
	{
		if (services == null)
		{
			throw new ArgumentNullException(nameof(services));
		}

		if (configuration == null || !configuration.GetSection(CommonConstants.CACHE_CONFIG_KEY).Exists())
		{
			throw new ArgumentNullException(nameof(configuration));
		}

		services.Configure<RedisConfigModel>(configuration.GetSection(CommonConstants.CACHE_CONFIG_KEY));
		services.AddSingleton<IRedLockService, RedLockService>();
		return services;
	}
}
