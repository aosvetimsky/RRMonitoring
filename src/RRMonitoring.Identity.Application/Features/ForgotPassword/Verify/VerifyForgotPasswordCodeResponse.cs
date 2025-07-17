namespace RRMonitoring.Identity.Application.Features.ForgotPassword.Verify;

public sealed record VerifyForgotPasswordCodeResponse
{
	public string ResetPasswordToken { get; init; }
}
