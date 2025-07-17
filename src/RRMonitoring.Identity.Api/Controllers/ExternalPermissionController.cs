using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RRMonitoring.Identity.Application.Features.ExternalPermission.GetAll;

namespace RRMonitoring.Identity.Api.Controllers;

[Route("external-permission")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ExternalPermissionController : ControllerBase
{
	private readonly IMediator _mediator;

	public ExternalPermissionController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	public async Task<List<ExternalPermissionResponse>> GetAll(CancellationToken cancellationToken)
	{
		return await _mediator.Send(new GetAllExternalPermissionsRequest(), cancellationToken);
	}
}
