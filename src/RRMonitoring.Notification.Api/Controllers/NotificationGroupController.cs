using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RRMonitoring.Notification.Application.Features.NotificationGroups.Create;
using RRMonitoring.Notification.Application.Features.NotificationGroups.GetById;
using RRMonitoring.Notification.Application.Features.NotificationGroups.Update;

namespace RRMonitoring.Notification.Api.Controllers;

[Route("v{version:apiVersion}/notification-group")]
[ApiController]
[ApiVersionNeutral]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class NotificationGroupController(IMediator mediator) : ControllerBase
{
	[HttpGet("{id}")]
	public async Task<NotificationGroupResponse> GetById([FromRoute] int id)
	{
		return await mediator.Send(new GetNotificationGroupByIdRequest(id));
	}

	[HttpPost]
	public async Task<int> Create([FromBody] CreateNotificationGroupRequest request)
	{
		return await mediator.Send(request);
	}

	[HttpPut]
	public async Task Update([FromBody] UpdateNotificationGroupRequest request)
	{
		await mediator.Send(request);
	}
}
