using AutoMapper;
using RRMonitoring.Mining.Domain.Models.Pool;
using RRMonitoring.Mining.PublicModels.Pools;

namespace RRMonitoring.Mining.Application.Features.Pools;

public class PoolMapper : Profile
{
	public PoolMapper()
	{
		CreateMap<SearchPoolsRequestDto, SearchPoolsCriteria>();
	}
}
