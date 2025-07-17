using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using RRMonitoring.Notification.Application.Services.Notification;
using RRMonitoring.Notification.Domain.Enums;

namespace RRMonitoring.Notification.Application.Features.Notification.SendManual;

public class SendManualNotificationRequest : IRequest
{
	public string Subject { get; set; }

	public string Text { get; set; }

	public Channels Channel { get; set; }

	public IList<string> Recipients { get; set; }

	public IList<IFormFile> Attachments { get; set; }
}

public class SendManualNotificationHandler(INotificationService notificationService)
	: IRequestHandler<SendManualNotificationRequest>
{
	public async Task<Unit> Handle(SendManualNotificationRequest request, CancellationToken cancellationToken)
	{
		await notificationService.Send(request);

		return Unit.Value;
	}
}
