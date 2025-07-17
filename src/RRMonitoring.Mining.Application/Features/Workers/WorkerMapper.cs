using AutoMapper;
using Nomium.Core.Models;
using RRMonitoring.Mining.Domain.Entities;
using RRMonitoring.Mining.Domain.Models.Worker;
using RRMonitoring.Mining.PublicModels.Workers;

namespace RRMonitoring.Mining.Application.Features.Workers;

public class WorkerMapper : Profile
{
	public WorkerMapper()
	{
		CreateMap<SearchWorkersRequestDto, SearchWorkersCriteria>();
		CreateMap<Coin, SearchWorkersCoinResponseDto>();
		CreateMap<WorkerStatus, SearchWorkersStatusResponseDto>();
		CreateMap<Worker, SearchWorkersResponseDto>()
			.ForMember(x => x.ClientName, opt => opt.MapFrom(x => x.Client.Name))
			.ForMember(x => x.PoolName, opt => opt.MapFrom(x => x.Pool.Name))
			.ForMember(x => x.Coin, opt => opt.MapFrom(x => x.Coin))
			.ForMember(x => x.Status, opt => opt.MapFrom(x => x.Status));
		CreateMap<PagedList<Worker>, PagedList<SearchWorkersResponseDto>>();
	}
}
