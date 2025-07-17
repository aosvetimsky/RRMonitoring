using System;

namespace RRMonitoring.Identity.Application.Services.ForgotPassword.Models;

public sealed record ForgotPasswordVerifyCodeResponse
{
	public Guid UserId { get; set; }

	public string ResetPasswordToken { get; init; }
}
