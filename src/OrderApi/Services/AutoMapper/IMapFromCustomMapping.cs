using AutoMapper;

namespace Order.Services.AutoMapper;

public interface IMapFromCustomMapping
{
	void CreateMappings(IProfileExpression profileExpression);
}
