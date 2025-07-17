using AutoMapper;
using RRMonitoring.Equipment.Domain.Entities;
using RRMonitoring.Equipment.PublicModels.HashrateUnits;

namespace RRMonitoring.Equipment.Application.Features.HashrateUnits;

public class HashrateUnitMapper : Profile
{
	public HashrateUnitMapper()
	{
		CreateMap<HashrateUnit, HashrateUnitResponseDto>();
	}
}
