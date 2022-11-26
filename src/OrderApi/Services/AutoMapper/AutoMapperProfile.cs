using System.Reflection;
using AutoMapper;

namespace Order.Services.AutoMapper;

public class AutoMapperProfile : Profile
{
	private const string profileName = "AutoMapperProfileMappings";
	public AutoMapperProfile() : this(profileName)
	{
	}

	public AutoMapperProfile(string assemblyString) : base(profileName)
	{
		if (string.IsNullOrEmpty(assemblyString))
			throw new ArgumentNullException(nameof(assemblyString));

		MapByConvention(assemblyString);
	}

	public AutoMapperProfile(AssemblyName assemblyRef) : base(profileName)
	{
		if (assemblyRef == null)
			throw new ArgumentNullException(nameof(assemblyRef));

		MapByConvention(assemblyRef);
	}
	private void MapByConvention(AssemblyName assemblyRef)
	{
		var types = Assembly.Load(assemblyRef).GetExportedTypes();

		LoadStandardMappings(types);
		LoadCustomMappings(types);
	}

	private void MapByConvention(string assemblyString)
	{
		var types = Assembly.Load(assemblyString).GetExportedTypes();

		LoadStandardMappings(types);
		LoadCustomMappings(types);
	}

	private void LoadStandardMappings(IEnumerable<Type> types)
	{
		var maps = (from t in types
					from i in t.GetInterfaces()
					where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>) && !t.IsAbstract && !t.IsInterface
					select new
					{
						Source = i.GetGenericArguments()[0],
						Destination = t
					}).ToArray();

		foreach (var map in maps)
		{
			CreateMap(map.Source, map.Destination);
			CreateMap(map.Destination, map.Source);
		}
	}

	private void LoadCustomMappings(IEnumerable<Type> types)
	{
		var maps = (from t in types
					from i in t.GetInterfaces()
					where typeof(IMapFromCustomMapping).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface
					select Activator.CreateInstance(t) as IMapFromCustomMapping).ToArray();

		foreach (var map in maps)
		{
			map.CreateMappings(this);
		}
	}
}
