using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RRMonitoring.Identity.Application.Features.Scope.GetAvailable;

namespace RRMonitoring.Identity.Api.Controllers;

[Route("scope")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ScopeController : Controller
{
	private readonly IMediator _mediator;

	public ScopeController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("available")]
	public async Task<IList<ScopeResponse>> GetAvailable()
	{
		return await _mediator.Send(new GetAvailableScopeRequest());
	}
}
