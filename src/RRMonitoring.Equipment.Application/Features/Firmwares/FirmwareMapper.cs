using System.Linq;
using AutoMapper;
using Nomium.Core.Models;
using RRMonitoring.Equipment.Domain.Entities;
using RRMonitoring.Equipment.Domain.Models.Firmware;
using RRMonitoring.Equipment.PublicModels.Firmware;

namespace RRMonitoring.Equipment.Application.Features.Firmwares;

public class FirmwareMapper : Profile
{
	public FirmwareMapper()
	{
		CreateMap<Firmware, FirmwareByIdResponseDto>()
			.ForMember(x => x.EquipmentModelIds, opt => opt.MapFrom(x => x.FirmwareEquipmentModels.Select(x => x.EquipmentModelId)));

		CreateMap<SearchFirmwareRequestDto, SearchFirmwareCriteria>();

		CreateMap<FirmwareEquipmentModel, SearchFirmwareEquipmentModelResponseDto>()
			.ForMember(x => x.Id, opt => opt.MapFrom(x => x.EquipmentModelId))
			.ForMember(x => x.Name, opt => opt.MapFrom(x => x.EquipmentModel.Name));
		CreateMap<Firmware, SearchFirmwareResponseDto>()
			.ForMember(x => x.EquipmentModels, opt => opt.MapFrom(x => x.FirmwareEquipmentModels));

		CreateMap<PagedList<Firmware>, PagedList<SearchFirmwareResponseDto>>();
	}
}
