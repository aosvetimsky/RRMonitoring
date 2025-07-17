using System.Linq;
using AutoMapper;
using Nomium.Core.Models;
using RRMonitoring.Identity.Application.Features.Users.Create;
using RRMonitoring.Identity.Application.Features.Users.CreateInternal;
using RRMonitoring.Identity.Application.Features.Users.GetByExternalId;
using RRMonitoring.Identity.Application.Features.Users.GetById;
using RRMonitoring.Identity.Application.Features.Users.GetByUserNames;
using RRMonitoring.Identity.Application.Features.Users.GetProfileById;
using RRMonitoring.Identity.Application.Features.Users.GetStatuses;
using RRMonitoring.Identity.Application.Features.Users.PartialUpdateInternal;
using RRMonitoring.Identity.Application.Features.Users.Registration;
using RRMonitoring.Identity.Application.Features.Users.Search;
using RRMonitoring.Identity.Application.Features.Users.Update;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Identity.Domain.Models;
using SystemTextJsonPatch;
using SystemTextJsonPatch.Operations;

namespace RRMonitoring.Identity.Application.Features.Users;

public class UserMapper : Profile
{
	public UserMapper()
	{
		ConfigureUserByIdInfoMapping();

		ConfigureUserProfileByIdInfoMapping();

		ConfigureUserByExternalIdInfoMapping();

		ConfigureSearchingUserInfoMapping();

		CreateMap<CreateUserRequest, CreateUserInternalRequest>();

		CreateMap<CreateUserInternalRequest, User>()
			.ForMember(dest => dest.UserName,
				opt => opt.MapFrom(src => GetUserName(src.Login, src.Email, src.PhoneNumber)))
			.ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.UserTypeId));

		CreateMap<UserRegistrationRequest, User>()
			.ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.UserName));

		ConfigureUpdatingUserMapper();

		ConfigurePartialUpdatingUserMapper();

		ConfigureUsersByUserNames();
	}

	private void ConfigureUsersByUserNames()
	{
		CreateMap<User, UserByUserNameResponse>();
	}

	private void ConfigureUserByIdInfoMapping()
	{
		CreateMap<User, UserByIdResponse>()
			.ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.UserName))
			.ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles))
			.ForMember(dest => dest.LockoutEndDate, opt => opt.MapFrom(src => src.LockoutEnd))
			.ForMember(dest => dest.Tenants, opt => opt.MapFrom(src => src.UserTenants));

		CreateMap<UserStatus, UserByIdStatusResponse>();
		CreateMap<UserType, UserByIdTypeResponse>();
		CreateMap<ExternalSource, UserByIdExternalSourceResponse>();
		CreateMap<UserStatus, UserStatusResponse>();

		CreateMap<UserRole, UserByIdRoleResponse>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RoleId))
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Role.Name))
			.ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Role.Code));
		CreateMap<TenantUser, UserByIdTenantResponse>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TenantId))
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Tenant.Name));
	}

	private void ConfigureUserProfileByIdInfoMapping()
	{
		CreateMap<User, UserProfileByIdResponse>();
	}

	private void ConfigureUserByExternalIdInfoMapping()
	{
		CreateMap<User, UserByExternalIdResponse>()
			.ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.UserName));

		CreateMap<UserStatus, UserByExternalIdStatusResponse>();
		CreateMap<UserType, UserByExternalIdTypeResponse>();
	}

	private void ConfigureSearchingUserInfoMapping()
	{
		CreateMap<SearchUsersRequest, SearchUsersCriteria>();

		CreateMap<PagedList<User>, PagedList<SearchUsersResponseItem>>();

		CreateMap<User, SearchUsersResponseItem>()
			.ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.UserName))
			.ForMember(dest => dest.BlockedByAdmin, opt => opt.MapFrom(src => src.BlockedUser.UserName))
			.ForMember(dest => dest.Roles, opt =>
				opt.MapFrom(src => src.UserRoles.Select(r => r.Role)))
			.ForMember(dest => dest.LockoutEndDate, opt => opt.MapFrom(src => src.LockoutEnd));

		CreateMap<UserStatus, SearchUserStatusResponse>();
		CreateMap<Role, SearchUserRoleResponse>();
	}

	private void ConfigureUpdatingUserMapper()
	{
		CreateMap<UpdateUserRequest, User>()
			.ForMember(dest => dest.UserName,
				opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Login) ? src.Login : src.Email))
			.ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.UserTypeId))
			.ForMember(dest => dest.EmailConfirmed,
				opt => opt.Condition((src, dest) => src.EmailConfirmed is not null))
			.ForMember(dest => dest.PhoneNumberConfirmed,
				opt => opt.Condition((src, dest) => src.PhoneNumberConfirmed is not null))
			.AfterMap((src, dest) =>
			{
				dest.UserRoles = src.RoleIds?.Select(x => new UserRole { RoleId = x, UserId = dest.Id })
					.ToList();

				dest.UserTenants = src.TenantIds?.Select(x => new TenantUser { TenantId = x, UserId = dest.Id })
					.ToList();
			});
	}

	private void ConfigurePartialUpdatingUserMapper()
	{
		CreateMap<PartialUpdateUserInternalRequest, User>();
		CreateMap(typeof(JsonPatchDocument<>), typeof(JsonPatchDocument<>));
		CreateMap(typeof(Operation), typeof(Operation<>));
	}

	private static string GetUserName(string login, string email, string phoneNumber)
	{
		if (!string.IsNullOrEmpty(login))
		{
			return login;
		}

		if (!string.IsNullOrEmpty(email))
		{
			return email;
		}

		return phoneNumber;
	}
}
