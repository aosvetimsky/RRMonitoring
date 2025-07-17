using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Security.Configuration;
using RRMonitoring.Identity.Api.Attributes;
using RRMonitoring.Identity.Application.Features.UserTypes.GetAll;

namespace RRMonitoring.Identity.Api.Controllers.Internal;

[InternalRoute("user-type")]
[ApiController]
[Authorize(AuthenticationSchemes = ApiKeyAuthenticationOptions.DefaultScheme)]
public class UserTypeInternalController : ControllerBase
{
	private readonly IMediator _mediator;

	public UserTypeInternalController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	public Task<IList<UserTypeResponse>> GetAll()
	{
		return _mediator.Send(new GetUserTypesRequest());
	}
}
