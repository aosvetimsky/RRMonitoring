using AutoMapper;
using RRMonitoring.Notification.Application.Features.RecipientSettings.GetByCurrentUser;
using RRMonitoring.Notification.Application.Features.RecipientSettings.Set;
using RRMonitoring.Notification.Domain.Entities;
using RRMonitoring.Notification.Domain.Enums;

namespace RRMonitoring.Notification.Application.Features.RecipientSettings;

public class RecipientSettingsMapper : Profile
{
	public RecipientSettingsMapper()
	{
		CreateMap<RecipientSetting, UserNotificationSettingResponse>()
			.ForMember(x => x.Channel, opt => opt.MapFrom(src => (Channels)src.ChannelId))
			.ForMember(x => x.NotificationDescription, opt => opt.MapFrom(src => src.Notification.Description));

		CreateMap<SetRecipientSettings, RecipientSetting>()
			.ForMember(x => x.Channel, opt => opt.Ignore())
			.ForMember(x => x.ChannelId, opt => opt.MapFrom(src => (byte)src.Channel));
	}
}
