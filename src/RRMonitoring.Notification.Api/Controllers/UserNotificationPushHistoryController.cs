using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Models;
using RRMonitoring.Notification.Application.Features.NotificationPush.GetByCurrentUser;

namespace RRMonitoring.Notification.Api.Controllers;

[Route("v{version:apiVersion}/user-notification-push-history")]
[ApiController]
[ApiVersionNeutral]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserNotificationPushHistoryController(IMediator mediator) : ControllerBase
{
	[HttpGet]
	public async Task<PagedList<UserNotificationPushMessageResponse>> Get()
	{
		return await mediator.Send(new GetCurrentUserNotificationPushMessagesRequest());
	}
}
