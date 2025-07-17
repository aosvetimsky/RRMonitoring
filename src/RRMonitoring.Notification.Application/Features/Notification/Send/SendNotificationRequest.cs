using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using RRMonitoring.Notification.Application.Services.Notification;
using RRMonitoring.Notification.Domain.Enums;

namespace RRMonitoring.Notification.Application.Features.Notification.Send;

public class SendNotificationRequest : IRequest
{
	public string Identifier { get; set; }

	public Channels Channel { get; set; }

	public string Recipient { get; set; }

	public string RecipientId { get; set; }

	public string UserData { get; set; }

	public IList<IFormFile> Attachments { get; set; }
}

public class SendNotificationHandler(INotificationService notificationService)
	: IRequestHandler<SendNotificationRequest>
{
	public async Task<Unit> Handle(SendNotificationRequest request, CancellationToken cancellationToken)
	{
		await notificationService.Send(request);

		return Unit.Value;
	}
}
