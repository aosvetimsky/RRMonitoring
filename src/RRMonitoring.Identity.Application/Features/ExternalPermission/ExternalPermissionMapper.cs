using AutoMapper;
using RRMonitoring.Identity.Application.Features.ExternalPermission.GetAll;

namespace RRMonitoring.Identity.Application.Features.ExternalPermission;

public class ExternalPermissionMapper : Profile
{
	public ExternalPermissionMapper()
	{
		CreateMap<Domain.Entities.ExternalPermission, ExternalPermissionResponse>();
	}
}
