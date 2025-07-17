using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Security.Configuration;
using RRMonitoring.Identity.Api.Attributes;
using RRMonitoring.Identity.Application.Features.Users.GetStatuses;

namespace RRMonitoring.Identity.Api.Controllers.Internal;

[InternalRoute("user-status")]
[ApiController]
[Authorize(AuthenticationSchemes = ApiKeyAuthenticationOptions.DefaultScheme)]
public class UserStatusInternalController : ControllerBase
{
	private readonly IMediator _mediator;

	public UserStatusInternalController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	public Task<List<UserStatusResponse>> GetAll(CancellationToken cancellationToken)
	{
		return _mediator.Send(new GetAllUserStatusesRequest(), cancellationToken);
	}
}
