using AutoMapper;
using RRMonitoring.Bff.Admin.Application.Services.Facilities.Models;
using RRMonitoring.Colocation.PublicModels.Facilities;

namespace RRMonitoring.Bff.Admin.Application.Services.Facilities;

internal class FacilityMapper : Profile
{
	public FacilityMapper()
	{
		CreateMap<SearchFacilitiesRequest, SearchFacilitiesRequestDto>();

		CreateMap<CreateFacilityRequest, CreateFacilityRequestDto>();

		CreateMap<UpdateFacilityRequest, UpdateFacilityRequestDto>();

		CreateMap<FacilityByIdResponseDto, FacilityByIdResponse>();
	}
}
