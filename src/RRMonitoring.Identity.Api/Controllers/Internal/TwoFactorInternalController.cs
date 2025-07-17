using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Security.Configuration;
using RRMonitoring.Identity.Api.Attributes;
using RRMonitoring.Identity.Application.Features.GoogleAuth.Reset;
using RRMonitoring.Identity.Application.Features.GoogleAuth.SendResetCode;

namespace RRMonitoring.Identity.Api.Controllers.Internal;

[InternalRoute("two-factor")]
[ApiController]
[Authorize(AuthenticationSchemes = ApiKeyAuthenticationOptions.DefaultScheme)]
public class TwoFactorInternalController : ControllerBase
{
	private readonly IMediator _mediator;

	public TwoFactorInternalController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost("send-reset-code")]
	public async Task SendResetAuthenticatorCode(
		[FromBody] SendResetAuthenticatorCodeRequest request,
		CancellationToken cancellationToken)
	{
		await _mediator.Send(request, cancellationToken);
	}

	[HttpPost("reset")]
	public async Task ResetAuthenticator(
		[FromBody] ResetUserAuthenticatorRequest request,
		CancellationToken cancellationToken)
	{
		await _mediator.Send(request, cancellationToken);
	}
}
