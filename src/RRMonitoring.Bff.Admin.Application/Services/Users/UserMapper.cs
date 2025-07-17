using AutoMapper;
using Nomium.Core.Models;
using RRMonitoring.Bff.Admin.Application.Services.Users.Models;
using SearchRolesResponseItem = RRMonitoring.Bff.Admin.Application.Services.Users.Models.SearchRolesResponseItem;

namespace RRMonitoring.Bff.Admin.Application.Services.Users;

public class UserMapper : Profile
{
	public UserMapper()
	{
		CreateMap<BlockUserRequest, Identity.ApiClients.Models.Users.ChangeUserBlockRequest>();
		CreateMap<SearchUserRoleRequest, Identity.ApiClients.Models.Roles.SearchRolesRequest>();
		CreateMap<SearchUsersRequest, Identity.ApiClients.Models.Users.SearchUsersRequest>();

		CreateMap<Identity.ApiClients.Models.Users.SearchUsersResponseItem, SearchUsersResponseItem>();
		CreateMap<Identity.ApiClients.Models.Users.SearchUserRoleResponse, SearchUserRoleResponse>();
		CreateMap<Identity.ApiClients.Models.Users.SearchUserStatusResponse, SearchUserStatusResponse>();
		CreateMap<Identity.ApiClients.Models.Roles.SearchRolesResponseItem, SearchRolesResponseItem>();
		CreateMap<Identity.ApiClients.Models.UserStatus.UserStatusResponse, UserStatusResponse>();
		CreateMap<PagedList<Identity.ApiClients.Models.Users.SearchUsersResponseItem>, PagedList<SearchUsersResponseItem>>();
		CreateMap<PagedList<Identity.ApiClients.Models.Roles.SearchRolesResponseItem>, PagedList<SearchRolesResponseItem>>();
	}
}
