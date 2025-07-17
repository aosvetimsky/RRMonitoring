using AutoMapper;
using Nomium.Core.Models;
using RRMonitoring.Notification.Application.Features.NotificationsHistory.Search;
using RRMonitoring.Notification.Application.Providers.Models;
using RRMonitoring.Notification.Domain.Entities;
using RRMonitoring.Notification.Domain.Models;

namespace RRMonitoring.Notification.Application.Features.NotificationsHistory;

public class NotificationHistoryMapper : Profile
{
	public NotificationHistoryMapper()
	{
		CreateMap<NotificationMessageHistory, SearchNotificationHistoryResponse>()
			.ForMember(x => x.RecipientId, opt => opt.MapFrom(src => src.NotificationMessage.RecipientId))
			.ForMember(x => x.RecipientAddress, opt => opt.MapFrom(src => src.NotificationMessage.RecipientAddress))
			.ForMember(x => x.ExternalMessageId, opt => opt.MapFrom(src => src.NotificationMessage.ExternalMessageId))
			.ForMember(x => x.GroupId, opt => opt.MapFrom(src => src.NotificationMessage.Notification.GroupId))
			.ForMember(x => x.ChannelId, opt => opt.MapFrom(src => src.NotificationMessage.ChannelId))
			.ForMember(x => x.NotificationId, opt => opt.MapFrom(src => src.NotificationMessage.NotificationId))
			.ForMember(x => x.NotificationBody, opt => opt.MapFrom(src => src.NotificationMessage.NotificationBody))
			.ForMember(x => x.ErrorText, opt => opt.MapFrom(src => src.ErrorText));

		CreateMap<PagedList<NotificationMessageHistory>, PagedList<SearchNotificationHistoryResponse>>();
		CreateMap<SearchNotificationHistoryRequest, SearchNotificationMessageHistoryCriteria>();

		CreateMap<NotificationResult, NotificationMessageHistory>()
			.ForMember(dest => dest.NotificationMessage, opt => opt.Ignore())
			.ForMember(dest => dest.Status, opt => opt.Ignore())
			.ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => (byte)src.Status))
			.ForMember(dest => dest.ErrorText, opt => opt.MapFrom(src => src.Error))
			.ForMember(dest => dest.ExternalSystemStatus, opt => opt.MapFrom(src => src.ExternalStatus));

		CreateMap<NotificationResult, NotificationMessage>()
			.ForMember(dest => dest.NotificationBody, opt => opt.MapFrom(src => src.Body))
			.ForMember(dest => dest.RecipientAddress, opt => opt.MapFrom(src => src.Recipient));
	}
}
