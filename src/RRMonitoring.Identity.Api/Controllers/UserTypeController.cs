using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RRMonitoring.Identity.Application.Features.UserTypes.GetAll;

namespace RRMonitoring.Identity.Api.Controllers;

[Route("user-type")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserTypeController : ControllerBase
{
	private readonly IMediator _mediator;

	public UserTypeController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	public async Task<IList<UserTypeResponse>> GetAll()
	{
		return await _mediator.Send(new GetUserTypesRequest());
	}
}
