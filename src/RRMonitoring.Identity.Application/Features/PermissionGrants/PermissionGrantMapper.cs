using System.Linq;
using AutoMapper;
using Nomium.Core.Models;
using RRMonitoring.Identity.Application.Features.PermissionGrants.Create;
using RRMonitoring.Identity.Application.Features.PermissionGrants.GetById;
using RRMonitoring.Identity.Application.Features.PermissionGrants.Search;
using RRMonitoring.Identity.Application.Features.PermissionGrants.Update;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Identity.Domain.Models;

namespace RRMonitoring.Identity.Application.Features.PermissionGrants;

public class PermissionGrantMapper : Profile
{
	public PermissionGrantMapper()
	{
		CreateMap<User, PermissionGrantUserResponse>()
			.ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.MiddleName} {src.LastName}"));
		CreateMap<PermissionGrant, PermissionGrantResponse>()
			.ForMember(dest => dest.PermissionIds, opt => opt.MapFrom(src => src.GrantedPermissions.Select(x => x.PermissionId)))
			.ForMember(dest => dest.GrantDates, opt => opt.MapFrom(src => new DateTimePeriod(src.StartDate, src.EndDate)));

		CreateMap<SearchPermissionGrantsRequest, SearchPermissionGrantsPagedCriteria>();
		CreateMap<User, SearchPermissionGrantsUserResponse>()
			.ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.MiddleName} {src.LastName}"));
		CreateMap<PermissionGrantPermission, SearchPermissionGrantPermissionResponseItem>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PermissionId))
			.ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Permission.DisplayName));
		CreateMap<PermissionGrant, SearchPermissionGrantsResponseItem>()
			.ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.GrantedPermissions))
			.ForMember(dest => dest.GrantDates, opt => opt.MapFrom(src => new DateTimePeriod(src.StartDate, src.EndDate)));
		CreateMap<PagedList<PermissionGrant>, PagedList<SearchPermissionGrantsResponseItem>>();

		CreateMap<CreatePermissionGrantRequest, SearchPermissionGrantsCriteria>()
			.ForMember(dest => dest.DestinationUserIds, opt => opt.Ignore());
		CreateMap<CreatePermissionGrantRequest, PermissionGrant>()
			.ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.GrantDates.StartDateTime))
			.ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.GrantDates.EndDateTime))
			.AfterMap((src, dest) =>
			{
				dest.GrantedPermissions = src.PermissionIds
					.Select(permissionId => new PermissionGrantPermission
					{
						PermissionId = permissionId
					})
					.ToList();
			});

		CreateMap<UpdatePermissionGrantRequest, SearchPermissionGrantsCriteria>()
			.ForMember(dest => dest.DestinationUserIds, opt => opt.Ignore());
		CreateMap<UpdatePermissionGrantRequest, PermissionGrant>()
			.ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.GrantDates.StartDateTime))
			.ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.GrantDates.EndDateTime))
			.AfterMap((src, dest) =>
			{
				dest.GrantedPermissions = src.PermissionIds
					.Select(permissionId => new PermissionGrantPermission
					{
						PermissionId = permissionId
					})
					.ToList();
			});
	}
}
