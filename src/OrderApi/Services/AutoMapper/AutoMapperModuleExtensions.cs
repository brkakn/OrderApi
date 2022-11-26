using System.Reflection;
using AutoMapper;

namespace Order.Services.AutoMapper;

public static class AutoMapperModuleExtensıons
{
	public static IServiceCollection AddAutoMapper(this IServiceCollection services, params Assembly[] optionalAssemblies)
	{
		var mapper = new MapperConfiguration(
				cfg =>
				{
					cfg.AddProfile(new AutoMapperProfile(Assembly.GetEntryAssembly()?.GetName().Name ?? ""));
					optionalAssemblies?.ToList().ForEach(a =>
					{
						cfg.AddProfile(new AutoMapperProfile(a.GetName().Name ?? ""));
					});
				})
			.CreateMapper();
		services.AddSingleton(mapper);
		return services;
	}
}
