using System.Linq;
using AutoMapper;
using Nomium.Core.Models;
using RRMonitoring.Identity.Application.Features.Roles.GetByCode;
using RRMonitoring.Identity.Application.Features.Roles.GetById;
using RRMonitoring.Identity.Application.Features.Roles.Search;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Identity.Domain.Models;

namespace RRMonitoring.Identity.Application.Features.Roles;

internal class RoleMapper : Profile
{
	public RoleMapper()
	{
		MapGet();
		MapGetByCodes();
		MapSearch();
	}

	private void MapSearch()
	{
		CreateMap<SearchRolesRequest, SearchRolesCriteria>();
		CreateMap<PagedList<Role>, PagedList<SearchRolesResponseItem>>();
		CreateMap<Role, SearchRolesResponseItem>();
	}

	private void MapGet()
	{
		CreateMap<Role, RoleResponse>()
			.ForMember(dest => dest.PermissionIds,
				opt => opt.MapFrom(src => src.RolePermissions.Select(x => x.PermissionId)));

		CreateMap<Tenant, RoleTenant>();
	}

	private void MapGetByCodes()
	{
		CreateMap<Role, RolesByCodesResponse>()
			.ForMember(dest => dest.PermissionIds,
				opt => opt.MapFrom(src => src.RolePermissions.Select(x => x.PermissionId)));

		CreateMap<Tenant, InternalRoleTenant>();
	}
}
