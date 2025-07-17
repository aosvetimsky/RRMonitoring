namespace RRMonitoring.Identity.Application.Services.ForgotPassword.Models;

public sealed record ForgotPasswordVerifyCodeRequest
{
	public string Email { get; init; }

	public string VerificationCode { get; init; }
}
