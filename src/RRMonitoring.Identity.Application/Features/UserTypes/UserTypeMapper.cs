using AutoMapper;
using RRMonitoring.Identity.Application.Features.UserTypes.GetAll;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Features.UserTypes;

public class UserTypeMapper : Profile
{
	public UserTypeMapper()
	{
		CreateMap<UserType, UserTypeResponse>();
	}
}
