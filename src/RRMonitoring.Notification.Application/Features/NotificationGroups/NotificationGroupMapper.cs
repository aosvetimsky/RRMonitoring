using AutoMapper;
using RRMonitoring.Notification.Application.Features.NotificationGroups.Create;
using RRMonitoring.Notification.Application.Features.NotificationGroups.GetById;
using RRMonitoring.Notification.Application.Features.NotificationGroups.Update;
using RRMonitoring.Notification.Domain.Entities;

namespace RRMonitoring.Notification.Application.Features.NotificationGroups;

public class NotificationGroupMapper : Profile
{
	public NotificationGroupMapper()
	{
		CreateMap<NotificationGroup, NotificationGroupResponse>();

		CreateMap<CreateNotificationGroupRequest, NotificationGroup>();
		CreateMap<UpdateNotificationGroupRequest, NotificationGroup>();
	}
}
