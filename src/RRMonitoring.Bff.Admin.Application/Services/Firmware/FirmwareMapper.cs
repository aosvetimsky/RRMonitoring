using AutoMapper;
using Nomium.Core.Models;
using RRMonitoring.Bff.Admin.Application.Services.Firmware.Models;
using RRMonitoring.Equipment.PublicModels.Firmware;

namespace RRMonitoring.Bff.Admin.Application.Services.Firmware;

internal class FirmwareMapper : Profile
{
	public FirmwareMapper()
	{
		CreateMap<SearchFirmwareRequest, SearchFirmwareRequestDto>();
		CreateMap<SearchFirmwareResponseDto, SearchFirmwareResponse>();
		CreateMap<SearchFirmwareEquipmentModelResponseDto, SearchFirmwareEquipmentModelResponse>();
		CreateMap<PagedList<SearchFirmwareResponseDto>, PagedList<SearchFirmwareResponse>>();

		CreateMap<UpdateFirmwareRequest, UpdateFirmwareRequestDto>();

		CreateMap<FirmwareByIdResponseDto, FirmwareByIdResponse>();
	}
}
