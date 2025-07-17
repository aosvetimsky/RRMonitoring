namespace RRMonitoring.Identity.Application.Services.ForgotPassword.Models;

public sealed record ForgotPasswordResendCodeRequest
{
	public string Login { get; init; }

	public bool IsViaEmail { get; init; }
}
