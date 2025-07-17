using System.Linq;
using AutoMapper;
using Nomium.Core.Models;
using RRMonitoring.Equipment.Domain.Entities;
using RRMonitoring.Equipment.Domain.Models.EquipmentModel;
using RRMonitoring.Equipment.PublicModels.EquipmentModels;

namespace RRMonitoring.Equipment.Application.Features.EquipmentModels;

public class EquipmentModelMapper : Profile
{
	public EquipmentModelMapper()
	{
		CreateMap<SearchEquipmentModelsRequestDto, SearchEquipmentModelsCriteria>();
		CreateMap<EquipmentModel, SearchEquipmentModelsResponseDto>()
			.ForMember(x => x.ManufacturerName, opt => opt.MapFrom(x => x.Manufacturer.Name))
			.ForMember(x => x.HashrateUnitName, opt => opt.MapFrom(x => x.HashrateUnit.Name))
			.ForMember(x => x.CoinIds, opt => opt.MapFrom(x => x.EquipmentModelCoins));
		CreateMap<PagedList<EquipmentModel>, PagedList<SearchEquipmentModelsResponseDto>>();

		CreateMap<EquipmentModel, EquipmentModelByIdResponseDto>()
			.ForMember(x => x.ManufacturerId, opt => opt.MapFrom(x => x.Manufacturer.Id))
			.ForMember(x => x.HashrateUnitId, opt => opt.MapFrom(x => x.HashrateUnit.Id))
			.ForMember(x => x.CoinIds, opt => opt.MapFrom(x => x.EquipmentModelCoins.Select(x => x.CoinId)));
	}
}
