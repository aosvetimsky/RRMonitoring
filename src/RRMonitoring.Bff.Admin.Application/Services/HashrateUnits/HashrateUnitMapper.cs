using AutoMapper;
using RRMonitoring.Bff.Admin.Application.Services.HashrateUnits.Models;
using RRMonitoring.Equipment.PublicModels.HashrateUnits;

namespace RRMonitoring.Bff.Admin.Application.Services.HashrateUnits;

public class HashrateUnitMapper : Profile
{
	public HashrateUnitMapper()
	{
		CreateMap<HashrateUnitResponseDto, HashrateUnitResponse>();
	}
}
