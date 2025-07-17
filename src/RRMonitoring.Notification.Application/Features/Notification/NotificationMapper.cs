using AutoMapper;
using Nomium.Core.Models;
using RRMonitoring.Notification.Application.Features.Notification.Create;
using RRMonitoring.Notification.Application.Features.Notification.GetById;
using RRMonitoring.Notification.Application.Features.Notification.Search;
using RRMonitoring.Notification.Application.Features.Notification.Update;
using RRMonitoring.Notification.Application.Providers.Models;
using RRMonitoring.Notification.Domain.Entities;
using RRMonitoring.Notification.Domain.Enums;
using RRMonitoring.Notification.Domain.Models;
using NotificationEntry = RRMonitoring.Notification.Domain.Entities.Notification;

namespace RRMonitoring.Notification.Application.Features.Notification;

public class NotificationMapper : Profile
{
	public NotificationMapper()
	{
		CreateMap<Template, NotificationTemplateResponseItem>()
			.ForMember(dest => dest.ChannelId, opt => opt.MapFrom(src => (Channels)src.ChannelId));
		CreateMap<NotificationEntry, NotificationInfoResponse>();

		CreateMap<SearchNotificationsRequest, SearchNotificationsCriteria>();
		CreateMap<PagedList<SearchNotificationsRequest>, PagedList<SearchNotificationsCriteria>>();
		CreateMap<NotificationEntry, SearchNotificationsResponse>();
		CreateMap<PagedList<NotificationEntry>, PagedList<SearchNotificationsResponse>>();

		CreateMap<CreateNotificationTemplateItem, Template>()
			.ForMember(dest => dest.ChannelId, opt => opt.MapFrom(src => (byte)src.ChannelId));
		CreateMap<CreateNotificationRequest, NotificationEntry>();

		CreateMap<UpdateNotificationTemplateItem, Template>()
			.ForMember(dest => dest.ChannelId, opt => opt.MapFrom(src => (byte)src.ChannelId));
		CreateMap<UpdateNotificationRequest, NotificationEntry>();

		CreateMap<NotificationInfo, NotificationResult>();
	}
}
