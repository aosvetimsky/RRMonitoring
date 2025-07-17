using AutoMapper;
using Nomium.Core.Models;
using RRMonitoring.Bff.Admin.Application.Services.Manufacturers.Models;
using RRMonitoring.Equipment.PublicModels.Manufacturers;

namespace RRMonitoring.Bff.Admin.Application.Services.Manufacturers;

internal class ManufacturerMapper : Profile
{
	public ManufacturerMapper()
	{
		CreateMap<SearchManufacturersRequest, SearchManufacturersRequestDto>();
		CreateMap<SearchManufacturersResponseDto, SearchManufacturersResponse>();
		CreateMap<PagedList<SearchManufacturersResponseDto>, PagedList<SearchManufacturersResponse>>();

		CreateMap<CreateManufacturerRequest, CreateManufacturerRequestDto>();

		CreateMap<UpdateManufacturerRequest, UpdateManufacturerRequestDto>();

		CreateMap<ManufacturerByIdResponseDto, ManufacturerByIdResponse>();
	}
}