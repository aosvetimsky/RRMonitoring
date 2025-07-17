using AutoMapper;
using Nomium.Core.Models;
using RRMonitoring.Bff.Admin.Application.Services.Equipment.Models;
using RRMonitoring.Equipment.PublicModels.Manufacturers;

namespace RRMonitoring.Bff.Admin.Application.Services.Equipment;

public class EquipmentMapper : Profile
{
	public EquipmentMapper()
	{
		CreateMap<SearchEquipmentManufacturersRequest, SearchManufacturersRequestDto>();
		CreateMap<SearchManufacturersResponseDto, SearchEquipmentManufacturersResponse>();
		CreateMap<PagedList<SearchManufacturersResponseDto>, PagedList<SearchEquipmentManufacturersResponse>>();
	}
}
