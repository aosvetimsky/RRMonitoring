using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using RRMonitoring.Notification.Domain.Contracts;
using RRMonitoring.Notification.Domain.Entities;

namespace RRMonitoring.Notification.Application.Features.NotificationGroups.Create;

public class CreateNotificationGroupRequest : IRequest<int>
{
	public string Name { get; set; }
}

public class CreateNotificationGroupHandler(
	INotificationGroupRepository notificationGroupRepository,
	IMapper mapper)
	: IRequestHandler<CreateNotificationGroupRequest, int>
{
	public async Task<int> Handle(CreateNotificationGroupRequest request, CancellationToken cancellationToken)
	{
		var notificationGroup = mapper.Map<NotificationGroup>(request);

		var addedGroup = await notificationGroupRepository.Add(notificationGroup, cancellationToken);

		return addedGroup.Id;
	}
}
