using System.Linq;
using AutoMapper;
using Nomium.Core.Models;
using RRMonitoring.Colocation.Domain.Entities;
using RRMonitoring.Colocation.Domain.Models.Facility;
using RRMonitoring.Colocation.PublicModels.Facilities;

namespace RRMonitoring.Colocation.Application.Features.Facilities;

public class FacilityMapper : Profile
{
	public FacilityMapper()
	{
		CreateMap<Facility, FacilityByIdResponseDto>()
			.ForMember(x => x.TechnicianIds, opt => opt.MapFrom(x => x.Technicians.Select(x => x.UserId)));

		CreateMap<SearchFacilitiesRequestDto, SearchFacilitiesCriteria>();

		CreateMap<Facility, SearchFacilitiesResponseDto>()
			.ForMember(x => x.TechnicianIds, opt => opt.MapFrom(x => x.Technicians.Select(x => x.UserId)));

		CreateMap<PagedList<Facility>, PagedList<SearchFacilitiesResponseDto>>();
	}
}
