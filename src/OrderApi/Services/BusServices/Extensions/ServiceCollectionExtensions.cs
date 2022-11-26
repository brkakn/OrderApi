using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.ObjectPool;
using Order.Services.BusServices.Models;
using OrderApi.Services.BusServices;
using RabbitMQ.Client;

namespace Order.Services.BusServices.Extensions;

public static class ServiceCollectionExtensions
{
	public static IBusSubscriber UseRabbitMq(this IApplicationBuilder app)
	{
		return new BusSubscriber(app);
	}
	public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
	{
		if (services == null)
		{
			throw new ArgumentNullException(nameof(services));
		}

		if (configuration == null || !configuration.GetSection("RabbitMqConfig").Exists())
		{
			throw new ArgumentNullException(nameof(configuration));
		}

		services.Configure<RabbitMqConfigModel>(options => configuration.GetSection("RabbitMqConfig").Bind(options));

		services.TryAddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
		services.TryAddSingleton<IPooledObjectPolicy<IModel>, RabbitMqConnections>();
		services.TryAddSingleton<IBusPublisher, BusPublisher>();

		return services;
	}
}
