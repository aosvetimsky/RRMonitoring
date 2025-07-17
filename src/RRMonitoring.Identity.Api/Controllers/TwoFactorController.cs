using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RRMonitoring.Identity.Application.Features.GoogleAuth.GetAuthenticatorKey;
using RRMonitoring.Identity.Application.Features.GoogleAuth.UpdateAuthenticator;
using RRMonitoring.Identity.Application.Features.TwoFactor.ChangeState;
using RRMonitoring.Identity.Application.Features.TwoFactor.GetSettings;
using RRMonitoring.Identity.Application.Features.TwoFactor.SendSmsCode;

namespace RRMonitoring.Identity.Api.Controllers;

[Route("two-factor")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TwoFactorController : ControllerBase
{
	private readonly IMediator _mediator;

	public TwoFactorController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("key")]
	public Task<UserAuthenticatorKeyResponse> GetAuthenticatorKey(CancellationToken cancellationToken)
	{
		return _mediator.Send(new GetUserAuthenticatorKeyRequest(), cancellationToken);
	}

	[HttpGet("settings")]
	public async Task<GetTwoFactorSettingsResponse> GetTwoFactorSettings()
	{
		return await _mediator.Send(new GetTwoFactorSettingsRequest());
	}

	[HttpPost("send-sms-code")]
	public Task<int> SendCodeToPhone(CancellationToken cancellationToken)
	{
		return _mediator.Send(new SendTwoFactorSmsCodeRequest(), cancellationToken);
	}

	[HttpPost("authenticator")]
	public async Task SetAuthenticatorKey([FromBody] UpdateUserAuthenticatorRequest request, CancellationToken cancellationToken)
	{
		await _mediator.Send(request, cancellationToken);
	}

	[HttpPost]
	public async Task ChangeTwoFactor([FromBody] ChangeTwoFactorRequest request, CancellationToken cancellationToken)
	{
		await _mediator.Send(request, cancellationToken);
	}
}
