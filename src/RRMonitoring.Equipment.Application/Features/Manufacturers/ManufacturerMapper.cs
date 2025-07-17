using AutoMapper;
using Nomium.Core.Models;
using RRMonitoring.Equipment.Domain.Entities;
using RRMonitoring.Equipment.Domain.Models.Manufacturer;
using RRMonitoring.Equipment.PublicModels.Manufacturers;

namespace RRMonitoring.Equipment.Application.Features.Manufacturers;

public class ManufacturerMapper : Profile
{
	public ManufacturerMapper()
	{
		CreateMap<SearchManufacturersRequestDto, SearchManufacturersCriteria>();
		CreateMap<Manufacturer, SearchManufacturersResponseDto>();
		CreateMap<PagedList<Manufacturer>, PagedList<SearchManufacturersResponseDto>>();
	}
}
