using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Security.Configuration;
using RRMonitoring.Notification.Application.Features.Notification.Send;
using RRMonitoring.Notification.Application.Features.Notification.SendManual;

namespace RRMonitoring.Notification.Api.Controllers;

[Route("v{version:apiVersion}/notification")]
[ApiController]
[ApiVersionNeutral]
[Authorize(AuthenticationSchemes = $"{JwtBearerDefaults.AuthenticationScheme}, {ApiKeyAuthenticationOptions.DefaultScheme}")]
public class SendNotificationController(IMediator mediator) : ControllerBase
{
	[HttpPost("send")]
	public async Task Send([FromForm] SendNotificationRequest sendNotificationRequest)
	{
		await mediator.Send(sendNotificationRequest);
	}

	[HttpPost("send-multiple")]
	public async Task Send([FromForm] SendMultipleNotificationRequest sendMultipleNotificationRequests)
	{
		foreach (var request in sendMultipleNotificationRequests.NotificationRequests)
		{
			await mediator.Send(request);
		}
	}

	[HttpPost("send-manual")]
	public async Task SendManualNotification([FromForm] SendManualNotificationRequest sendManualNotificationRequest)
	{
		await mediator.Send(sendManualNotificationRequest);
	}
}
