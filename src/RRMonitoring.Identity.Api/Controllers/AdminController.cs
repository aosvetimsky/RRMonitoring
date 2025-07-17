using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Security.Attributes;
using RRMonitoring.Identity.Api.Security;
using RRMonitoring.Identity.Application.Features.Auth.Login;

namespace RRMonitoring.Identity.Api.Controllers;

[Route("admin")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AdminController : Controller
{
	private readonly IMediator _mediator;

	public AdminController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost("login-as-user/{userId:guid}")]
	[Permission(Permissions.LoginAsUser)]
	public async Task<IActionResult> LoginAsUser(Guid userId)
	{
		var loginRequest = new LoginAsUserRequest(userId);
		var result = await _mediator.Send(loginRequest);

		return result.IsSuccess
			? Ok()
			: BadRequest(result.ErrorMessage);
	}
}
