using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Models;
using RRMonitoring.Notification.Application.Features.Notification.Create;
using RRMonitoring.Notification.Application.Features.Notification.Delete;
using RRMonitoring.Notification.Application.Features.Notification.GetById;
using RRMonitoring.Notification.Application.Features.Notification.Search;
using RRMonitoring.Notification.Application.Features.Notification.Update;

namespace RRMonitoring.Notification.Api.Controllers;

[Route("v{version:apiVersion}/notification")]
[ApiController]
[ApiVersionNeutral]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class NotificationController(IMediator mediator) : ControllerBase
{
	[HttpGet("{id:guid}")]
	public Task<NotificationInfoResponse> GetById([FromRoute] Guid id)
	{
		return mediator.Send(new GetNotificationByIdRequest(id));
	}

	[HttpPost("search")]
	public Task<PagedList<SearchNotificationsResponse>> Search([FromBody] SearchNotificationsRequest request)
	{
		return mediator.Send(request);
	}

	[HttpPost]
	public async Task Create([FromBody] CreateNotificationRequest request)
	{
		await mediator.Send(request);
	}

	[HttpPut("{id:guid}")]
	public async Task Update([FromRoute] Guid id, [FromBody] UpdateNotificationRequest request)
	{
		request.Id = id;
		await mediator.Send(request);
	}

	[HttpDelete("{id:guid}")]
	public async Task Delete([FromRoute] Guid id)
	{
		await mediator.Send(new DeleteNotificationRequest(id));
	}
}
