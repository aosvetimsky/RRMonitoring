using AutoMapper;
using Nomium.Core.Models;
using RRMonitoring.Bff.Admin.Application.Services.Workers.Models;
using RRMonitoring.Mining.PublicModels.Workers;

namespace RRMonitoring.Bff.Admin.Application.Services.Workers;

internal class WorkerMapper : Profile
{
	public WorkerMapper()
	{
		CreateMap<SearchWorkersRequest, SearchWorkersRequestDto>();
		CreateMap<SearchWorkersResponseDto, SearchWorkersResponse>();
		CreateMap<SearchWorkersCoinResponseDto, SearchWorkersCoinResponse>();
		CreateMap<SearchWorkersStatusResponseDto, SearchWorkersStatusResponse>();
		CreateMap<PagedList<SearchWorkersResponseDto>, PagedList<SearchWorkersResponse>>();
	}
}
