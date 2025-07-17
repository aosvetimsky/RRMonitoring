using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RRMonitoring.Notification.Application.Features.RecipientSettings.Set;

namespace RRMonitoring.Notification.Api.Controllers;

[Route("v{version:apiVersion}/recipient-settings")]
[ApiController]
[ApiVersionNeutral]
public class RecipientSettingsController(IMediator mediator) : ControllerBase
{
	[HttpPost("set")]
	public async Task Set(SetRecipientSettingsRequest request)
	{
		await mediator.Send(request);
	}
}
