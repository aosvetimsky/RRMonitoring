using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RRMonitoring.Notification.Application.Features.RecipientSettings.GetByCurrentUser;
using RRMonitoring.Notification.Application.Features.RecipientSettings.SetCurrentUserRecipientSettings;

namespace RRMonitoring.Notification.Api.Controllers;

[Route("v{version:apiVersion}/user-notification-settings")]
[ApiController]
[ApiVersionNeutral]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserNotificationSettingsController(IMediator mediator) : ControllerBase
{
	[HttpGet]
	public async Task<IList<UserNotificationSettingResponse>> Get()
	{
		return await mediator.Send(new GetCurrentUserNotificationSettingsRequest());
	}

	[HttpPost]
	public Task SetNotificationSettings(
		[FromBody] SetUserSettingsRequest request,
		CancellationToken cancellationToken)
	{
		return mediator.Send(request, cancellationToken);
	}
}
