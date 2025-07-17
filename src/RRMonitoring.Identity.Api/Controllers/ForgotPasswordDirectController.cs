using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Api.Helpers;
using RRMonitoring.Identity.Application.Features.ResetPassword;
using RRMonitoring.Identity.Application.Services.ForgotPassword;
using RRMonitoring.Identity.Application.Services.ForgotPassword.Models;

namespace RRMonitoring.Identity.Api.Controllers;

[ApiController]
[Route("forgot-password-direct")]
[AllowAnonymous]
public class ForgotPasswordDirectController : ControllerBase
{
	private readonly IForgotPasswordService _forgotPasswordService;
	private readonly IMediator _mediator;

	public ForgotPasswordDirectController(
		IForgotPasswordService forgotPasswordService,
		IMediator mediator)
	{
		_forgotPasswordService = forgotPasswordService;
		_mediator = mediator;
	}

	[HttpGet("verification-time")]
	public async Task<int> GetLastVerificationEmailTime(
		[FromQuery] string login,
		CancellationToken cancellationToken)
	{
		return await _forgotPasswordService.GetForgotPasswordCodeTimeout(login, cancellationToken);
	}

	[HttpPost]
	public Task<ForgotPasswordResponse> ForgotPassword(
		[FromBody] ForgotPasswordRequest request,
		CancellationToken cancellationToken)
	{
		request = request with
		{
			Login = PhoneNumberHelper.ModifyPhoneCountryCode(request.Login.Trim())
		};

		return _forgotPasswordService.ForgotPasswordDirect(request, cancellationToken);
	}

	[HttpPost("verify-code")]
	public Task<ForgotPasswordVerifyCodeResponse> VerifyCode(
		[FromBody] ForgotPasswordVerifyCodeRequest request,
		CancellationToken cancellationToken)
	{
		return _forgotPasswordService.VerifyCode(request, cancellationToken);
	}

	[HttpPost("resend-code")]
	public Task<int> ResendCode(
		[FromBody] ForgotPasswordResendCodeRequest request,
		CancellationToken cancellationToken)
	{
		return _forgotPasswordService.ResendCode(request, cancellationToken);
	}

	[HttpPost("reset-password")]
	public async Task ResetPassword(
		[FromBody] ResetPasswordRequest request,
		CancellationToken cancellationToken)
	{
		var result = await _mediator.Send(request, cancellationToken);
		if (!result.IsSuccess)
		{
			throw new ValidationException(result.ErrorMessage);
		}
	}
}
