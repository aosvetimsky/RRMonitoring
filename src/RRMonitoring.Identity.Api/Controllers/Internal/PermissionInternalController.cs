using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Security.Configuration;
using RRMonitoring.Identity.Api.Attributes;
using RRMonitoring.Identity.Application.Features.Permissions.GetGroupedPermissions;

namespace RRMonitoring.Identity.Api.Controllers.Internal;

[InternalRoute("permission")]
[ApiController]
[Authorize(AuthenticationSchemes = ApiKeyAuthenticationOptions.DefaultScheme)]
public class PermissionInternalController : ControllerBase
{
	private readonly IMediator _mediator;

	public PermissionInternalController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	public async Task<IList<PermissionGroupResponse>> GetGroupedPermissions()
	{
		return await _mediator.Send(new GetGroupedPermissionsRequest());
	}
}
