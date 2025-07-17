using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Models;
using Nomium.Core.Security.Configuration;
using RRMonitoring.Notification.Application.Features.NotificationsHistory.LastSentDate;
using RRMonitoring.Notification.Application.Features.NotificationsHistory.Search;

namespace RRMonitoring.Notification.Api.Controllers;

[Route("v{version:apiVersion}/notification-history")]
[ApiController]
[ApiVersionNeutral]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class NotificationHistoryController(IMediator mediator) : ControllerBase
{
	[HttpPost("search")]
	public async Task<PagedList<SearchNotificationHistoryResponse>> Search([FromBody] SearchNotificationHistoryRequest request)
	{
		return await mediator.Send(request);
	}

	[Authorize(AuthenticationSchemes = ApiKeyAuthenticationOptions.DefaultScheme)]
	[HttpPost("last-sent-date")]
	public Task<DateTime?> GetNotificationLastSentDate([FromBody] GetNotificationLastSentDateRequest request)
	{
		return mediator.Send(request);
	}
}
