using AutoMapper;
using Nomium.Core.Models;
using RRMonitoring.Bff.Admin.Application.Services.Pools.Models;
using RRMonitoring.Mining.PublicModels.Pools;

namespace RRMonitoring.Bff.Admin.Application.Services.Pools;

internal class PoolMapper : Profile
{
	public PoolMapper()
	{
		CreateMap<SearchPoolsRequest, SearchPoolsRequestDto>();
		CreateMap<SearchPoolsResponseDto, SearchPoolsResponseItem>();
		CreateMap<SearchPoolsCoinResponseDto, SearchPoolsCoinResponse>();
		CreateMap<PagedList<SearchPoolsResponseDto>, PagedList<SearchPoolsResponseItem>>();

		CreateMap<CreatePoolRequest, CreatePoolRequestDto>();
		CreateMap<CreatePoolCoinAddressRequest, CreatePoolCoinAddressRequestDto>();

		CreateMap<UpdatePoolRequest, UpdatePoolRequestDto>();
		CreateMap<UpdatePoolCoinAddressRequest, UpdatePoolCoinAddressRequestDto>();

		CreateMap<PoolByIdResponseDto, PoolByIdResponse>();
		CreateMap<PoolCoinAddressResponseDto, PoolCoinAddressResponse>();
	}
}
