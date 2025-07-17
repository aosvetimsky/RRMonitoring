using AutoMapper;
using RRMonitoring.Identity.Application.Features.Permissions.GetGroupedPermissions;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Features.Permissions;

internal class PermissionMapper : Profile
{
	public PermissionMapper()
	{
		CreateMap<Permission, PermissionResponse>()
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DisplayName));

		CreateMap<PermissionGroup, PermissionGroupResponse>();
	}
}
