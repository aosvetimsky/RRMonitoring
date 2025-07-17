using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RRMonitoring.Identity.Application.Features.Permissions.GetGroupedPermissions;

namespace RRMonitoring.Identity.Api.Controllers;

[Route("permission")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PermissionController : Controller
{
	private readonly IMediator _mediator;

	public PermissionController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	public async Task<IList<PermissionGroupResponse>> GetGroupedPermissions()
	{
		return await _mediator.Send(new GetGroupedPermissionsRequest());
	}
}
