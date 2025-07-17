using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Security.Configuration;
using RRMonitoring.Identity.Api.Attributes;
using RRMonitoring.Identity.Application.Features.UserCodes.Generate;
using RRMonitoring.Identity.Application.Features.UserCodes.Validate;
using RRMonitoring.Identity.Application.Features.UserCodes.ValidateTwoFactor;

namespace RRMonitoring.Identity.Api.Controllers.Internal;

[InternalRoute("user-code")]
[ApiController]
[Authorize(AuthenticationSchemes = ApiKeyAuthenticationOptions.DefaultScheme)]
public class UserCodeInternalController : ControllerBase
{
	private readonly IMediator _mediator;

	public UserCodeInternalController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost]
	public Task<string> Generate([FromBody] GenerateUserCodeRequest request, CancellationToken cancellationToken)
	{
		return _mediator.Send(request, cancellationToken);
	}

	[HttpPost("validate")]
	public Task<bool> Validate([FromBody] ValidateUserCodeRequest request, CancellationToken cancellationToken)
	{
		return _mediator.Send(request, cancellationToken);
	}

	[HttpPost("2fa-validate")]
	public Task<bool> ValidateTwoFactor([FromBody] ValidateUserTwoFactorRequest request, CancellationToken cancellationToken)
	{
		return _mediator.Send(request, cancellationToken);
	}
}
