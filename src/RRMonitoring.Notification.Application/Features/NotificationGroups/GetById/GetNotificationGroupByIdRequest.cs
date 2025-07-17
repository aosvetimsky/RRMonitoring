using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using RRMonitoring.Notification.Domain.Contracts;

namespace RRMonitoring.Notification.Application.Features.NotificationGroups.GetById;

public class GetNotificationGroupByIdRequest(int id)
	: IRequest<NotificationGroupResponse>
{
	public int Id { get; set; } = id;
}

public class GetNotificationGroupByIdHandler(
	INotificationGroupRepository notificationGroupRepository,
	IMapper mapper)
	: IRequestHandler<GetNotificationGroupByIdRequest, NotificationGroupResponse>
{
	public async Task<NotificationGroupResponse> Handle(
		GetNotificationGroupByIdRequest request, CancellationToken cancellationToken)
	{
		var response = await notificationGroupRepository.GetById(request.Id,
			includePaths: null, asNoTracking: true, cancellationToken);

		return mapper.Map<NotificationGroupResponse>(response);
	}
}
