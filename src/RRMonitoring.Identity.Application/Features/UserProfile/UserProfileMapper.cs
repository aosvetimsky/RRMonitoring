using AutoMapper;
using RRMonitoring.Identity.Application.Features.UserProfile.GetInfo;
using RRMonitoring.Identity.Application.Features.UserProfile.UpdateInfo;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Features.UserProfile;

public class UserProfileMapper : Profile
{
	public UserProfileMapper()
	{
		CreateMap<UpdateCurrentUserInfoRequest, User>();

		CreateMap<User, CurrentUserInfoResponse>()
			.ForMember(dest => dest.IsTwoFactorEnabled, opt => opt.MapFrom(src => src.TwoFactorEnabled));
	}
}
