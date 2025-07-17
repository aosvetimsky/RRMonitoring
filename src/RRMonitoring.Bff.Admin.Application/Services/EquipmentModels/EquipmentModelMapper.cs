using AutoMapper;
using RRMonitoring.Bff.Admin.Application.Services.EquipmentModels.Models;
using RRMonitoring.Equipment.PublicModels.EquipmentModels;

namespace RRMonitoring.Bff.Admin.Application.Services.EquipmentModels;

internal class EquipmentModelMapper : Profile
{
	public EquipmentModelMapper()
	{
		CreateMap<SearchEquipmentModelsRequest, SearchEquipmentModelsRequestDto>();

		CreateMap<EquipmentModelByIdResponseDto, EquipmentModelByIdResponse>();

		CreateMap<CreateEquipmentModelRequest, CreateEquipmentModelRequestDto>();

		CreateMap<UpdateEquipmentModelRequest, UpdateEquipmentModelRequestDto>();
	}
}
