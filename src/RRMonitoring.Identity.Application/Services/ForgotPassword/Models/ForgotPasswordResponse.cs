namespace RRMonitoring.Identity.Application.Services.ForgotPassword.Models;

public sealed record ForgotPasswordResponse
{
	public int UntilResend { get; init; }
}
