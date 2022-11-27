using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.ObjectPool;
using Order.Services.BusServices.Models;
using OrderApi.Constants;
using OrderApi.Services.BusServices;
using OrderApi.Services.BusServices.Handlers;
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

		if (configuration == null || !configuration.GetSection(CommonConstants.RABBITMQ_CONFIG_KEY).Exists())
		{
			throw new ArgumentNullException(nameof(configuration));
		}

		services.Configure<RabbitMqConfigModel>(options => configuration.GetSection(CommonConstants.RABBITMQ_CONFIG_KEY).Bind(options));

		services.TryAddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
		services.TryAddSingleton<IPooledObjectPolicy<IModel>, RabbitMqConnections>();
		services.TryAddSingleton<IBusPublisher, BusPublisher>();

		return services;
	}

	public static IServiceCollection AddHandlers(this IServiceCollection services, params Assembly[] assemblies)
	{
		if (assemblies == null)
		{
			throw new ArgumentNullException(nameof(assemblies));
		}
		List<Type> types = new();

		foreach (var assembly in assemblies)
		{
			types.AddRange(Assembly.Load(assembly.GetName().Name ?? string.Empty).GetExportedTypes());
		}

		var messageHandlers = (from t in types
							   from i in t.GetInterfaces()
							   where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMessageHandler<>)
							   select new
							   {
								   IMessageHandler = i,
								   MessageHandler = t
							   }).ToArray();

		foreach (var messageHandler in messageHandlers)
		{
			services.AddScoped(messageHandler.IMessageHandler, messageHandler.MessageHandler);
		}

		return services;
	}
}
