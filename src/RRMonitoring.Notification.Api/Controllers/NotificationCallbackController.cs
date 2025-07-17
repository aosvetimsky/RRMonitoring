using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RRMonitoring.Notification.Application.Features.NotificationMessages.ApproveReceiving;
using RRMonitoring.Notification.Application.Features.NotificationMessages.ReceiveCallback;
using RRMonitoring.Notification.Application.Providers.Email.MailoPost.Models;
using RRMonitoring.Notification.Application.Providers.Sms.SmsAero.Models;
using RRMonitoring.Notification.Domain.Enums;

namespace RRMonitoring.Notification.Api.Controllers;

[Route("v{version:apiVersion}/notification-callback")]
[ApiController]
[ApiVersionNeutral]
public class NotificationCallbackController(IMediator mediator) : ControllerBase
{
	// TODO: Think about auth
	[HttpPost("mailo-post")]
	public async Task CallbackMailoPost(MailoPostCallbackInfo eventData)
	{
		var receiveCallbackRequest = new ReceiveCallbackRequest { CallbackInfo = eventData, Channel = Channels.Email };

		await mediator.Send(receiveCallbackRequest);
	}

	[HttpPost("sms-aero")]
	public async Task CallbackSmsAero([FromForm] SmsAeroCallbackInfo eventData)
	{
		var receiveCallbackRequest = new ReceiveCallbackRequest { CallbackInfo = eventData, Channel = Channels.Sms };

		await mediator.Send(receiveCallbackRequest);
	}

	[Authorize]
	[HttpPost("push")]
	public async Task ApproveReceiving([FromBody] ApproveReceivingRequest approveReceivingRequest)
	{
		await mediator.Send(approveReceivingRequest);
	}
}
