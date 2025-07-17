using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Nomium.Core.Exceptions;
using RRMonitoring.Notification.Domain.Contracts;

namespace RRMonitoring.Notification.Application.Features.NotificationGroups.Update;

public class UpdateNotificationGroupRequest : IRequest
{
	public int Id { get; set; }
	public string Name { get; set; }
}

public class UpdateNotificationGroupHandler(INotificationGroupRepository notificationGroupRepository)
	: IRequestHandler<UpdateNotificationGroupRequest>
{
	public async Task<Unit> Handle(UpdateNotificationGroupRequest request, CancellationToken cancellationToken)
	{
		var notificationGroup =
			await notificationGroupRepository.GetById(request.Id, cancellationToken: cancellationToken);

		if (notificationGroup is null)
		{
			throw new ValidationException($"Группа с ID {notificationGroup.Id} не найдена");
		}

		notificationGroup.Name = request.Name;

		await notificationGroupRepository.Update(notificationGroup, cancellationToken);

		return Unit.Value;
	}
}
